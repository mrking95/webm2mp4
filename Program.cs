using System;
using System.IO;
using System.Threading.Tasks;
using System.CommandLine.DragonFruit;
using Serilog;

namespace webm2mp4
{
    class Program
    {
        /// <summary>
        /// Converts webm file or directory containing multiple webm's to mp4
        /// </summary>
        /// <param name="input">The path to the .webm or directorty containing multiple webm's for conversion</param>
        /// <param name="output">The path for the files to be outputted (defaults to the input directory)</param>
        /// <param name="force">Overwrite if the target file exists</param>
        /// <param name="clean">Remove the original webm when conversion is complete</param>
        /// <param name="log">If specified, output will be written to log</param>
        static async Task<int> Main(string input, string output = null, bool force = true, bool clean = true, string log = null)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.ColoredConsole()
                        .WriteTo.File(log ?? Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.ToString("s").Split("T")[0]}.log"))
                        .CreateLogger();

            Log.Information($"Logging to: {Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.ToString("s").Split("T")[0]}.log")}");

            return await Convert(new DirectoryInfo(input.Trim()), output != null ? new DirectoryInfo(output) : null, clean, force);
        }

        static async Task<int> Convert(DirectoryInfo inputDir, DirectoryInfo outputDir, bool deleteOriginal, bool overwrite)
        {
            try 
            {
                WebmConverter converter = new WebmConverter(inputDir);

                var files = await converter.GetFiles();
                Log.Information($"Got {files.Count} files for conversion");
                var index = 1;
                foreach(var file in files) 
                {
                    var outputPath = outputDir != null ? outputDir : file.Directory;
                    var outputFile = new FileInfo($"{outputPath}/{Path.GetFileNameWithoutExtension(file.Name)}.mp4");
                    Log.Information($"Converting ({index}/{files.Count}) {file.FullName} to {outputFile.FullName}");
                    var conversionResult = await converter.Convert(file, outputFile, overwrite);

                    if(conversionResult && deleteOriginal) {
                        Log.Information($"Removing {file.Name}");
                        file.Delete();
                    }
                    index++;
                }
            } 
            catch (Exception ex) 
            {
                return ex.HResult;
            }
            return 0;
        }
    }

}

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
                        .WriteTo.File(log ?? Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.ToString("s")}.log"))
                        .CreateLogger();

            return await Convert(new DirectoryInfo(input.Trim()), new DirectoryInfo(output ?? input), clean, force);
        }

        static async Task<int> Convert(DirectoryInfo inputDir, DirectoryInfo outputDir, bool deleteOriginal, bool overwrite)
        {
            try 
            {
                WebmConverter converter = new WebmConverter(inputDir);

                var files = await converter.GetFiles();
                Log.Information($"Got {files.Count} files for conversion");

                foreach(var file in files) 
                {
                    Log.Information($"Converting {file.Name}");
                    var conversionResult = await converter.Convert(file, new FileInfo($"{outputDir.FullName}/{Path.GetFileNameWithoutExtension(file.Name)}.mp4"), overwrite);

                    if(conversionResult && deleteOriginal) {
                        Log.Information($"Removing {file.Name}");
                        file.Delete();
                    }
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

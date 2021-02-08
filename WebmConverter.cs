using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace webm2mp4
{
    public class WebmConverter
    {
        private readonly FileInfo _inputFile;
        private readonly DirectoryInfo _inputPath;

        public WebmConverter(DirectoryInfo inputPath)
        {
            _inputPath = inputPath;
        }

        public WebmConverter(FileInfo inputFile)
        {
            _inputFile = inputFile;
        }

        public async Task<List<FileInfo>> GetFiles()
        {
            try
            {
                DirectoryInfo dir = _inputFile?.Directory ?? _inputPath;
                var files = dir.GetFiles("*.webm", SearchOption.AllDirectories);

                return files.Where(file => string.IsNullOrEmpty(_inputFile?.Name) ? true : file.Name.Equals(_inputFile.Name)).ToList();
            }
            catch (IOException)
            {
                throw;
            }
        }

        public async Task<bool> Convert(FileInfo originalFile, FileInfo outputFile, bool overwrite)
        {
            var snippet = await FFmpeg.Conversions.FromSnippet.Convert(originalFile.FullName, outputFile.FullName);
            snippet.SetOverwriteOutput(overwrite);

            try
            {
                await snippet.Start();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error converting {originalFile.Name} - ffmpeg {snippet.Build()}", ex);
                return false;
            }
        }

        // private static async Task Converter()
        // {
        //     var files = GetFiles(inputPath);
        //     Console.WriteLine($"Converting {files.Length} webm's to mp4...");
        //     int i = 0, failureIndex = 0;
        //     foreach (var file in files)
        //     {
        //         try
        //         {
        //             i++;
        //             var fileName = Path.GetFileName(file);
        //             var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
        //             var outputFile = Path.Combine(GetOutputPath(file), fileNameWithoutExt + ".mp4"); ;
        //             Console.SetCursorPosition(0, 2);
        //             Console.Write($"Converting ({i}/{files.Length})... {fileName} to {outputFile}");
        //             var snippet = FFmpeg.Conversions.FromSnippet.Convert(file, outputFile);
        //             var awaiter = snippet.GetAwaiter();
        //             var c = awaiter.GetResult();
        //             await c.Start();

        //             if (!disableRemove)
        //             {
        //                 Console.SetCursorPosition(0, 2);
        //                 Console.Write($"Deleting ({i}/{files.Length})... {fileName}");
        //                 File.Delete(file);
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             failureIndex++;
        //             Console.SetCursorPosition(0, 12);
        //             Console.WriteLine($"Something failed during conversion.. skipping ({failureIndex} failures) {ex.Message}");
        //         }
        //     }
        // }

        // private static string GetOutputPath(string fileName)
        // {
        //     var currPath = Path.GetDirectoryName(fileName);
        //     if (outputPath != currPath)
        //         return currPath;

        //     return outputPath;
        // }    }
    }
}

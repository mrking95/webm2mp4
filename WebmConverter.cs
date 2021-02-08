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
            snippet.AddParameter(@"-vf scale=1280:-2");

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
    }
}

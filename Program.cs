using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Test
{
    class Program
    {
        private static string inputPath;
        private static string outputPath;
        private static Boolean disableRemove = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                AskQuestions();
            }
            else
            {
                foreach (var arg in args)
                {
                    Console.WriteLine(arg);
                    switch (arg.Split(' ')[0])
                    {
                        case "--input":
                            inputPath = arg.Split(' ')[1];
                            break;

                        case "--output":
                            outputPath = arg.Split(' ')[1];
                            break;

                        case "--disableRemove":
                            disableRemove = true;
                            break;
                    }

                    if (string.IsNullOrEmpty(outputPath))
                    {
                        outputPath = inputPath;
                    }

                }
            }

            Console.Clear();
            Converter().GetAwaiter().GetResult();

            
            Console.SetCursorPosition(0, 10);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static async Task Converter()
        {
            var files = GetFiles(inputPath);
            Console.WriteLine($"Converting {files.Length} webm's to mp4...");
            int i = 0, failureIndex = 0;
            foreach (var file in files)
            {
                try
                {
                    i++;
                    var fileName = Path.GetFileName(file);
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                    var outputFile = Path.Combine(GetOutputPath(file), fileNameWithoutExt + ".mp4"); ;
                    Console.SetCursorPosition(0, 2);
                    Console.Write($"Converting ({i}/{files.Length})... {fileName} to {outputFile}");
                    var snippet = FFmpeg.Conversions.FromSnippet.Convert(file, outputFile);
                    var awaiter = snippet.GetAwaiter();
                    var c = awaiter.GetResult();
                    await c.Start();

                    if (!disableRemove)
                    {
                        Console.SetCursorPosition(0, 2);
                        Console.Write($"Deleting ({i}/{files.Length})... {fileName}");
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    failureIndex++;
                    Console.SetCursorPosition(0, 12);
                    Console.WriteLine($"Something failed during conversion.. skipping ({failureIndex} failures) {ex.Message}");
                }
            }
        }

        private static string GetOutputPath(string fileName) {
            var currPath = Path.GetDirectoryName(fileName);
            if(outputPath != currPath)
                return currPath;

            return outputPath;
        } 

        static void AskQuestions()
        {
            Console.WriteLine("I haven't received any input parameters.");
            Console.WriteLine("\n");
            Console.WriteLine("Input path:");
            inputPath = Console.ReadLine();

            Console.WriteLine("Output path: (or click enter to use same as inputPath)");
            var t = Console.ReadLine();
            if (!string.IsNullOrEmpty(t))
                outputPath = t;
        }

        static string[] GetFiles(string path)
        {
            var p = path.Replace("\\", Path.DirectorySeparatorChar.ToString());
            return Directory.GetFiles(p, "*.webm", SearchOption.AllDirectories);
        }
    }

}

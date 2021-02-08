# webm2mp4
A .net core based wrapper for ffmpeg. Made for converting webm's to mp4's whilst retaining quality and sound. Support multi level directory scanning for .webm's.

Feel free to use or fork - if you have any problems using this piece of code, create an issue (Or PR with the fix).

# How to use
Grab the latest release or compile the source (`dotnet restore && dotnet run -- args`).

```
webm2mp4:
  Converts webm file or directory containing multiple webm's to mp4

Usage:
  webm2mp4 [options]

Options:
  -i, --input <input>      The path to the .webm or directorty containing multiple webm's for conversion
  -o, --output <output>    The path for the files to be outputted
  -f, --force              Overwrite if the target file exists [default: True]
  -c, --clean              Remove the original webm when conversion is complete [default: False]
  -l, --log <log>          If specified, writes output to file
  --version                Show version information
  -?, -h, --help           Show help and usage information
```
# webm2mp4
A .net core based wrapper for ffmpeg. Made for converting webm's to mp4's whilst retaining quality and sound. Support multi level directory scanning for .webm's.

This tool can be used to convert webm's to mp4's. It works offline and is commandline based. Simply pass the arguments explained below, let it convert the webm's to mp4's and voila, done.
the ffmpeg wrapper supports many more conversions. If you want support for other file formats, feel free to submit an issue and I'll have a look.

Feel free to use or fork - if you have any problems using this piece of code, create an issue (Or PR with the fix).

# Examples
Converts all the files in Z:\example\path, force overwrites existing files and cleans the original webm after
`webm2mp4 --input Z:\\example\\path\\ --force --clean`

Converts all the files in Z:\example\path, force overwrites existing files
`webm2mp4 --input Z:\\example\\path\\ --force`

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
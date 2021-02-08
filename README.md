# webm2mp4
A .net core based wrapper for ffmpeg. Made for converting webm's to mp4's whilst retaining quality and sound. Support multi level directory scanning for .webm's.

# How to
Grab the latest release or compile the source (`dotnet run`).

```
--input /path/to/files
directory contaning webm files

--output /path/to/output
if left empty, will output where the input file is. Or another output path.

--disableDelete
Optitional, disables deleting of the source webm file
!! Will delete webm by default
```
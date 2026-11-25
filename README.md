# LOPINK CULL

A Windows desktop photo selection tool (C# WinForms / .NET 8).

### Features
- Open folders of JPG/JPEG, PNG, BMP, TIFF or GIF images
- Preview photos
- Star ratings (1–5), pending and reject labels
- Keyboard navigation (left/right) and rating shortcuts (1–5); Delete marks reject
- Copy rated photos to a chosen export folder
- Dark UI

### Build
Install the .NET 8 SDK, then run `publish_windows.bat`. The published single-file EXE is under `bin/Release/net8.0-windows/win-x64/publish`.

GitHub Actions also builds the Windows app on push and manual runs. Download the artifact from the Actions run.

This is a recreated starter implementation uploaded as source; it has not yet been compiled or tested on a Windows machine.
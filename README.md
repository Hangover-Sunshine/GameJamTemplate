# GameJamTemplate
Hangover Sunshine game jam template to provide baseline setup information agnostic to any potential idea. This template was made for Godot 4.2+, backporting to Godot 3.5 may require some touch-ups and rewriting of systems as it has not been tested there.

## Exporting to Windows
If you run into a problem with Windows Security (or other) anti-virus software flagging the build as a problem, it's likely due to the embded PCK. Uncheck that, it just becomes more annoying for the end user in the event the game refuses to launch.

## Exporting to Mac
TODO: make the build chain for it

## For Web
The provided template provides a Web build version; however, Godot 4.2 doesn't support C# exports to web. Use the non-Mono-supportive version of the engine to build the game.

## Non-Hangover Sunshine Users
Go ahead and feel free to use this template for whatever you want - jam, actual game, etc. If you wish to give credit, credit the template as this:

- GDScript: UTILIZES THE GDSCRIPT TEMPLATE FROM Hangover Sunshine
- C#: UTILIZIES THE C# TEMPLATE FROM Hangover Sunshine

But it is not required. Distribution of the license, as it is MIT, is also not required. If your jam requires all non-custom made plugins and code to be linked, please link it to the right branch you have decided to use.

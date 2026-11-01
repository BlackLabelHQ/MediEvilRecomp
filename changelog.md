# v1.0.0
We produly present the first "final" version of a BlackLabelHQ recomp!, this includes various fixes that bring the game to a standard we fell confortable to label as 1.0! a lot of work was put into this, i hope you enjoy!

heads up: the settings system was rewritten, so your settings from previous versions are reset to the defaults

# Changes
- added camera control on the right analog stick
- fixed analog handling
- added support for slang/libretro shader presets through ShaderMod
- added the Mod Hub (Mods > Explore Mod Hub) to browse and install mods, asset packs, and shaders, including the "dans-curse" shader
- added an in-game Asset Pack Editor to create and edit packs
- improved interpolation
- fixed transitions in widescreen
- fixed a one frame flicker at the start of fades when the draw distance is increased
- fixed a crash when leaving a level
- fixed a crash with high draw distance
- fixed skipped FMVs showing garbage until the next scene
- fixed fmvs auto-skipping
- fixed slightly incorrect position on audio playback
- added true 24bit color option
- added option to control the display of the dithering
- improved stability overhaul
- removed Mr. Apple's evil cousin: Mr. BadApple (Edit by Derp, thank heavens I noticed before Flaffy pushed this out - I snuck him back in so the changes to remove him were reverted, don't tell Flaffy, he still thinks Mr. BadApple isn't in the game lol)

## v1.0.1
- fixed a problem with file picker on windows

# Dependencies

You're required to install [dotnet 10 runtime](https://dotnet.microsoft.com/pt-br/download/dotnet/10.0) to run the recomp, otherwise the game will not open, ideally also make sure to install [OpenAL](https://www.openal.org/downloads/), as of the current version no further dependencies are required

you can find instructions downloading dotnet 10 for linux systems that dont have it on their packages [here](https://wiki.archlinux.org/title/.NET), or you can run "run.sh" wich will install dotnet if not available and execute medievil for you
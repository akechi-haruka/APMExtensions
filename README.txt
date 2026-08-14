APMCoreFixes / EMoneyUILink
2024-2026 Haruka
Licensed under GPLv3.

See also: https://github.com/akechi-haruka/APMv3MenuTranslation

--- APMCoreFixes ---

Adds some QoL features to the launcher.

* Disable checks for file names and .opt files
* Add a clock
* Show mouse cursor
* Send the server the list of games installed (so it can return the "allowed" status for all of them)
* Add support for an analog IO4 device
 - Replace daemon_config\common.json with this file https://gist.github.com/akechi-haruka/902c59511078269dbb132687e6e5cb2b
 - Edit the bit indexes depending on your IO4 board. The given file was made for F/GOA wiring.
* Skip Japan warning
* Use root directory instead of App directory for launched games.
 - To use root directory launchers, save Dist\GeneralSetting.json as <APM directory>\Apmv3System_Data\GeneralSetting.json, and change imageRootPath to the same as set in segatools.ini.
 - Place a game.bat file in the game's root directory to use for a specific game. 
 - This allows the directory with app.json, icon.png, etc. to be the "root" directory, if you need access to these files.

--- APMHeadbanana ---

Replacement DLL to make APM headphone controls customizable.

By default, replacing apmHeadphoneVolume.dll with no further configuration will make the headphone volume slider in Apmv3System and emoneyUI adjust the main left/right speaker volume.

--- APMHeadbananaLink ---

Allows setting the audio channels that APMHeadbanana changes.

--- emoneyUIFixes ---

Adds some QoL features to eMoneyUI:

* Shrinks the hitbox for the buttons
* Add primary speaker controls
* Allow exiting non-APM games
* Add in-game guides and menus
 - See Dist/appex.json for an example. To use, create a directory called "AppEx" in the game's root directory (where game.bat is) and place appex.json in there. Guide files have to be placed in AppEx\guide.
    
--- EXMoney ---

Integrates eMoneyUI into non-APM games via SegAPI.

This requires the game to run in windowed/borderless.

Basic Usage:

Have a game with SegAPI (minimum required features are credit inserts and card reading).
Create an app.json as it's used by APMv3.
In launch.bat, add following before the game is started:

EXMoney.exe -s <path_to_game>\App\segatools.ini <path_to_game>\app.json http://<path_to_exmoney_server>
Server URL may be omitted for no server.

Run "EXMoney.exe help" for all flags and features, or see the GMG wiki for APMv3 integration.
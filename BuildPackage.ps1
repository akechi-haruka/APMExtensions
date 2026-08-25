param(
    [string]
    $ReleasePath = "Release",

    [string]
    $OutputFile = "Release.zip",

    [switch]
    $Build
)

$ErrorActionPreference = "Stop"

Write-Output Cleaning...
If (Test-Path $ReleasePath)
{
    Remove-Item -Recurse -Path $ReleasePath
}
New-Item -Path $ReleasePath -ItemType Directory

if ($Build.IsPresent)
{
    Write-Output Building...

    msbuild /p:Configuration=Release
    msbuild /p:RuntimeIdentifier=win-x64 /p:SelfContained=false /p:PublishSingleFile=true /p:PublishReadyToRun=false /p:PublishTrimmed=false /p:IncludeNativeLibrariesForSelfExtract=false /p:IncludeAllContentForSelfExtract=false /p:Configuration=Release APMExtensions.sln
}

$LauncherRoot = "$ReleasePath\LauncherDirectory"

Write-Output Gathering files...
New-Item -Path $LauncherRoot -ItemType Directory
New-Item -Path $LauncherRoot\Apmv3System_Data -ItemType Directory
Copy-Item Resources\GeneralSetting.json $LauncherRoot\Apmv3System_Data\GeneralSetting.json

New-Item -Path $LauncherRoot\BepInEx\plugins -ItemType Directory
New-Item -Path $LauncherRoot\BepInEx\plugins\emoneyUIFixes -ItemType Directory
Copy-Item APMCoreFixes\bin\Release\APMCoreFixes.* $LauncherRoot\BepInEx\plugins
Copy-Item APMCoreFixes\bin\Release\Newtonsoft.* $LauncherRoot\BepInEx\plugins
Copy-Item emoneyUIFixes\bin\Release\emoneyUIFixes.* $LauncherRoot\BepInEx\plugins\emoneyUIFixes
Copy-Item emoneyUIFixes\bin\Release\Haruka.* $LauncherRoot\BepInEx\plugins\emoneyUIFixes
Copy-Item emoneyUIFixes\bin\Release\Microsoft.* $LauncherRoot\BepInEx\plugins\emoneyUIFixes
Copy-Item emoneyUIFixes\bin\Release\SEGA835Lib.* $LauncherRoot\BepInEx\plugins\emoneyUIFixes
Copy-Item emoneyUIFixes\bin\Release\System.* $LauncherRoot\BepInEx\plugins\emoneyUIFixes
Copy-Item BananaphoneApmSystem\bin\Release\net481\BananaphoneApmSystem.* $LauncherRoot\BepInEx\plugins
Copy-Item BananaphoneEmoneyUi\bin\Release\net481\BananaphoneEmoneyUi.* $LauncherRoot\BepInEx\plugins
Copy-Item BananaphoneLib\bin\Release\BananaphoneLib.* $LauncherRoot\BepInEx\plugins
Copy-Item Libs\naudio\* $LauncherRoot\BepInEx\plugins
Copy-Item Libs\*.dll $LauncherRoot\BepInEx\plugins\emoneyUIFixes

Copy-Item -Recurse EXMoney\bin\Release\net10.0-windows\win-x64 $LauncherRoot\EXMoney

Copy-Item -Recurse APMMenuTranslation\Text $LauncherRoot\BepInEx\Translation\en\Text

New-Item -Path $LauncherRoot\amfs -ItemType Directory
Copy-Item Resources\ICF1 $LauncherRoot\amfs

Copy-Item -Recurse UtilityGames $ReleasePath

Write-Output Packaging...
Compress-Archive -Force -Path $ReleasePath\* -DestinationPath $OutputFile

Write-Output Finished.
Write-Output (Resolve-Path $OutputFile)
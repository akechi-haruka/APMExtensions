param(
    [string]
    $ReleasePath = "Release",

    [string]
    $OutputFile = "Release.zip",

    [Boolean]
    $Build = 0
)

Write-Output Cleaning...
If (Test-Path $ReleasePath)
{
    Remove-Item -Recurse -Path $ReleasePath
}
New-Item -Path $ReleasePath -ItemType Directory

if ($Build -ne 0)
{
    Write-Output Building...

    dotnet build
}

$LauncherRoot = "$ReleasePath\LauncherDirectory"

Write-Output Gathering files...
New-Item -Path $LauncherRoot -ItemType Directory
New-Item -Path $LauncherRoot\Apmv3System_Data -ItemType Directory
Copy-Item Resources\GeneralSetting.json $LauncherRoot\Apmv3System_Data\GeneralSetting.json

New-Item -Path $LauncherRoot\BepInEx\plugins -ItemType Directory
Copy-Item APMCoreFixes\bin\Release\APMCoreFixes.* $LauncherRoot\BepInEx\plugins
Copy-Item APMCoreFixes\bin\Release\Newtonsoft.* $LauncherRoot\BepInEx\plugins
Copy-Item APMHeadbananaLink\bin\Release\APMHeadbananaLink.* $LauncherRoot\BepInEx\plugins
Copy-Item emoneyUIFixes\bin\Release\emoneyUIFixes.* $LauncherRoot\BepInEx\plugins

New-Item -Path $LauncherRoot\Apmv3System_Data\Plugins\x86_64 -ItemType Directory
New-Item -Path $LauncherRoot\emoneyUI_Data\Plugins\x86_64 -ItemType Directory
Copy-Item x64\Release\APMHeadbanana.dll $LauncherRoot\Apmv3System_Data\Plugins\x86_64\apmHeadphoneVolume.dll
Copy-Item x64\Release\APMHeadbanana.dll $LauncherRoot\emoneyUI_Data\Plugins\x86_64\apmHeadphoneVolume.dll

Copy-Item -Recurse EXMoney\bin\Release\net10.0-windows\win-x64 $LauncherRoot\EXMoney

Copy-Item -Recurse APMMenuTranslation\Text $LauncherRoot\BepInEx\Translation\en\Text

New-Item -Path $LauncherRoot\amfs -ItemType Directory
Copy-Item Resources\ICF1 $LauncherRoot\amfs

Copy-Item -Recurse UtilityGames $ReleasePath

Write-Output Packaging...
Compress-Archive -Force -Path $ReleasePath\* -DestinationPath $OutputFile

Write-Output Finished.
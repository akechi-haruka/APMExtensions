taskkill /F /IM amdaemond.exe
taskkill /F /IM Apmv3System.exe
timeout /T 1
del %TEMP%\SequenceSetting.json
shutdown /s /t 0
pause
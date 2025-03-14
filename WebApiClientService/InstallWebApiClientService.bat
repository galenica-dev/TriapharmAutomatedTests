rmdir /s /q "C:\QaTools\WebApiClientService"
dotnet publish -c Release -r win-x64 --self-contained false -o C:\QaTools\WebApiClientService
xcopy "C:\repo\TriapharmAutomatedTests\WebApiClientService\InstallWebApiClientService.bat" "C:\QaTools\WebApiClientService\" /Y
xcopy "C:\repo\TriapharmAutomatedTests\WebApiClientService\UninstallWebApiClientService.bat" "C:\QaTools\WebApiClientService\" /Y
sc create WebApiClientService binPath="C:\QaTools\WebApiClientService\WebApiClientService.exe" start=auto
sc start WebApiClientService
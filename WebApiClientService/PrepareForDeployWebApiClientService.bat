rmdir /s /q "C:\QaTools\WebApiClientServiceUpdates"
dotnet publish -c Release -r win-x64 --self-contained false -o C:\QaTools\WebApiClientServiceUpdates
xcopy "C:\repo\TriapharmAutomatedTests\WebApiClientService\InstallWebApiClientService.bat" "C:\QaTools\WebApiClientServiceUpdates\" /Y
xcopy "C:\repo\TriapharmAutomatedTests\WebApiClientService\UninstallWebApiClientService.bat" "C:\QaTools\WebApiClientServiceUpdates\" /Y
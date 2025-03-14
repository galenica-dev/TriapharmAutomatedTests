rmdir /s /q "C:\QaTools\WebApiClientServiceUpdater"
dotnet publish -c Release -r win-x64 --self-contained false -o C:\QaTools\WebApiClientServiceUpdater
sc create WebApiClientServiceUpdater binPath="C:\QaTools\WebApiClientServiceUpdater\WebApiClientServiceUpdater.exe" start=auto
sc start WebApiClientServiceUpdater
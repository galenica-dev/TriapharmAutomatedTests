sc stop WebApiClientService
sc delete WebApiClientService
sc create WebApiClientService binPath="C:\QaTools\WebApiClientService\WebApiClientService.exe" start=auto
sc start WebApiClientService
sc query WebApiClientService
# WebApiClientService

## How to deploy

### Build
dotnet publish -c Release -r win-x64 --self-contained false -o C:\QaTools\WebApiClientService

### Register
sc create WebApiClientService binPath="C:\QaTools\WebApiClientService\WebApiClientService.exe" start=auto

### Start service
sc start WebApiClientService

### Check service
sc query WebApiClientService

### Stop and delete
sc stop WebApiClientService
sc delete WebApiClientService


## Consume

### Home 
http://localhost:8080/

### An API
http://localhost:8080/api/Version

## Firewalls - Communication matrix
* https://galenica.atlassian.net/servicedesk/customer/portal/3/JSMST-1540?created=true
	* Port (8080) => VPN_Galenica => swama704vm02.centralinfra.net
	* Port (8080) => VPN_Galenica => swcvi506vm01.centralinfra.net
	* Port (8080) => VPN_Galenica => swsun007vm01.centralinfra.net
	* Port (8080) => VPN_Galenica => swsun008vm01.centralinfra.net
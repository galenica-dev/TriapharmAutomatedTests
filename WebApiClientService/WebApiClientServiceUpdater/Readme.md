# WebApiClientServiceUpdater

This service run independently from WebApiClientService, so it could handle WebApiClientService updates.
How it works?

Pre-requirement
* WebApiClientServiceUpdater is installed in `C:\QaTools\WebApiClientServiceUpdater` and it is watching the folder `C:\QaTools\WebApiClientServiceUpdates` for a file named `updateNow.txt`. As long this file is not present, no update is triggered.
* WebApiClientService is installed in `C:\QaTools\WebApiClientService`

1. When we deploy a new version of WebApiClientService, the binaries are deployed in `C:\QaTools\WebApiClientServiceUpdates`, without the file `updateNow.txt`
2. When the deployment of the binaries are done, the file `updateNow.txt` is pushed in the same folder as the new binaries.
3. WebApiClientServiceUpdater detects `updateNow.txt`
4. WebApiClientServiceUpdater stops the service WebApiClientService
5. WebApiClientServiceUpdater copy the new binaries in `C:\QaTools\WebApiClientService`
6. WebApiClientServiceUpdater starts the service WebApiClientService
7. Clean the folder `C:\QaTools\WebApiClientServiceUpdates`

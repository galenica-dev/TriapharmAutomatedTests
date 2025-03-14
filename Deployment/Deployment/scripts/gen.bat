@echo off
set JAVA_HOME=C:\Program Files\Microsoft\jdk-17.0.2.8-hotspot
for %%d in (swama704vm02.centralinfra.net swama705vm01.centralinfra.net swama707vm01.centralinfra.net swcvi503vm01.centralinfra.net swcvi504vm01.centralinfra.net swcvi506vm01.centralinfra.net swsun004vm01.centralinfra.net swsun006vm01.centralinfra.net swsun007vm01.centralinfra.net swsun008vm01.centralinfra.net swama888vm01.centralinfra.net swcvi888vm01.centralinfra.net swsun888vm01.centralinfra.net) do (
    echo Generating report for %%d
    C:\QaTools\allure-2.32.0\allure-2.32.0\bin\allure.bat generate %%d --clean -o allure-report/%%d
    IF %ERRORLEVEL% NEQ 0 (
        echo "Error generating report for %%d"
    )
)


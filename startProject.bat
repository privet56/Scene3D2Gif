set "PATH=%~dp0..\devenv\git;%~dp0..\devenv\git\bin;%PATH%"

set APPDATA=%~dp0../devenv/MsVSCode
set USERPROFILE=%~dp0../devenv/MsVSCode

start "" %~dp0../devenv/MsVSCode/Code.exe %~dp0

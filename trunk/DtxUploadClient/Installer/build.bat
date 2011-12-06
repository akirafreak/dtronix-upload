@ECHO OFF

IF "%ProgramFiles(x86)%"=="" (
	"%ProgramFiles%\NSIS\makensis.exe" "%CD%\Installer\gen_installer.nsi"
) ELSE (
	"%ProgramFiles(x86)%\NSIS\makensis.exe" "%CD%\Installer\gen_installer.nsi"
)

EXIT 0
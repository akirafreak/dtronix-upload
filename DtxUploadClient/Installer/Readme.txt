* 07-27-11 #############################################
This setup generator requires Nullsoft Scriptable Install System.
http://nsis.sourceforge.net

This setup has been tested only with NSIS version 2.46.  It may function with later versions, but it has not been tested due to this being the latest release at this time.
http://sourceforge.net/projects/nsis/files/NSIS%202/2.46/

!!! REQUIRED PLUGINS.  Can be found on the project website.  You must have all these plugins insalled into the NSIS/Plugins directory for the installer to compile.
http://code.google.com/p/dtronix-upload/downloads/detail?name=Plugins.zip


To manually generate the setup file, run the build.bat. (Currently not working!  Follow the instructions below.)
The project must have already been built in the parent directory of the Installer folder for the relative paths to function correctly in the script.

Alternatively, you can right click on the gen_installer.nsi file and click "Compile NSIS Script" if you have enabled the context menu support in NSIS.
# Introduction #

This will wall you through how to setup the systems that are required by the setup generator and how to generate the setup file for Dtronix Upload Client.


# Details #

  1. Download the NSIS program. http://dtronix-upload.googlecode.com/files/nsis-2.46-setup.exe
  1. Install the NSIS program in the **default location**.  This is important because the bat file that will be used later searches for the makensis.exe file in its default location.
  1. Open up Visual Studio 2010 and switch the mode to _Release_
  1. Right click on the dtxUpload project and build the client.  This will automatically build the project and run the installer generator.
To find the setup file, browse to the bin/release/Installer folder and look for the setup.exe.

If you just want to manually build the setup.exe, you can browse to your bin/release/Installer folder, and run the build.bat.  Just make sure that the project is already built so as to not generate an outdated setup file.
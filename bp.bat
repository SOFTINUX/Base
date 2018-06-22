::Copyright © 2017 SOFTINUX. All rights reserved.
::Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

@echo off

:: set .NET output folder name
set netVersion="netcoreapp2.1"

cd %~dp0
IF "%1" == "/?" GOTO Help
IF "%1" == "-h" GOTO Help
IF "%1" == "clean" GOTO Clean
IF "%1" == "build" GOTO Build
IF "%1" == "copyexts" GOTO CopyExts
IF "%1" == "copydeps" GOTO CopyDeps
IF "%1" == "publish" GOTO Publish
IF "%1" == "bundles" GOTO Bundles
IF "%1" == "cleanbin" GOTO CleanBuildFolder

GOTO Clean

:Clean
echo ###################
echo CLEAN SOLUTION
echo ###################
dotnet clean
IF "%1" == "clean" GOTO End

:Bundles
echo ###################
echo Updating bundles
echo ###################
for /f "tokens=*" %%i in (bundles.txt) DO (
    pushd ".\%%i"
    dotnet bundle
    popd
)
IF "%1" == "bundles" GOTO End

:Build
echo ###################
echo BUILD SOLUTION
echo ###################
dotnet build /property:GenerateFullPaths=true
IF "%1" == "build" GOTO End

:CopyDeps
::source https://stackoverflow.com/a/6258198
echo ###################
echo Copy Dependencies
echo ###################
set dst_folder=.\WebApplication\bin\Debug\%netVersion%\
if not exist "%dst_folder%" GOTO End
for /f "tokens=*" %%i in (dependencies.txt) DO (
    echo F| xcopy "%%i" /B /F /Y "%dst_folder%" /K
)
IF "%1" == "copydeps" GOTO End

:CopyExts
echo ###################
echo Copy extensions
echo ###################
set dst_folder=.\WebApplication\Extensions\
if not exist "%dst_folder%" mkdir "%dst_folder%"
for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" /B /F /Y  "%dst_folder%" /K
)
GOTO End

:Publish
echo ###################
echo Publish Application
echo ###################
Goto End
dotnet publish %~dp0\Published
set dst_folder=.\WebApplication\bin\Debug\%netVersion%\publish
mkdir "%dst_folder%\Extensions"
if not exist "%dst_folder%" mkdir "%dst_folder%"
for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" "%dst_folder%" /E /Y /I
)

for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" "%dst_folder%\Extensions" /E /Y /I
)
xcopy .\WebApplication\basedb.sqlite %dst_folder% /R

xcopy .\wwwroot %dst_folder%\wwwroot /E /Y /I
Goto End

:CleanBuildFolder
echo #########################
echo Clean obj and bin folders
echo #########################
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
Goto End

:Help
echo Available parameter is :
echo     - clean : clean solution
echo     - build : only build solution
echo     - copydeps : only copy dependencies (defined in dependencies.txt)
echo     - copyexts : only copy extensions (defined in extensions.txt)
echo     - bundles : only update bundles (projects defined in bundles.txt)
echo     - cleanbin : remove bin & obj folders recursively
echo     - publish : not yet implemented
echo.
echo with no parameter or unsupported parameter, build proccess is:
echo     - clean solution (not cleanbin)
echo     - update bundles
echo     - build solution
echo     - copy dependencies
echo     - copy extensions

GOTO End

:End
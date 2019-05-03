::Copyright © 2017-2019 SOFTINUX. All rights reserved.
::Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

@echo off

:: set .NET output folder name (use .NET Core version defined into csproj files)
set netVersion="netcoreapp2.2"
:: Extensions folder
set ext_folder=".\src\WebApplication\Extensions\"
:: Dependencies folder
set dep_folder=".\src\WebApplication\bin\Debug\%netVersion%\"
:: Publish folder
set pub_folder=".\src\WebApplication\bin\Debug\%netVersion%\publish"

cd %~dp0
IF "%1" == "/?" GOTO Help
IF "%1" == "-h" GOTO Help
IF "%1" == "help" GOTO Help
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
IF "%1" == "" GOTO Build
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
if not exist "%dep_folder%" GOTO End
for /f "tokens=*" %%i in (dependencies.txt) DO (
    echo F| xcopy "%%i" /B /F /Y "%dep_folder%" /K
)
IF "%1" == "copydeps" GOTO End

:CopyExts
echo ###################
echo Copy extensions
echo ###################
if not exist "%ext_folder%" mkdir "%ext_folder%"
for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" /B /F /Y  "%ext_folder%" /K
)
GOTO End

:Publish
echo ###################
echo Publish Application
echo ###################
Goto End
dotnet publish %~dp0\Published
mkdir "%pub_folder%\Extensions"
if not exist "%pub_folder%" mkdir "%pub_folder%"
for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" "%pub_folder%" /E /Y /I
)

for /f "tokens=*" %%i in (extensions.txt) DO (
    xcopy "%%i" "%pub_folder%\Extensions" /E /Y /I
)
xcopy .\WebApplication\basedb.sqlite %pub_folder% /R

if not exist "%pub_folder%\wwwroot" mkdir "%pub_folder%\wwwroot"
xcopy .\wwwroot %pub_folder%\wwwroot /E /Y /I
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
echo     - cleanbin : remove bin ^& obj folders recursively
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

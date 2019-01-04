# Copyright © 2017 SOFTINUX. All rights reserved.
# Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

<#
.SYNOPSIS
    This is a Powershell script to create a build
.DESCRIPTION
    Usage:
.PARAMETER parameters
    Specifies optional paramerters.
    clean : clean solution
    build : only build solution
    copydeps : only copy dependencies (defined in dependencies.txt)
    copyexts : only copy extensions (defined in extensions.txt)
    bundles : only update bundles (projects defined in bundles.txt)
    cleanbin : remove bin ^& obj folders recursively
    publish : not yet implemented
.EXAMPLE
    .\bp.ps1 clean
.EXAMPLE
    .\bp.ps1 copyexts
#>

param(
    [string]$parameters
)

# set .NET output folder name (use .NET Core version defined into csproj files)
$netVersion="netcoreapp2.2"
# Extensions folder
$ext_folder=".\src\WebApplication\Extensions\"
# Dependencies folder
$dep_folder=".\src\WebApplication\bin\Debug\$netVersion\"
# Publish folder
$pub_folder=".\src\WebApplication\bin\Debug\$netVersion\publish"
# List extensions
$extensionsListFiles=Get-Content -Path extensions.txt -Raw
# Dependencies list
$dependenciesListFiles=Get-Content -Path dependencies.txt -Raw
# Bundle directory list
$bundle_Dirs=Get-Content -Path bundles.txt -Raw

$echo_title="###################"
$ESC = [char]27

Filter timestamp
{
    if (![string]::IsNullOrEmpty($_) -and ![string]::IsNullOrWhiteSpace($_))
    {
        Write-Host -NoNewline -ForegroundColor Magenta "[$(((get-date).ToUniversalTime()).ToString("yyyy-MM-dd HH:mm:ss.ffffffZ"))]: "
    }

    $_
}

# colorize prompt: https://superuser.com/questions/1259900/how-to-colorize-the-powershell-prompt
Write-Output "Running build core ps1 with parameters: $ESC[33m$parameters$ESC[!p" | timestamp

# Print current PowerShell environment version
Write-Output "Current PowerShell environment version: $($PSVersionTable.PSVersion.ToString())" | timestamp

# Check PowerShell version, exit when below 4.0
if ($PSVersionTable.PSVersion.Major -lt 4)
{
    Write-Error "PowerShell version should be equal with or higher than 4.0, current PowerShell version is $PSVersionTable.PSVersion.Major"
}

Function ConsoleErrorAndExit([string]$message, [int]$exitCode)
{
    Write-Host -ForegroundColor Red $message "Exiting with error code: " $exitCode
    # return $exitCode
}

Function ConsoleSafeAndExit([string]$message, [int]$exitCode)
{
    Write-Host -ForegroundColor Green $message "Exiting with code: " $exitCode
    #return $exitCode
}

Function GetCurrentLine
{
    return $Myinvocation.ScriptlineNumber
}

Function CreateFolderIfNotExists([string]$folder)
{
    if(!(Test-Path "$folder"))
    {
        New-Item "$folder" -ItemType Directory
    }
}

Function EchoMessage([string]$msg_)
{
    Write-Output $echo_title | timestamp
    Write-Output $msg_ | timestamp
    Write-Output $echo_title | timestamp
}

Function CopyFiles([string]$dest_, [string]$fileList_)
{
    ForEach ($itemToCopy in $($fileList_ -split "`r`n"))
    {
        if (!(Test-Path $dest_ -PathType Container)) {
            #Create destination folder
            New-Item -Path $dest_ -ItemType Directory -Force
        }

        $filepath = $Null
        $filepath = get-childitem -literalpath $itemToCopy -ErrorAction SilentlyContinue

        if ($filepath){
            Copy-Item -Path $itemToCopy -Destination $dest_ -Force
            Write-Host $filepath -Destination $dest_ -Force
        }
    }
}

# main
Function Help
{
    Write-Output "Available parameters is :"
    Write-Output "    - clean : clean solution"
    Write-Output "    - build : only build solution"
    Write-Output "    - copydeps : only copy dependencies (defined in dependencies.txt)"
    Write-Output "    - copyexts : only copy extensions (defined in extensions.txt)"
    Write-Output "    - bundles : only update bundles (projects defined in bundles.txt)"
    Write-Output "    - cleanbin : remove bin & obj folders recursively"
    Write-Output "    - publish : not yet implemented"
    Write-Output ""
    Write-Output "with no parameter or unsupported parameter, build proccess is:"
    Write-Output "    - clean solution (not cleanbin)"
    Write-Output "    - update bundles"
    Write-Output "    - build solution"
    Write-Output "    - copy dependencies"
    Write-Output "    - copy extensions"
}

Function Clean
{
    EchoMessage("Start Cleaning")
    dotnet clean
    EchoMessage("End Cleaning")
}

Function CreateBundles
{
    EchoMessage("Start Bundles")
    ForEach ($itemToCopy in $($bundle_Dirs -split "`r`n"))
    {
        Push-Location $itemToCopy
        Write-Output @("Goto: " + (Get-Location).tostring()) | timestamp
        dotnet bundle
        Pop-Location
    }
    EchoMessage("End Bundles")
}

Function Build
{
    EchoMessage("Start Build")
    dotnet build /property:GenerateFullPaths=true
    EchoMessage("End Build")
}

Function Copyexts()
{
    EchoMessage("Start Copy Extenions")
    CopyFiles($ext_folder) ($extensionsListFiles)
    EchoMessage("End Copy Extenions")
}

Function Copydeps()
{
    EchoMessage("Start Copy Dependencies")
    CopyFiles($dep_folder) ($dependenciesListFiles)
    EchoMessage("End Copy Dependencies")
}

Function CleanBin
{
    EchoMessage("Start Clean bin & obj folders")
    ForEach ($itemToRemove in $($(Get-ChildItem .\ -include bin,obj -Recurse) -split"`r`n"))
    {
        remove-item $itemToRemove -Force -Recurse
        Write-Host "$itemToRemove $ESC[31mDELETED"
    }
    EchoMessage("End Clean bin & obj folders")
}

Function NoParam
{
    Clean
    CreateBundles
    Build
    Copydeps
    Copyexts
}

switch ($parameters.ToUpper()) {
    '/?' { Help; break }
    '-h' { Help; break }
    'HELP' { Help; ConsoleSafeAndExit("Finished.")(0); break }
    'CLEAN' { Clean; ConsoleSafeAndExit("Finished.")(0); break }
    'BUILD' { Build; ConsoleSafeAndExit("Finished.")(0); break }
    'COPYDEPS' { Copydeps; ConsoleSafeAndExit("Finished.")(0); break }
    'COPYEXTS' { Copyexts; ConsoleSafeAndExit("Finished.")(0); break }
    'BUNDLES' { CreateBundles; ConsoleSafeAndExit("Finished.")(0); break }
    'CLEANBIN' { CleanBin; ConsoleSafeAndExit("Finished.")(0); break }
    #'PUBLISH' { Clean; break }
    Default { NoParam; break }
}

exit

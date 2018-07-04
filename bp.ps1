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
$netVersion="netcoreapp2.1"
# Extensions folder
$ext_folder=".\WebApplication\Extensions\"
# Dependencies folder
$dep_folder=".\WebApplication\bin\Debug\$netVersion\"
# Publish folder
$pub_folder=".\WebApplication\bin\Debug\$netVersion\publish"
# List extensions
$extensionsListFiles=Get-Content -Path extensions.txt -Raw
# Dependencies list
$dependenciesListFiles=Get-Content -Path dependencies.txt -Raw

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
echo "Running build core ps1 with parameters: $ESC[33m$parameters$ESC[!p" | timestamp

# Print current PowerShell environment version
echo "Current PowerShell environment version: $($PSVersionTable.PSVersion.ToString())" | timestamp

# Check PowerShell version, exit when below 4.0
if ($PSVersionTable.PSVersion.Major -lt 4)
{
    Write-Error "PowerShell version should be equal with or higher than 4.0, current PowerShell version is $PSVersionTable.PSVersion.Major"
}

Function ConsoleErrorAndExit([string]$message, [int]$exitCode)
{
    Write-Host -ForegroundColor Red $message "with error code: " $exitCode
    # return $exitCode
}

Function ConsoleSafeAndExit([string]$message, [int]$exitCode)
{
    Write-Host -ForegroundColor Green $message "with code: " $exitCode
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
    echo $echo_title | timestamp
    echo $msg_ | timestamp
    echo $echo_title | timestamp
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
        }
    }
}

# main
Function Help
{
    echo "Available parameter is :"
    echo "    - clean : clean solution"
    echo "    - build : only build solution"
    echo "    - copydeps : only copy dependencies (defined in dependencies.txt)"
    echo "    - copyexts : only copy extensions (defined in extensions.txt)"
    echo "    - bundles : only update bundles (projects defined in bundles.txt)"
    echo "    - cleanbin : remove bin & obj folders recursively"
    echo "    - publish : not yet implemented"
    echo ""
    echo "with no parameter or unsupported parameter, build proccess is:"
    echo "    - clean solution (not cleanbin)"
    echo "    - update bundles"
    echo "    - build solution"
    echo "    - copy dependencies"
    echo "    - copy extensions"
}

Function Clean
{
    EchoMessage("Start Cleaning")
    dotnet clean
    EchoMessage("End Cleaning")

    ConsoleSafeAndExit("Script end") (0)
}

Function Build
{
    EchoMessage("Start Build")
    dotnet build /property:GenerateFullPaths=true
    EchoMessage("End Build")

    ConsoleSafeAndExit("Script end") (0)
}

Function CreateBundles
{
    Write-Host "Not yet implemented."
}

Function Copyexts()
{
    CopyFiles($ext_folder) ($extensionsListFiles)
}

Function Copydeps()
{
    CopyFiles($dep_folder) ($dependenciesListFiles)
}

Function NoParam
{
    Help
    ConsoleSafeAndExit("Script end") (0)
}

switch ($parameters.ToUpper()) {
    '/?' { Help; break }
    '-h' { Help; break }
    'HELP' { Help; break }
    'CLEAN' { Clean; break }
    'BUILD' { Build; break }
    'COPYDEPS' { Copydeps; break }
    'COPYEXTS' { Copyexts; break }
    'BUNDLES' { CreateBundles; break }
    'CLEANBIN' { Clean; break }
    #'PUBLISH' { Clean; break }
    Default { NoParam; break }
}

exit

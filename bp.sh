#!/bin/bash

# Copyright © 2017-2019 SOFTINUX. All rights reserved.
# Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

#These two lines is to set correct directory position if you build from JetBrain Rider
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
#.NET Core version (defined into csproj)
NETVERSION="net7.0"
#Extension destination folder
EXT_FOLDER="./src/WebApplication/Extensions"
#Dependencies destination folder
DEP_FOLDER="./src/WebApplication/bin/Debug/$NETVERSION/"
#Publish folder
PUB_FOLDER=".\src\WebApplication\bin\Debug\%netVersion%\publish"

cd "$DIR"

function clean ()
{
    echo "###################"
    echo "CLEAN SOLUTION"
    echo "###################"
    dotnet clean
}

function bundles ()
{
    echo "###################"
    echo "Updating bundles"
    echo "###################"
    gulp
}

function build ()
{
    echo "###################"
    echo "BUILD SOLUTION"
    echo "###################"
    dotnet build /property:GenerateFullPaths=true
}

function generateBareboneCss ()
{
    echo "###################"
    echo "generateBareboneCss"
    echo "###################"
    node ./node_modules/sass/sass.js --no-source-map --no-charset ./src/SoftinuxBase.Barebone/Styles/scss/index.scss ./src/SoftinuxBase.Barebone/Styles/barebone.css
}

function generateSecurityCss ()
{
    echo "###################"
    echo "generateSecurityCss"
    echo "###################"
    node ./node_modules/sass/sass.js --no-source-map --no-charset ./src/SoftinuxBase.Security/Styles/scss/index.scss ./src/SoftinuxBase.Security/Styles/Security.css
}

function copyexts ()
{
    echo "###################"
    echo "Copy extensions"
    echo "###################"
    if [ ! -d "$EXT_FOLDER" ]; then
        mkdir -p $EXT_FOLDER
    fi
    cat extensions.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $EXT_FOLDER; echo cp % $EXT_FOLDER"
}

function copydeps ()
{
    echo "###################"
    echo "Copy Dependencies"
    echo "###################"
    if [ ! -d "$DEP_FOLDER" ]; then
        echo "The dependencies destination folder does not exist."
        exit 1
    fi
    cat dependencies.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $DEP_FOLDER; echo cp % $DEP_FOLDER"
}

function cleanbuildfolders ()
{
    echo "#########################"
    echo "Clean obj and bin folders"
    echo "#########################"
    find . -iname "bin" -o -iname "obj" | xargs rm -rf
}

function help
{
    echo "Available parameter is :"
    echo "    - clean : clean solution"
    echo "    - build : only build solution"
    echo "    - copydeps : only copy dependencies (defined in dependencies.txt)"
    echo "    - copyexts : only copy extensions (defined in extensions.txt)"
    echo "    - bundles : only update bundles"
    echo -ne "\n"
    echo "with no parameter or unsupported parameters, build proccess is:"
    echo "    - clean solution (not cleanbin)"
    echo "    - update bundles"
    echo "    - build solution"
    echo "    - copy dependencies"
    echo "    - copy extensions"
}


case $1 in
    "clean" )
        clean ;;
    "build" )
        build ;;
    "copyexts" )
        copyexts ;;
    "copydeps" )
        copydeps ;;
    "bundles" )
        bundles ;;
    "cleanbin" )
        cleanbuildfolders ;;
    "-h|--help" )
        help ;;
    "--help" )
        help ;;
    "generateBareboneCss")
        generateBareboneCss ;;
    "generateSecurityCss")
        generateSecurityCss ;;
    *)
        #clean
        echo "######################################## EXIT CLEAN ###################################################"
        #generateBareboneCss
        echo "######################################## EXIT CSS1 ###################################################"
        #generateSecurityCss
        echo "######################################## EXIT CSS2 ###################################################"
        #build
        echo "######################################## EXIT BUILD ###################################################"
        copydeps
        copyexts
        ;;
esac
#Clean
echo "All done"

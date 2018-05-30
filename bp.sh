#!/bin/bash

# Copyright © 2017 SOFTINUX. All rights reserved.
# Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

#These two lines is to set correct directory position if you build from JetBrain Rider
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$DIR"

function clean
{
    echo "###################"
    echo "CLEAN SOLUTION"
    echo "###################"
    dotnet clean
}

function build
{
    echo "###################"
    echo "BUILD SOLUTION"
    echo "###################"
    dotnet build /property:GenerateFullPaths=true
}

function copyexts
{
    echo "###################"
    echo "Copy extensions"
    echo "###################"
    SET_DEST="./WebApplication/Extensions"
    if [ ! -d "$SET_DEST" ]; then
        mkdir -p $SET_DEST
    fi
    cat extensions.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $SET_DEST; echo cp % $SET_DEST"
}

function copydeps
{
    echo "###################"
    echo "Copy Dependencies"
    echo "###################"
    SET_DEST="./WebApplication/bin/Debug/netcoreapp2.0/"
    if [ ! -d "$SET_DEST" ]; then
        echo "The dependencies destination folder does not exist."
        exit 1
    fi
    cat dependencies.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $SET_DEST; echo cp % $SET_DEST"
}

function bundles
{
    echo "###################"
    echo "Updating bundles"
    echo "###################"
    cat bundles.txt | sed 's/\\/\//g' | xargs -I % bash -c "cd %; dotnet bundle; cd $OLDPWD"
}

function help
{
    echo "Available parameter is :"
    echo "    - clean : clean solution"
    echo "    - build : only build solution"
    echo "    - copydeps : only copy dependencies (defined in dependencies.txt)"
    echo "    - copyexts : only copy extensions (defined in extensions.txt)"
    echo "    - bundles : only update bundles (projects defined in bundles.txt)"
    echo -ne "\n"
    echo "with no parameter or unsupported parameters, build proccess is:"
    echo "    - clean solution"
    echo "    - update bundles"
    echo "    - build solution"
    echo "    - copy dependencies"
    echo "    - copy extensions"
}


case $1 in
    clean)
        clean
        ;;
    build)
        build
        ;;
    copyexts)
        copyexts
        ;;
    copydeps)
        copydeps
        ;;
    bundles)
        bundles
        ;;
    -h|--help)
        help
        ;;
    *)
        clean
        bundles
        build
        copydeps
        copyexts
        ;;
esac
#Clean
echo "All done"

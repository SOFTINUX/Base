#!/bin/bash

# Copyright © 2017 SOFTINUX. All rights reserved.
# Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

function clean
{
    echo "###################"
    echo "CLEANING SOLUTION"
    echo "###################"
    dotnet clean
    if [ "$1" = "clean" ]; then exit; fi
}

function build
{
    echo "###################"
    echo "BUILD SOLUTION"
    echo "###################"
    dotnet build
    if [ "$1" = "build" ]; then exit; fi
}

function copyexts
{
    echo "###################"
    echo "Copy extensions"
    echo "###################"
    SET_DEST="./WebApplication/Extensions"
    cat extensions.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $SET_DEST; echo cp % $SET_DEST"
    if [ "$1" = "copyexts" ]; then exit; fi
}

function copydeps
{
    echo "###################"
    echo "Copy Dependencies"
    echo "###################"
    SET_DEST="./WebApplication/bin/Debug/netcoreapp2.0/"
    cat dependencies.txt | sed 's/\\/\//g' | xargs -I % bash -c "cp % $SET_DEST; echo cp % $SET_DEST"
    if [ "$1" = "copydeps" ]; then exit; fi
}

function help
{
    echo "Avaliable parameters is :"
    echo "    - clean : clean solution"
    echo "    - build : only build solution"
    echo "    - copydeps : only copy dependencies (defined in dependecies.txt)"
    echo "    - copyexts : only copy extensions (defined in extentions.txt)"
    echo -ne "\n"
    echo "with no parameters or unsupported parameters, build proccess is:"
    echo "    - cleaning solution"
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
    -h|--help)
        help
        ;;
    *)
        clean
        build
        copydeps
        copyexts
        ;;
esac
#Clean
echo "All done"

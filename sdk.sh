#!/usr/bin/env bash
set -e

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
export HOME=~
export NUGET_PACKAGES=~/.nuget/packages
export NUGET_HTTP_CACHE_PATH=~/.local/share/NuGet/v3-cache

Configuration=Debug

RootDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DotNetSDKPath=$RootDir"/.tools/dotnet/"$DotNetSDKVersion
DotNetExe=$DotNetSDKPath"/dotnet"

usage() {
        echo "Usage: sdk.sh [-i|--install ]"
}

downloadCatalog() {
        local isForce=$1
        local catalog=$RootDir"/.data/catalog.bin"
        local data=$(dirname $catalog)

        if [[ ! -e $data ]]; then
                mkdir $data
        fi

        if [[ $isForce == "true" && -e $catalog ]]; then
                echo "Deleting existing catalog"
                rm $catalog
        fi

        if [[ ! -e $catalog ]]; then
                echo "Downloading catalog.bin..."
                curl --output $catalog "https://portability.blob.core.windows.net/catalog/catalog.bin"
        fi
}

installSDK() {
        local dotnetPath=$(command -v dotnet)

        if [[ $dotnetPath != "" ]]; then
                echo "dotnet is found on PATH. using that."
                DotNetExe=$dotnetPath
                return 0
        fi

        if [[ -e $DotNetExe ]]; then
                echo $DotNetExe" exists.  Skipping install..."
                return 0
        fi

        local DotNetToolsPath=$(dirname $DotNetSDKPath)

        if [ ! -d $DotNetToolsPath ]; then
                mkdir -p $DotNetToolsPath
        fi

        curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current --install-dir $DotNetSDKPath
}

while [[ $# -gt 0 ]]; do
        option="$(echo $1 | awk '{print tolower($0)}')"
        case "$option" in
        "-?" | "--help")
                usage
                exit 1
                ;;
        "-i" | "--install")
                installSDK "true"
                exit 0
                ;;
        "--downloadcatalog")
                downloadCatalog "true"
                exit 0
                ;;
        *)
                echo "Unknown option: "$option
                usage
                exit 1
                ;;
        esac
done


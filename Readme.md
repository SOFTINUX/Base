# Project overview

[![Build status](https://ci.appveyor.com/api/projects/status/mktm6ae3csndb9ma?svg=true)](https://ci.appveyor.com/project/Xarkam/base) [![Build Status](https://softinux.visualstudio.com/Softinux%20Base/_apis/build/status/Softinux%20Base-ASP.NET%20Core-CI?branchName=master)](https://softinux.visualstudio.com/Softinux%20Base/_build/latest?definitionId=1&branchName=master) ![Line of code](https://tokei.rs/b1/github/SOFTINUX/Base) ![Documentation Status](https://readthedocs.org/projects/softinux-base/badge/?version=latest) [![Gitter chat](https://badges.gitter.im/SOFTINUX/Base/repo.png)](https://gitter.im/softinux-base/Lobby) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

*SoftinuxBase* is a free, open source, and cross-platform framework with built-in security access support and management for creating modular and extendable [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) Core web applications.

It is built using [ExtCore](http://extcore.net/) framework and the most modern tools and languages.
Join our team!

:warning: During the pre-alpha development phase, the issues are managed in our [bug tracker](https://issues.osames.org/projects/SOFB/issues) :warning:

## Few Facts About SOFTINUX Base

- It's free and open source.
- It runs on Windows, MacOS and Linux.
- It's completely modular and extendable. Using the features of the underlying ExtCore framework you can easily create your own extensions to extend its functionality.

## Table of content

- [Project overview](#project-overview)
    - [Few Facts About SOFTINUX Base](#few-facts-about-softinux-base)
    - [Table of content](#table-of-content)
    - [Basic Concepts](#basic-concepts)
- [License](#license)
- [Getting started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
        - [1. Restore dependencies](#1-restore-dependencies)
        - [2. Restore nuget packages](#2-restore-nuget-packages)
        - [3. Update database with migration](#3-update-database-with-migration)
        - [4. Build the application](#4-build-the-application)
        - [5. Run the app](#5-run-the-app)
            - [:information_source: Information About Visual Studio 2017 :information_source:](#informationsource-information-about-visual-studio-2017-informationsource)
            - [:information_source: Information About Rider 2017.3 :information_source:](#informationsource-information-about-rider-20173-informationsource)
        - [6. Add the first user (demo user)](#6-add-the-first-user-demo-user)
        - [7. Login with demo user](#7-login-with-demo-user)
    - [Implement your own extension](#implement-your-own-extension)
        - [Add a new project](#add-a-new-project)
        - [Add project reference to the solution](#add-project-reference-to-the-solution)
        - [Write your code](#write-your-code)
    - [Code coverage](#Code-coverage)
- [Browsers Support](browsers-support)
- [Using Visual Studio Code For Developing](#using-visual-studio-code-for-developing)

## Basic Concepts

*SoftinuxBase* is a framework that looks like a .NET Core web application, but is intended to host mini web applications called extensions. Every extension will plug its content (pages, menu items) as well as security and authentication related items (permissions, roles, links...).

*SoftinuxBase* manages the common stuff so that the developer can focus on its extension and business logic, just having to provide what we call metadata to know how to display and authorize access to content, and use our version of Authorize attribute.

Read [documentation](https://softinux-base.readthedocs.io/en/latest/?) to learn more about this.

Some screenshots of features:

<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/login.png" title="Login" width="300" heigth="200">\
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/menu_and_administration.png" title="Administration" width="600" heigth="400">\
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/user_profile.png" title="Administrator editing user profile" width="600" heigth="400">\
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/grant_global_permissions_wip.png" title="Set permissions" width="600" heigth="400">

# License

Licensed under the MIT License.
See LICENSE file for license information.

# Getting started

## Prerequisites

In order you must have installed:

- [.NET Core SDK](https://www.microsoft.com/net/download) version 3.1.
- [Node JS](https://nodejs.org/en/) to get javascript dependencies with npm.

### Linux
To avoid error `System.IO.IOException: The configured user limit (128) on the number of inotify instances has been reached.
   at System.IO.FileSystemWatcher.StartRaisingEvents()` you must execute this command in terminal:

`echo fs.inotify.max_user_instances=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p`

## Installation

#### 1. Restore dependencies

Run `npm i` command so that dependencies packages are installed.

#### 2. Restore nuget packages

Restore the nuGet packages is now an implicit command executed at application build.
But you can still restore packages without building the application with the command `dotnet restore` in solution root folder.

#### 3. Update database with migration

Go to *src/WebApplication* folder and run `dotnet ef database update`.
This will create the database. See *appsettings.json* for database path.
The Entity Framework database context is defined in web application's *Startup.cs*.
We use Sqlite for development, but you can change this easily for another SGDB (see *appsettings.json* file).

#### 4. Build the application

Go to the solution root folder and run `bp.bat` under Windows or `bp.sh` under Linux/MacOS. (use -h for help).
This is the quick way. Some commands from `bp.bat`/`bp.sh` are also used by PreBuild and PostBuild events
but this may not work for all IDEs.

#### 5. Run the app

Go to *src/WebApplication* folder and type `dotnet run`.
If you prefer, you can also execute this command from solution root folder: `dotnet run --project src\WebApplication\WebApplication.csproj`
(Beware of the path if you are on Linux/MacOS).

After that, the application is available on <http://localhost:5000/> or <https://localhost:5001/>

##### :information_source: Information About Visual Studio 2017 :information_source:

If you launched application from Visual Studio, this port will change,
being randomly defined, and value is stored in *src/WebApplication/Properties/launchSettings.json*
You can edit this value in Visual Studio: WebApplication's properties > Debug tab > Web Server Settings/App URL or directly in *launchSettings.json* file.
After, the default port used by *dotnet run* is the port defined in *src/WebApplication/Properties/launchSettings.json*.

##### :information_source: Information About Rider 2017.3 :information_source:

Rider 2017.3 cannot execute the PostBuild event declared in *src/WebApplication.csproj*.
You need to execute `./bp.sh copyexts` and `./bp.sh copydeps` after building the solution or project.
Or refer to our [documentation](https://softinux-base.readthedocs.io/en/latest/howto/configure_rider.html) to see how to configure external tools that will be launched by build process.

#### 6. Add the first user (demo user)

With Postman (or the program of your choice) make a POST request to this url: <http://localhost:5000/dev/seed/create-user>
With command line:

- using curl: `curl -i -X POST http://localhost:5000/dev/seed/create-user -d "Content-Length: 0"`
- using powershell: `Invoke-WebRequest -Uri http://localhost:5000/dev/seed/create-user -Method POST`

This will create the demo user with general permissions.

#### 7. Login with demo user

user: **johndoe@softinux.com**
(or johndoe)
password: **123_Password**
(password is case sensitive)

## Implement your own extension

:warning: You cannot place your Extensions folder to another drive. See [#2981](https://github.com/dotnet/core-setup/issues/2981#issuecomment-322572374)

You can use [Visual Studio 2017](https://www.visualstudio.com/fr/downloads/), [Visual Studio Code](https://code.visualstudio.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/) to make your own extension.
If you decide to use Visual Studio, be aware **that projects are not compatible with Visual Studio 2015**.

### Add a new project

Using command-line (easy and cross-platform):

```bash
dotnet new classlib -o <you_new_project>
```

### Add project reference to the solution

Go to solution folder and type:

```bash
dotnet add reference <path_to_your_new_project>
```

### Write your code

In your new project, create a class that implements `SoftinuxBase.Infrastructure.IExtensionMetadata`.

Your extension will depend on `SoftinuxBase.Infrastructure`.

Have a look at sample extensions, [wiki](https://github.com/SOFTINUX/Base/wiki), feel free to open issues for questions.

# Code coverage

We give simple code covering with [coverlet coverage](https://github.com/coverlet-coverage/coverlet).
But to use it with Visual Studio Code, you need two extensions:

- [Net Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer)
- [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters)

## .NET Test Explorer configuration

If you want code coverage automatically in .NET Test Explorer, you must configure `dotnet-test-explorer.testArguments`
and add `/p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info` in text field.

Configuratrion by Json value :

    "dotnet-test-explorer.testArguments": "/p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info"

We also provide many test watchers script in TestWatchesr folders.

:warning: Don't forget to add "coverlet" package in your tests projets.

# Browsers support

| ![][ie]<br />IE / Edge | ![][firefox]<br />Firefox | ![][chrome]<br />Chrome | ![][safari]<br />Safari | ![][opera]<br />Opera |
| ---------------------- | ------------------------- | ----------------------- | ----------------------- | --------------------- |
| IE11, Edge             | last 10 versions          | last 10 versions        | last 10 versions        | last 10 versions      |

[ie]: https://raw.githubusercontent.com/godban/browsers-support-badges/master/src/images/edge.png
[firefox]: https://raw.githubusercontent.com/godban/browsers-support-badges/master/src/images/firefox.png
[chrome]: https://raw.githubusercontent.com/godban/browsers-support-badges/master/src/images/chrome.png
[safari]: https://raw.githubusercontent.com/godban/browsers-support-badges/master/src/images/safari.png
[opera]: https://raw.githubusercontent.com/godban/browsers-support-badges/master/src/images/opera.png

# Using Visual Studio Code For Developing

If you prefer to use Visual Studio Code, you need these extensions:

Very recommended for this project

- [Net Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer)
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [Debugger for Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome)
- [IntelliSense for CSS class name](https://marketplace.visualstudio.com/items?itemName=Zignd.html-css-class-completion)
- [Path Intellisense](https://marketplace.visualstudio.com/items?itemName=christian-kohler.path-intellisense)
- [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)
- [Visual Studio Code Solution Explorer](https://marketplace.visualstudio.com/items?itemName=fernandoescolar.vscode-solution-explorer)
- [Visual IntelliCode](https://marketplace.visualstudio.com/items?itemName=VisualStudioExptTeam.vscodeintellicode)
- [Bracket Pair Colorizer 2](https://marketplace.visualstudio.com/items?itemName=CoenraadS.bracket-pair-colorizer-2)
- [EditorConfig for VS Code](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig)
- [Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- [Better Comments](https://marketplace.visualstudio.com/items?itemName=aaron-bond.better-comments)
- [MarkdownLint](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint)
- [Version Lens](https://marketplace.visualstudio.com/items?itemName=pflannery.vscode-versionlens)
- [Trailing Spaces](https://marketplace.visualstudio.com/items?itemName=shardulm94.trailing-spaces)
- [Todo](https://marketplace.visualstudio.com/items?itemName=fabiospampinato.vscode-todo-plus)
- [Log File Highlighter](https://marketplace.visualstudio.com/items?itemName=emilast.LogFileHighlighter)

Optional for better experience:

- [Git History](https://marketplace.visualstudio.com/items?itemName=donjayamanne.githistory)
- [Git Lens](https://marketplace.visualstudio.com/items?itemName=eamodio.gitlens)
- [Git Graph](https://marketplace.visualstudio.com/items?itemName=mhutchie.git-graph)
- [gitignore](https://marketplace.visualstudio.com/items?itemName=codezombiech.gitignore)
- [HTML CSS Support](https://marketplace.visualstudio.com/items?itemName=ecmel.vscode-html-css)
- ~~[Output Colorizer](https://marketplace.visualstudio.com/items?itemName=IBM.output-colorizer)~~ not maintained.
- [Task Explorer](https://marketplace.visualstudio.com/items?itemName=spmeesseman.vscode-taskexplorer)
- ~~[Code Outline](https://marketplace.visualstudio.com/items?itemName=patrys.vscode-code-outline)~~ depreciated.Vs Code to this.
- [TSLint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
- [Can I Use](https://marketplace.visualstudio.com/items?itemName=akamud.vscode-caniuse)

At your discretion:

- [To Do Task](https://marketplace.visualstudio.com/items?itemName=sandy081.todotasks)
- [Remind Me](https://marketplace.visualstudio.com/items?itemName=cg-cnu.vscode-remind-me)
- [Python](https://marketplace.visualstudio.com/items?itemName=donjayamanne.python)
- [Quokka.js](https://marketplace.visualstudio.com/items?itemName=WallabyJs.quokka-vscode)

We also provide the `tasks.json` and `launch.json` configuration for Visual studio Code.

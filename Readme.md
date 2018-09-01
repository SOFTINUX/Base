# Project overview
![Build status](https://ci.appveyor.com/api/projects/status/mktm6ae3csndb9ma?svg=true) ![Line of code](https://tokei.rs/b1/github/SOFTINUX/Base) ![Documentation Status](https://readthedocs.org/projects/softinux-base/badge/?version=latest) [![Gitter chat](https://badges.gitter.im/SOFTINUX/Base/repo.png)](https://gitter.im/softinux-base/Lobby) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

*SOFTINUX Base* is a free, open source, and cross-platform framework with built-in security access support and management for creating modular and extendable [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) Core web applications.

It is built using [ExtCore](http://extcore.net/) framework and the most modern tools and languages.  
Join our team!

:warning: During the pre-alpha development phase, the issues are managed in our [bug tracker](https://issues.osames.org/projects/SOFB/issues) :warning:

## Few Facts About SOFTINUX Base

- It's free and open source.
- It runs on Windows, MacOS and Linux.
- It's completely modular and extendable. Using the features of the underlying ExtCore framework you can easily create your own extensions to extend its functionality.

## Table of content
- [Basic Concepts](#basic-concepts)
- [License](#license)
- [Getting started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
        - [1. Restore dependencies](#1-restore-dependencies)
        - [2. Restore nuget packages](#2-restore-nuget-packages)
        - [3. Update database with migration](#3-update-database-with-migration)
        - [4. Build the application](#4-build-the-appplication)
        - [5. Run the app](#5-run-the-app)
            - [Information About Visual Studio 2017](#informationsource-information-about-visual-studio-2017-informationsource)
            - [Information About Rider 2017.3](#informationsource-information-about-rider-20173-informationsource)
        - [6. Add the first user (demo user)](#6-add-the-first-user-demo-user)
        - [7. Login with demo user](#7-login-with-demo-user)
    - [Implement your own extension](#implement-your-own-extension)
        - [Information About Visual Studio 2017](#information_source-information-about-visual-studio-2017-information_source)
        - [Information About Rider 2017.3](#information_source-information-about-rider-20173-information_source)
        - [Add a new project](#add-a-new-project)
        - [Add project reference to the solution](#add-project-reference-to-the-solution)
        - [Write your code](#write-your-code)
- [Using Visual Studio Code For Developing](#using-visual-studio-code-for-developing)

## Basic Concepts

*Softinux Base* is a framework that looks like a .NET Core web application, but is intended to host mini web applications called extensions. Every extension will plug its content (pages, menu items) as well as security and authentication related items (permissions, roles, links...).

*Base* manages the common stuff so that the developer can focus on its extension and business logic, just having to provide what we call metadata to know how to display and authorize access to content, and use our version of Authorize attribute.

Read [documentation](https://softinux-base.readthedocs.io/en/latest/?) to learn more about this.

Some screenshots of features:

<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/login.png" title="Login" width="300" heigth="200"><br />
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/menu_and_administration.png" title="Administration" width="600" heigth="400"><br />
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/user_profile.png" title="Administrator editing user profile" width="600" heigth="400"><br />
<img src="https://raw.githubusercontent.com/SOFTINUX/Base/master/screenshots/grant_global_permissions_wip.png" title="Set permissions" width="600" heigth="400">

# License
Licensed under the MIT License.  
See LICENSE file for license information.

# Getting started

## Prerequisites
In order you must have installed:  
- [.NET Core SDK](https://www.microsoft.com/net/download)
- [Node JS](https://nodejs.org/en/)

## Installation
#### 1. Restore dependencies
Go to *Barebone* folder and run `npm i --save-dev` command so that dependencies packages are installed and settings updated.

#### 2. Restore nuget packages
Restore the nuGet packages is now an implicit command executed at application build.  
But you can still restore packages without building the application with the command `dotnet restore` in solution root folder.

#### 3. Update database with migration
Go to *Webapplication* folder and run `dotnet ef database update`.  
This will create the database. See *appsettings.json* for database path.  
The Entity Framework database context is defined in web application's *Startup.cs*.  
We use Sqlite for development, but you can change this easily for another SGDB (see *appsettings.json* file).

#### 4. Build the application
Go to the solution root folder and run `bp.bat` under Windows or `bp.sh` under Linux/MacOS. (use -h for help).

#### 5. Run the app
Go to *WebApplication* folder and type `dotnet run`.  
If you prefer, you can also execute this command from solution root folder: `dotnet run --project WebApplication\WebApplication.csproj`  
(Beware of the path if you are on Linux/MacOS).

After that, the application is available on <http://localhost:5000/> or <https://localhost:5000/>

##### :information_source: Information About Visual Studio 2017 :information_source:
If you launched application from Visual Studio, this port will change,  
being randomly defined, and value is stored in *WebApplication/Properties/launchSettings.json*  
You can edit this value in Visual Studio: WebApplication's properties > Debug tab > Web Server Settings/App URL or directly in launchSettings file.  
After, the default port used by *dotnet run* is the port defined in *WebApplication/Properties/launchSettings.json*.

##### :information_source: Information About Rider 2017.3 :information_source:
Rider 2017.3 cannot execute the PostBuildEvent declared into WebApplication.csproj  
You need to execute `./bp.sh copyexts` and `./bp.sh copydeps` after building the solution or project.  
Or refer to our [documentation](https://softinux-base.readthedocs.io/en/latest/howto/configure_rider.html) to see how to configure external tools that will be launched by build process.

#### 6. Add the first user (demo user)
With Postman (or the program of your choice) make a POST request to this url: <http://localhost:5000/dev/seed/CreateUser>  
With command line:

- using curl: `curl -i -X POST http://localhost:5000/dev/seed/CreateUser`
- using powershell: `Invoke-WebRequest -Uri http://localhost:5000/dev/seed/CreateUser -Method POST`

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

`dotnet new classlib -o <you_new_project>`

### Add project reference to the solution
Go to solution folder and type:

`dotnet add reference <path_to_your_new_project>`

### Write your code
In your new project, create a class that implements `Infrastructure.IExtensionMetadata`. You may also implement `Infrastructure.IExtensionDatabaseMetadata`.

Your extension will depend on `Infrastructure`.

Have a look at sample extensions, [wiki](https://github.com/SOFTINUX/Base/wiki), feel free to open issues for questions.

# Using Visual Studio Code For Developing
If you prefer to use Visual Studio Code, you need these extensions:

- [Net Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer)
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [Code Outline](https://marketplace.visualstudio.com/items?itemName=patrys.vscode-code-outline)
- [Debugger for Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome)
- [IntelliSense for CSS class name](https://marketplace.visualstudio.com/items?itemName=Zignd.html-css-class-completion)
- [Path Intellisense](https://marketplace.visualstudio.com/items?itemName=christian-kohler.path-intellisense)
- [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)
- [TSLint](https://marketplace.visualstudio.com/items?itemName=eg2.tslint)
- [Visual Studio Code Solution Explorer](https://marketplace.visualstudio.com/items?itemName=fernandoescolar.vscode-solution-explorer)

Optional for better experience:

- [Can I Use](https://marketplace.visualstudio.com/items?itemName=akamud.vscode-caniuse)
- [Git History](https://marketplace.visualstudio.com/items?itemName=donjayamanne.githistory)
- [Git Lens](https://marketplace.visualstudio.com/items?itemName=eamodio.gitlens)
- [gitignore](https://marketplace.visualstudio.com/items?itemName=codezombiech.gitignore)
- [HTML CSS Support](https://marketplace.visualstudio.com/items?itemName=ecmel.vscode-html-css)
- [Log File Highlighter](https://marketplace.visualstudio.com/items?itemName=emilast.LogFileHighlighter)
- [Output Colorizer](https://marketplace.visualstudio.com/items?itemName=IBM.output-colorizer)
- [Trailing Spaces](https://marketplace.visualstudio.com/items?itemName=shardulm94.trailing-spaces)
- [Todo Highlight](https://marketplace.visualstudio.com/items?itemName=wayou.vscode-todo-highlight)

At your discretion:

- [Better Comments](https://marketplace.visualstudio.com/items?itemName=aaron-bond.better-comments)
- [To Do Task](https://marketplace.visualstudio.com/items?itemName=sandy081.todotasks)
- [Remind Me](https://marketplace.visualstudio.com/items?itemName=cg-cnu.vscode-remind-me)
- [Python](https://marketplace.visualstudio.com/items?itemName=donjayamanne.python)
- [Quokka.js](https://marketplace.visualstudio.com/items?itemName=WallabyJs.quokka-vscode)

We also provide the `tasks.json` and `launch.json` configuration for Visual studio Code.

# Project overview
[![Build status](https://ci.appveyor.com/api/projects/status/mktm6ae3csndb9ma?svg=true)](https://ci.appveyor.com/project/Xarkam/base)

*SOFTINUX Base* is a free, open source and cross-platform based on ASP.NET Core and ExtCore framework. It is built using the best and the most modern tools and languages (Visual Studio 2017, Visual Studio Code, C# etc). Join our team!

## Few Facts About SOFTINUX Base

It is free and open source.<br />
It runs on Windows, Mac and Linux.<br />
It is completely modular and extendable. Using the features of the underlying ExtCore framework you can easily create your own extensions to extend its functionality.

## Basic Concepts

*Softinux Base* is a framework that looks like a .NET Core web application, but is intended to host mini web applications called extensions. Every extension will plug its content (pages, menu items) as well as security and authentication related items (permissions, roles, links...).

*Base* manages the common stuff so that the developer can focus on its extension and business logic, just having to provide what we call metadata to know how to display and authorize access to content, and use our version of Authorize attribute.

Read [this wiki page](https://github.com/SOFTINUX/Base/wiki/Writing-extensions) to learn more about this.

# Getting started

## Installation
#### 1. Restore dependencies
Go to *Barebone* folder and run `npm i --save-dev` command so that dependencies packages are installed.
#### 2. Restore nuget packages
Restore the nuGet packages is now an implicit command executed at application build.
#### 3. Generate database migration.
Go to *WebApplication* folder and run `dotnet ef migrations add InitialCreate`.<br />
(Do not take into account the error concerning the permissions table not found.)
#### 4. Update database with migration
Go to *Webapplication* folder and run `dotnet ef database update`.<br />
This will create the database. (See application.json for database. By default is a Sqlite file)
#### 5. Build the appplication
Go to the root folder and run `bp.bat` under Windows or `bp.sh` under Linux/Macos. (use -h for help)
#### 6. Run the app
Go to *WebApplication* folder and type `dotnet run`.<br/>
(If you want, you can also execute from root solution folder with this command `dotnet run --project WebApplication\WebApplication.csproj`)<br /><br />
After that, the application is available on http://localhost:5000/ <br />
##### Information About Visual Studio 2015/2017
If you launched application from Visual Studio, this port will change, <br />
being randomly defined, and value is stored in *WebApplication/Properties/launchSettings.json* <br />
You can edit this value in Visual Studio: WebApplication's properties > Debug tab > Web Server Settings/App URL or directly in launchSettings file.<br />
After, the default port used by *dotnet run* is the port defined in *WebApplication/Properties/launchSettings.json*.

#### 7. Add the first user (demo user)
With Postman (or the program of your choice) make a POST request to this url: http://localhost:5000/dev/seed/CreateUser<br />
(with curl: `curl -i -X POST http://localhost:5000/dev/seed/CreateUser`)<br />
This will create the demo user with general permissions.

#### 8. Login with demo user
user: johndoe@softinux.com<br />
password: 123_Password<br />
(password is case sensitive)

## Implement your own extension
You can use [Visual Studio 2017](https://www.visualstudio.com/fr/downloads/), [Visual Studio Code](https://code.visualstudio.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/) to make your own extension.<br />
If you decide to use Visual Studio, be aware **that projects are not compatible with Visual Studio 2015**.
### Add a new project
Using command-line (easy and cross-platform):

`dotnet new classlib -o <you_new_project>`
### Add project reference to the solution
Go to solution folder and type:

`dotnet add reference <path_to_your_new_project>`

### Write your code
In your new project, create a class that implements `Infrastructure.IExtensionMetadata`. You may also implement `Infrastructure.IExtensionDatabaseMetadata`.

Your extension will depend on `Infrastructure` and `Security.Common`.

Have a look at sample extensions, [wiki](https://github.com/SOFTINUX/Base/wiki), feel free to open issues for questions.

### Update Packages
Currently dotnet does not have a clean way to update packages in a project.

It is nevertheless expected that this will change [#6064](https://github.com/NuGet/Home/issues/6054)
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

We also provide the Tasks and Launcher of Visual studio Code configuration.
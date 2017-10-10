# Project overview

To be redacted

# Technical installation

1. Before building the app, go to *Barebone* subfolder and run `bower install` command so that bootstrap package is installed. Bower must be installed globally with `npm install -g bower`.
2. Restore the nuGet packages and build the app.
3. Go to *WebApplication* subfolder and run `gulp copy-extensions`. If gulp is installed globally but not found, run `npm link gulp` and rerun `gulp copy-extensions`.
4. Run the app.
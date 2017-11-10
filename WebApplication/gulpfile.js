// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

var gulp = require("gulp");

gulp.task(
    "copy-extensions",
    function(cb) {
        gulp.src(["../Extension1/bin/Debug/netcoreapp2.0/Extension1.dll"]).pipe(gulp.dest("Extensions"));
        gulp.src(["../Extension2/bin/Debug/netcoreapp2.0/Extension2.dll"]).pipe(gulp.dest("Extensions"));
        cb();
    }
);
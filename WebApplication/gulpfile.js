var gulp = require("gulp");

gulp.task(
    "copy-extensions",
    function(cb) {
        gulp.src(["../Extension1/bin/Debug/netstandard1.6/Extension1.dll"]).pipe(gulp.dest("Extensions"));
        cb();
    }
);
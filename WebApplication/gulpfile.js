var gulp = require("gulp");

gulp.task(
    "copy-extensions",
    function(cb) {
        gulp.src(["../Extension1/bin/Debug/netstandard1.6/Extension1.dll"]).pipe(gulp.dest("Extensions"));
        gulp.src(["../Extension2/bin/Debug/netstandard1.6/Extension2.dll"]).pipe(gulp.dest("Extensions"));
        cb();
    }
);
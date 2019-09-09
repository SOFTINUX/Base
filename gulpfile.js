'use strict';

var gulp = require('gulp');
var concat = require('gulp-concat');
var cssmin = require('gulp-cssmin');
var htmlmin = require('gulp-htmlmin');
var uglify = require('gulp-uglify');
var terser = require('gulp-terser');
var merge = require('merge-stream');
var del = require('del');
var bundleconfig = require('./bundleconfig.json');
var path = require('path');

const regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

const nodeModulesFolder = path.join(__dirname, '/node_modules');
const destination = path.join(__dirname, '/src/SoftinuxBase.Barebone/Scripts/dependencies/');

const coreDependencies = [
    { source: '/admin-lte/dist/**', dest: '/admin-lte' },

    { source: '/bootstrap/dist/**', dest: '/bootstrap' },
    { source: '/bootstrap-datepicker/dist/**', dest: '/bootstrap-datepicker' },
    { source: '/bootstrap-slider/dist/**', dest: '/bootstrap-slider' },

    { source: '/jquery/dist/**', dest: '/jquery' },
    { source: '/jquery-ui/ui/**', dest: '/jquery-ui/ui' },
    { source: '/jquery-validation/**', dest: '/jquery-validation' },
    { source: '/jquery-validation-unobtrusive/**', dest: '/jquery-validation-unobtrusive' },

    { source: '/font-awesome/css/**', dest: '/font-awesome/css' },
    { source: '/font-awesome/fonts/**', dest: '/font-awesome/fonts' },

    { source: '/inputmask/dist/**', dest: '/inputmask' },
    { source: '/ionicons/dist/**', dest: '/ionicons' },
    { source: '/icheck/skins/**', dest: '/icheck/skins' },
    { source: '/icheck/icheck.min.js', dest: '/icheck' },
    { source: '/wfk-opensans/opensans.css', dest: '/wfk-opensans' },
    { source: '/wfk-opensans/fonts/**', dest: '/wfk-opensans/fonts' },
    { source: '/toastr/build/**', dest: '/toastr' }
];

const distDependencies = [
    { source: '/bootstrap-colorpicker/dist/**', dest: '/bootstrap-colorpicker' },
    { source: '/bootstrap-daterangepicker/daterangepicker.js', dest: '/bootstrap-daterangepicker' },
    { source: '/bootstrap-daterangepicker/daterangepicker.css', dest: '/bootstrap-daterangepicker' },
    { source: '/bootstrap-timepicker/css/**', dest: '/bootstrap-timepicker' },
    { source: '/bootstrap-timepicker/js/**', dest: '/bootstrap-timepicker' },
    { source: '/chart.js/dist/**', dest: '/chart.js' },

    { source: '/ckeditor/skins/**', dest: '/ckeditor/skins' },
    { source: '/ckeditor/lang/**', dest: '/ckeditor/lang' },
    { source: '/ckeditor/plugins/**', dest: '/ckeditor/plugins' },
    { source: '/ckeditor/ckeditor.js', dest: '/ckeditor' },
    { source: '/ckeditor/config.js', dest: '/ckeditor' },
    { source: '/ckeditor/contents.css', dest: '/ckeditor' },
    { source: '/ckeditor/styles.js', dest: '/ckeditor' },

    { source: '/datatables.net/js/**', dest: '/datatables.net' },
    { source: '/datatables.net-bs/js/**', dest: '/datatables.net-bs/js' },
    { source: '/datatables.net-bs/css/**', dest: '/datatables.net-bs/css' },

    { source: '/fastclick/lib/**', dest: '/fastclick' },
    { source: '/flot/dist/**', dest: '/flot' },
    { source: '/fullcalendar/dist/**', dest: '/fullcalendar' },

    { source: '/ion-rangeslider/css/**', dest: '/ion-rangeslider/css' },
    { source: '/ion-rangeslider/js/**', dest: '/ion-rangeslider/js' },

    { source: '/jquery-knob/dist/**', dest: '/jquery-knob' },
    { source: '/jquery-sparkline/jquery.sparkline.min.js', dest: '/jquery-sparkline' },

    { source: '/jvectormap/jquery-jvectormap.min.js', dest: '/jvectormap' },
    { source: '/jvectormap/jquery-jvectormap.css', dest: '/jvectormap' },
    { source: '/jvectormap/lib/**', dest: '/jvectormap/lib' },
    { source: '/jvectormap-next/dist/**', dest: '/jvectormap-next' },

    { source: '/moment/min/moment-with-locales.min.js', dest: '/moment' },
    { source: '/morris.js/morris.min.js', dest: '/morris.js' },
    { source: '/morris.js/morris.css', dest: '/morris.js' },
    { source: '/pace/pace.js', dest: '/pace' },
    { source: '/raphael/raphael.min.js', dest: '/raphael' },
    { source: '/select2/dist/**', dest: '/select2' },
    { source: '/slimscroll/lib/slimscroll.js', dest: '/slimscroll' },
    { source: '/normalize.css/normalize.css', dest: '/normalize.css' },
    { source: '/popper.js/dist/**', dest: '/popper.js' }
];

gulp.task('copy:coreDependencies', async (done) => {
    coreDependencies.forEach((item) => {
        return gulp.src(nodeModulesFolder + item.source, { allowEmpty: true })
            .pipe(gulp.dest(destination + item.dest));
    });

    done();
});

gulp.task('copy:distDependencies', async (done) => {
    distDependencies.forEach((item) => {
        return gulp.src(nodeModulesFolder + item.source, { allowEmpty: true })
            .pipe(gulp.dest(destination + item.dest));
    });

    done();
});

gulp.task('min:js', async function () {
    merge(getBundles(regex.js).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify().on('error', console.error))
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min:es6js', async function () {
    merge(getBundles(regex.js).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(terser().on('error', console.error))
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min:css', async function () {
    merge(getBundles(regex.css).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min:html', async function () {
    merge(getBundles(regex.html).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
            .pipe(gulp.dest('.'));
    }));
});

gulp.task('min', gulp.series(['min:js', 'min:css', 'min:html']));

gulp.task('mines6', gulp.series(['min:es6js', 'min:css', 'min:html']));

gulp.task('clean', () => {
    return del(bundleconfig.map(bundle => bundle.outputFileName));
});

gulp.task('watch', () => {
    getBundles(regex.js).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(['min:js'])));

    getBundles(regex.css).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(['min:css'])));

    getBundles(regex.html).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(['min:html'])));
});

const getBundles = (regexPattern) => {
    return bundleconfig.filter(bundle => {
        return regexPattern.test(bundle.outputFileName);
    });
};

gulp.task('default', gulp.series('min'));

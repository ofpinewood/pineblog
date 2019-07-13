'use strict';

var gulp = require('gulp'),
    rimraf = require('rimraf'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    runSequence = require('run-sequence');

var paths = {
    themeroot: './wwwroot/themes/default/'
};

paths.js = paths.themeroot + '*.js';
paths.minJs = paths.themeroot + 'js/*.min.js';
paths.css = paths.themeroot + 'css/*.css';
paths.minCss = paths.themeroot + 'css/*.min.css';
paths.concatJsDest = paths.themeroot + 'js/theme.min.js';
paths.concatCssDest = paths.themeroot + 'css/theme.min.css';

gulp.task('default', function (done) {
    runSequence('sass', 'clean', 'min', function () { done(); });
});

gulp.task('sass', function () {
    return gulp.src(paths.themeroot + '/theme.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.themeroot + '/css'));
});

gulp.task('clean:js', function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task('clean:css', function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task('clean', ['clean:js', 'clean:css']);

gulp.task('min:js', function () {
    return gulp.src([paths.js, '!' + paths.minJs], { base: '.' })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:css', function () {
    return gulp.src([paths.css, '!' + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});

gulp.task('min', ['min:js', 'min:css']);

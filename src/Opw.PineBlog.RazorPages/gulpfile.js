'use strict';

var gulp = require('gulp'),
    rimraf = require('rimraf'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    runSequence = require('run-sequence');

var paths = {
    themeroot: './wwwroot/themes/default/',
    adminroot: './wwwroot/admin/'
};

gulp.task('default', function (done) {
    runSequence('clean', 'sass', 'sass-admin', 'min', function () { done(); });
});

gulp.task('sass', function () {
    return gulp.src(paths.themeroot + '/theme.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.themeroot + '/css'));
});
gulp.task('sass-admin', function () {
    return gulp.src(paths.adminroot + '/admin.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.adminroot + '/css'));
});

gulp.task('clean:js', function (cb) {
    rimraf('./wwwroot/**/js', cb);
});

gulp.task('clean:css', function (cb) {
    rimraf('./wwwroot/**/css', cb);
});

gulp.task('clean', ['clean:js', 'clean:css']);

gulp.task('min:js', function () {
    return gulp.src([paths.themeroot + '*.js', !paths.themeroot + 'js/*.min.js'], { base: '.' })
        .pipe(concat(paths.themeroot + 'js/theme.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});
gulp.task('min:js-admin', function () {
    return gulp.src([paths.adminroot + '*.js', !paths.adminroot + 'js/*.min.js'], { base: '.' })
        .pipe(concat(paths.adminroot + 'js/admin.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:css', function () {
    return gulp.src([paths.themeroot + 'css/theme.css'])
        .pipe(concat(paths.themeroot + 'css/theme.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});
gulp.task('min:css-admin', function () {
    return gulp.src([paths.adminroot + 'css/admin.css'])
        .pipe(concat(paths.adminroot + 'css/admin.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});

gulp.task('min', ['min:js', 'min:js-admin', 'min:css', 'min:css-admin']);

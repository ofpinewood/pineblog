'use strict';

var gulp = require('gulp'),
    rimraf = require('rimraf'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass');
    //runSequence = require('run-sequence');

var paths = {
    themeroot: './wwwroot/themes/default/',
    adminroot: './wwwroot/admin/'
};

gulp.task('sass', function () {
    return gulp.src(paths.themeroot + '/theme-resume.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.themeroot + '/css'));
});
gulp.task('sass-admin', function () {
    return gulp.src(paths.adminroot + '/admin-resume.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.adminroot + '/css'));
});

gulp.task('clean:js', function (cb) {
    rimraf('./wwwroot/**/js', cb);
});

gulp.task('clean:css', function (cb) {
    rimraf('./wwwroot/**/css', cb);
});

gulp.task('clean', gulp.series('clean:js', 'clean:css'));

gulp.task('min:js', function () {
    return gulp.src([paths.themeroot + '*.js', !paths.themeroot + 'js/*.min.js'], { base: '.' })
        .pipe(concat(paths.themeroot + 'js/theme-resume.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});
gulp.task('min:js-admin', function () {
    return gulp.src([paths.adminroot + '*.js', !paths.adminroot + 'js/*.min.js'], { base: '.' })
        .pipe(concat(paths.adminroot + 'js/admin-resume.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:css', function () {
    return gulp.src([paths.themeroot + 'css/theme-resume.css'])
        .pipe(concat(paths.themeroot + 'css/theme-resume.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});
gulp.task('min:css-admin', function () {
    return gulp.src([paths.adminroot + 'css/admin-resume.css'])
        .pipe(concat(paths.adminroot + 'css/admin-resume.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});

gulp.task('min', gulp.series('min:js', 'min:js-admin', 'min:css', 'min:css-admin'));

gulp.task('default', gulp.series('clean', 'sass', 'sass-admin', 'min'));
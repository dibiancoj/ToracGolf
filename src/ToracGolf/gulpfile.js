/// <binding BeforeBuild='reactValidation, reactTransformChangePw, reactCourseStats' Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    react = require('gulp-react');

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

//react
paths.reactChangePassword = paths.webroot + "js/**/ChangeMyPassword.jsx";
paths.minReactChangePassword = paths.webroot + "js/**/ChangeMyPassword.js";

paths.reactValidation = paths.webroot + "js/**/ReactValidation.jsx";
paths.minReactValidation = paths.webroot + "js/**/ReactValidation.js";

paths.reactCourseStats = paths.webroot + 'js/**/CourseStats.jsx';
paths.minReactCourseStats = paths.webroot + "js/**/CourseStats.js";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

//react transforms

gulp.task("reactTransformChangePw", function () {
    return gulp.src([paths.reactChangePassword, "!" + paths.minReactChangePassword], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("reactValidation", function () {
    return gulp.src([paths.reactValidation, "!" + paths.minReactValidation], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("reactCourseStats", function () {
    return gulp.src([paths.reactCourseStats, "!" + paths.minReactCourseStats], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("min", ["min:js", "min:css"]);

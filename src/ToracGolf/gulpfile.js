/// <binding Clean='clean' ProjectOpened='BoomerangAssets:min:js, BoomerangControlJs:min:js, BoomerangJQueryForms:min:js, BoomerangResponsiveMobile:min:js, reactFormatting, reactCourseStats, reactTransformChangePw, reactValidation' />
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
//paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

//react
paths.reactChangePassword = paths.webroot + "js/**/ChangeMyPassword.jsx";
paths.minReactChangePassword = paths.webroot + "js/**/ChangeMyPassword.js";

paths.reactValidation = paths.webroot + "js/**/ReactValidation.jsx";
paths.minReactValidation = paths.webroot + "js/**/ReactValidation.js";

paths.reactFormatting = paths.webroot + "js/**/ReactFormatting.jsx";
paths.minReactFormatting = paths.webroot + "js/**/ReactFormatting.js";

paths.reactCourseStats = paths.webroot + 'js/**/CourseStats.jsx';
paths.minReactCourseStats = paths.webroot + "js/**/CourseStats.js";

//gulp.task("clean:js", function (cb) {
//    rimraf(paths.concatJsDest, cb);
//});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

//gulp.task("clean", ["clean:js", "clean:css"]);
gulp.task("clean", ["clean:css"]);

//gulp.task("min:js", function () {
//    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
//        .pipe(concat(paths.concatJsDest))
//        .pipe(uglify())
//        .pipe(gulp.dest("."));
//});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

//react transforms

gulp.task("reactValidation", function () {
    return gulp.src([paths.reactValidation, "!" + paths.minReactValidation], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("reactFormatting", function () {
    return gulp.src([paths.reactFormatting, "!" + paths.minReactFormatting], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("reactTransformChangePw", function () {
    return gulp.src([paths.reactChangePassword, "!" + paths.minReactChangePassword], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

gulp.task("reactCourseStats", function () {
    return gulp.src([paths.reactCourseStats, "!" + paths.minReactCourseStats], { base: "." })
        .pipe(react({ harmony: false }))
        .pipe(gulp.dest('.'))
});

//layout .css with the theme
function GetWebRootPath(file) {
    return paths.webroot + file;
}

gulp.task('BoomerangLayout:css', function () {
    return gulp.src([GetWebRootPath('css/**/site.css'), GetWebRootPath('lib/**/owl.carousel.css'), GetWebRootPath('lib/**/owl.theme.css'), GetWebRootPath('lib/**/sky-forms.css')])
                .pipe(concat(GetWebRootPath('min/LayoutBoomerang.min.css')))
                .pipe(cssmin())
                .pipe(gulp.dest("."));
});


gulp.task("BoomerangControlJs:min:js", function () {
    return gulp.src([GetWebRootPath('lib/bootstraptemplate/js/jquery.mousewheel-3.0.6.pack.js'), GetWebRootPath('lib/bootstraptemplate/js/jquery.easing.js'),
                     GetWebRootPath('lib/bootstraptemplate/js/jquery.metadata.js'), GetWebRootPath('lib/bootstraptemplate/js/jquery.hoverup.js'),
                     GetWebRootPath('lib/bootstraptemplate/js/jquery.hoverdir.js'), GetWebRootPath('lib/bootstraptemplate/js/jquery.stellar.js')])
        .pipe(concat(GetWebRootPath('min/BoomerangeJQueryControls.min.js')))
        .pipe(uglify())
        .pipe(gulp.dest("."));
})

gulp.task("BoomerangAssets:min:js", function () {
    return gulp.src([GetWebRootPath('/lib/bootstraptemplate/assets/hover-dropdown/bootstrap-hover-dropdown.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/page-scroller/jquery.ui.totop.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/mixitup/jquery.mixitup.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/mixitup/jquery.mixitup.init.js'),
                     //GetWebRootPath('/lib/bootstraptemplate/assets/fancybox/jquery.fancybox.pack.js?v=2.1.5'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/waypoints/waypoints.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/milestone-counter/jquery.countTo.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/easy-pie-chart/js/jquery.easypiechart.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/social-buttons/js/rrssb.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/nouislider/js/jquery.nouislider.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/owl-carousel/owl.carousel.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/bootstrap/js/tooltip.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/bootstrap/js/popover.js')])

        .pipe(concat(GetWebRootPath('min/BoomerangAssets.min.js')))
        .pipe(uglify())
        .pipe(gulp.dest("."));
})


gulp.task("BoomerangJQueryForms:min:js", function () {
    return gulp.src([GetWebRootPath('/lib/bootstraptemplate/assets/ui-kit/js/jquery.powerful-placeholder.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/ui-kit/js/cusel.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/sky-forms/js/jquery.form.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/sky-forms/js/jquery.validate.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/sky-forms/js/jquery.maskedinput.min.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/sky-forms/js/jquery.modal.js')])

        .pipe(concat(GetWebRootPath('min/BoomerangJQueryForms.min.js')))
        .pipe(uglify())
        .pipe(gulp.dest("."));
})

gulp.task("BoomerangResponsiveMobile:min:js", function () {
    return gulp.src([GetWebRootPath('/lib/bootstraptemplate/assets/responsive-mobile-nav/js/jquery.dlmenu.js'),
                     GetWebRootPath('/lib/bootstraptemplate/assets/responsive-mobile-nav/js/jquery.dlmenu.autofill.js')])

        .pipe(concat(GetWebRootPath('min/BoomerangResponsiveMobile.min.js')))
        .pipe(uglify())
        .pipe(gulp.dest("."));
})

//gulp.task("min", ["min:js", "min:css"]);
gulp.task("min", ["min:css"]);


//in project.json i can say on publish to server, run this task
//"scripts": {
//    "prepublish": [ "npm install", "bower install", "gulp clean", "gulp min", "OnPublishToServer"]
//}
gulp.task("OnPublishToServer", ["reactValidation", "reactFormatting", "reactTransformChangePw", "reactCourseStats",
                        "BoomerangLayout:css", "BoomerangControlJs:min:js", "BoomerangAssets:min:js", "BoomerangJQueryForms:min:js",
                        "BoomerangResponsiveMobile:min:js"]);


//the following is all for angular 2 and dealing with the npm crap...copying it over to wwwroot

//var config = {
//    libBase: 'node_modules',
//    lib: [
//        //require.resolve('systemjs/dist/system.js'),
//        require.resolve('es6-shim/es6-shim.js'),
//        require.resolve('zone.js/dist/zone.js'),
//        require.resolve('angular2-in-memory-web-api/typings/main/ambient/es6-shim/index.d.ts')
//    ]
//};

//gulp.task('copy_NPM', function () {
//    return gulp.src(config.lib, { base: config.libBase })
//        .pipe(gulp.dest(paths.webroot + 'lib'));
//});


gulp.task('CopyAngular2FromNPMToWWWLib', function () {
    return gulp.src('node_modules/@angular/**/*.js')
        .pipe(gulp.dest(paths.webroot + 'lib/@angular'));
});

gulp.task('angular2-in-memory-web-api', function () {
    return gulp.src('node_modules/angular2-in-memory-web-api/**/*.js')
        .pipe(gulp.dest(paths.webroot + 'lib/angular2-in-memory-web-api'));
});

gulp.task('CopyRXJSFromNPMToWWWLib', function () {
    return gulp.src('node_modules' + '/rxjs' + "/**/*.js")
        .pipe(gulp.dest(paths.webroot + 'lib/rxjs'));
});

gulp.task('CopySystemJsFromNPMToWWWLib', function () {
    return gulp.src('node_modules' + '/systemjs' + "/**/*.js")
        .pipe(gulp.dest(paths.webroot + 'lib/systemjs'));
});

gulp.task('ES6Shim', function () {
    return gulp.src('node_modules' + '/es6-shim' + "/**/*.js")
        .pipe(gulp.dest(paths.webroot + 'lib/es6-shim'));
});

gulp.task('Reflect', function () {
    return gulp.src('node_modules' + '/reflect-metadata' + "/**/*.js")
        .pipe(gulp.dest(paths.webroot + 'lib/reflect-metadata'));
});

gulp.task('Zone', function () {
    return gulp.src('node_modules' + '/zone.js' + "/**/*.js")
        .pipe(gulp.dest(paths.webroot + 'lib/zone.js'));
});


////Moves Angular 2 & related scripts to wwwroot folder of ASP.NET Core app
//gulp.task("scriptsNStyles", () => {
//    gulp.src([
//            'es6-shim/es6-shim.min.js',
//            'systemjs/dist/system-polyfills.js',
//            'systemjs/dist/system.src.js',
//            'reflect-metadata/Reflect.js',
//            'rxjs/**',
//            'zone.js/dist/**',
//            '@angular/**',
//            'jquery/dist/jquery.*js',
//            'bootstrap/dist/js/bootstrap*.js',
//    ], {
//        cwd: "node_modules/**"
//    })
//        .pipe(gulp.dest("./wwwroot/lib"));

//    gulp.src([
//        'node_modules/bootstrap/dist/css/bootstrap.css'
//    ]).pipe(gulp.dest('./wwwroot/lib/css'));
//});
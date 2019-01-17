var gulp        = require('gulp');
var browserSync = require('browser-sync').create();
//var sass        = require('gulp-sass');

// Static Server + watching scss/html files
gulp.task('default',  function() {

    browserSync.init({
        server: "./",
        port: 3020
    });

    //gulp.watch("app/scss/*.scss", ['sass']);
    gulp.watch([
    		"./components/**/*.*",
    		"./directives/**/*.*",
    		"./factories/**/*.*",
    		"./libs/**/*.*",
    		"./modules/**/*.*",
    		"./services/**/*.*",
    		"./assets/**/*.*",
    		"./app.config.js","./app.route.js","./app.run.js"
    		]).on('change', browserSync.reload);
});

// Compile sass into CSS & auto-inject into browsers
gulp.task('sass', function() {
    return gulp.src("app/scss/*.scss")
        .pipe(sass())
        .pipe(gulp.dest("app/css"))
        .pipe(browserSync.stream());
});


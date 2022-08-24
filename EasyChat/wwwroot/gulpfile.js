const fs = require("fs");
const gulp = require("gulp");
const browserify = require("browserify");
const tsify = require("tsify");
const { EventEmitter } = require("events");
const sass = require("gulp-sass")(require("sass"));

function clean(cb) {
    if(fs.existsSync("css")) {
        fs.rmdirSync("css", { recursive: true});
    }
    if(fs.existsSync("js")) {
        fs.rmdirSync("js", { recursive: true });
    }
    fs.mkdirSync("css");
    fs.mkdirSync("js");
    
    cb();
}

function scss() {
    return gulp.src("./src/scss/**/*.scss")
        .pipe(sass().on("error", sass.logError))
        .pipe(gulp.dest("./css"));
}

function ts(cb) {
    browserify().add("src/ts/main.ts").plugin("tsify", { global: true }).bundle((err, buffer) => {
        if(err) {
            let ee = new EventEmitter();
            ee.emit("error", err);
        } else {
            fs.writeFileSync("js/main.js", buffer);
        }
    })
    
    cb();
}

exports.build = gulp.series(clean, scss, ts);
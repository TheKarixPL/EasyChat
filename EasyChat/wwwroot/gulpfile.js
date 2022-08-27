const fs = require("fs");
const gulp = require("gulp");
/*const browserify = require("browserify");
const tsify = require("tsify");
const babelify = require("babelify");
const { EventEmitter } = require("events");*/
const sass = require("gulp-sass")(require("sass"));
const ts = require("gulp-typescript");

function clean(cb) {
    if(fs.existsSync("css")) {
        fs.rmSync("css", { recursive: true});
    }
    if(fs.existsSync("js")) {
        fs.rmSync("js", { recursive: true });
    }
    fs.mkdirSync("css");
    fs.mkdirSync("js");
    
    cb();
}

function scss() {
    return gulp.src("./src/scss/**/*.scss")
        .pipe(sass({ outputStyle: "compressed" }).on("error", sass.logError))
        .pipe(gulp.dest("./css"));
}

/*function ts(cb) {
    browserify().add("src/ts/shared.ts").plugin(tsify, { global: true, target: "es6", module: "es5", moduleResolution: "node" })
        .transform(babelify).bundle((err, buffer) => {
        if(err) {
            let ee = new EventEmitter();
            ee.emit("error", err);
        } else {
            fs.writeFileSync("js/shared.js", buffer);
        }
    });
    
    cb();
}*/

function compileTypescript() {
    return gulp.src("./src/ts/**/*.ts")
        .pipe(ts({
            noImplicitAny: true,
            target: "es6",
            module: "es6",
            lib: ["es5", "es6", "dom"]
        })).pipe(gulp.dest("./js/"));
}

//exports.build = gulp.series(clean, scss, ts);
exports.build = gulp.series(clean, scss, compileTypescript);
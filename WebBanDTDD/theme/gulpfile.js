"use strict";

const autoprefixer = require("gulp-autoprefixer");
const browsersync = require("browser-sync").create();
const cleanCSS = require("gulp-clean-css");
const del = require("del");
const gulp = require("gulp");
const header = require("gulp-header");
const merge = require("merge-stream");
const plumber = require("gulp-plumber");
const rename = require("gulp-rename");
const sass = require("gulp-sass");
const uglify = require("gulp-uglify");
const jshint = require("gulp-jshint");
var concat = require("gulp-concat");
const sourcemaps = require("gulp-sourcemaps");

const pkg = require("./package.json");

const pathVendor = "vendor/";
// Set the banner content
const banner = [
  "/*!\n",
  " * <%= pkg.title %> v<%= pkg.version %> (<%= pkg.homepage %>)\n",
  " * Copyright " + new Date().getFullYear(),
  " <%= pkg.author %>\n",
  " */\n",
  "\n",
].join("");

// Clean vendor
function clean() {
  return del([pathVendor]);
}

// Clean Fonts
function cleanFonts() {
  return del(["fonts/"]);
}

// Bring third party dependencies from node_modules into vendor directory
function modules() {

  // Bootstrap JS
  var bootstrapJS = gulp
    .src("node_modules/bootstrap/dist/js/*")
    .pipe(gulp.dest(pathVendor + "bootstrap/js"));
  // Bootstrap SCSS
  var bootstrapSCSS = gulp
    .src("node_modules/bootstrap/scss/**/*")
    .pipe(gulp.dest(pathVendor + "bootstrap/scss"));
  // ChartJS
  var popperJS = gulp
    .src("node_modules/popper.js/dist/umd/*.js")
    .pipe(gulp.dest(pathVendor + "popper.js"));
  // Font Awesome
  var fontAwesome = gulp
    .src("node_modules/font-awesome/scss/**/*")
    .pipe(gulp.dest(pathVendor + "font-awesome/scss"));

  // Font Awesome
  var animateCss = gulp
    .src("node_modules/animate.css/**/*.css")
    .pipe(gulp.dest(pathVendor + "animate.css/css"));

  // jQuery
  var jquery = gulp
    .src([
      "node_modules/jquery/dist/*",
      "!node_modules/jquery/dist/core.js",
    ])
    .pipe(gulp.dest(pathVendor + "jquery/"));

  // jQuery browser
  var jqueryBrowser = gulp
    .src(["node_modules/jquery.browser/dist/*.js"])
    .pipe(gulp.dest(pathVendor + "jquery.browser/"));

  // jQuery Validation
  var jqueryValidation = gulp
    .src(["node_modules/jquery-validation/dist/*.js"])
    .pipe(gulp.dest(pathVendor + "jquery-validation/"));

  var jqueryUi = gulp
    .src(["node_modules/jquery-ui/dist/*.js"])
    .pipe(gulp.dest(pathVendor + "jquery-ui/"));

  // validatorJs
  var validatorJs = gulp
    .src(["node_modules/validator/*.js"])
    .pipe(gulp.dest(pathVendor + "validator/"));

  return merge(
    jquery,
    jqueryValidation,
    jqueryBrowser,
    validatorJs,
    jqueryUi,
    bootstrapJS,
    bootstrapSCSS,
    popperJS,
    fontAwesome,
    animateCss
  );
}

// Clean Fonts
function loadFonts() {
  // Font Awesome
  var getfontAwesome = gulp
    .src("node_modules/font-awesome/fonts/**/*")
    .pipe(gulp.dest("fonts/"));
  return merge(getfontAwesome);
}

function jsVendor() {
  var vendors = gulp
    .src([
      //'vendor/requirejs/require.js',
      "vendor/jquery/jquery.js",
      //"vendor/jquery.browser/jquery.browser.js",
      "vendor/popper.js/popper.js",
      "vendor/bootstrap/js/bootstrap.js",
      "vendor/jquery-ui/jquery-ui.js",
      //"vendor/jarallax/jarallax.js",      
      // "vendor/jquery-validation/jquery.validate.js",
      // "vendor/validator/validator.js",
      // "vendor/moment/min/moment.min.js",
      // "vendor/moment/locale/en-au.js",
      // "vendor/daterangepicker/daterangepicker.js",      
      //'js/validator.additional.js',
      // "js/bootstrap3-typeahead.js",
      // "js/lazyload.js",
    ])
    .pipe(jshint())
    .pipe(concat("vendors.js", { newLine: " " }))
    //.pipe(uglify())
    .pipe(
      header(banner, {
        pkg: pkg,
      })
    )
    .pipe(gulp.dest("./scripts"))
    .pipe(uglify())
    .pipe(rename({ suffix: ".min" }))
    .pipe(gulp.dest("./scripts"));
  return merge(vendors);
}

// CSS task
function css() {
  return gulp
    .src("scss/**/*.scss")
    .pipe(sourcemaps.init())
    .pipe(plumber())
    .pipe(
      sass({
        outputStyle: "expanded",
        includePaths: "node_modules",
      })
    )
    .on("error", sass.logError)
    .pipe(
      autoprefixer({
        browsers: ["last 2 versions"],
        cascade: false,
      })
    )
    .pipe(
      header(banner, {
        pkg: pkg,
      })
    )
    .pipe(sourcemaps.write())
    .pipe(gulp.dest("css"))
    .pipe(
      rename({
        suffix: ".min",
      })
    )
    .pipe(cleanCSS())
    .pipe(gulp.dest("css"));
  //.pipe(browsersync.stream());
}

function appJS() {
  console.log("build app javacript non vendors");
  var app = gulp
    .src([
      "js/_categories.js",
      "js/_blog.js",
      "js/_contact.js",
      "js/_single.js",
      "js/app.js",

    ])
    .pipe(jshint())
    .pipe(concat("app.js", { newLine: " " }))
    .pipe(sourcemaps.init({ loadMaps: true }))
    // .pipe(babel({
    //   presets: ['env']
    // }))
    .pipe(gulp.dest("./scripts"))
    .pipe(uglify())
    .pipe(rename({ suffix: ".min" }))
    .pipe(gulp.dest("./scripts"));

  return merge(app);
}

function jsVendor() {
  var vendors = gulp
    .src([
      //'vendor/requirejs/require.js',
      "vendor/jquery/jquery.js",
      "vendor/jquery.browser/jquery.browser.js",
      "vendor/popper.js/popper.js",
      "vendor/bootstrap/js/bootstrap.js",
      "vendor/jquery-validation/jquery.validate.js",
      "vendor/validator/validator.js",
      "vendor/jquery-ui/jquery-ui.js",
      //'js/validator.additional.js',
      //"js/bootstrap3-typeahead.js",
      //"js/lazyload.js",
    ])
    .pipe(jshint())
    .pipe(concat("vendors.js", { newLine: " " }))
    //.pipe(uglify())
    .pipe(
      header(banner, {
        pkg: pkg,
      })
    )
    .pipe(gulp.dest("scripts"))
    .pipe(uglify())
    .pipe(rename({ suffix: ".min" }))
    .pipe(gulp.dest("scripts"));
  return merge(vendors);
}
// JS task
function js() {
  return gulp
    .src(["Scripts/*.js", "!Scripts/*.min.js"])
    .pipe(uglify())
    .pipe(
      header(banner, {
        pkg: pkg,
      })
    )
    .pipe(
      rename({
        suffix: ".min",
      })
    )
    .pipe(gulp.dest("scripts"))
    .pipe(browsersync.stream());
}
function watchScssFile() {
  gulp.watch("scss/**/*.scss", css);
}
function watchJsFiles() {
  gulp.watch(
    [
      // "js/_init.js",
      // "js/_core.js",
      // "js/_home-page.js",
      // "js/app.js", 
      "js/*.js",
    ],
    appJS
  );
}
// Define complex tasks
const vendor = gulp.series(clean, modules, loadFonts);
const fonts = gulp.series(cleanFonts, loadFonts);
const watchAppFile = gulp.series(gulp.parallel(watchScssFile, watchJsFiles));
const build = gulp.series(vendor, gulp.parallel(css, jsVendor, js));

exports.vendor = vendor;
exports.build = build;
exports.watch = watchAppFile;

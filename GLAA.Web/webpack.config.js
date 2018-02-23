var path = require('path');
var jquery = require('jquery');
var webpack = require('webpack');
//var jqueryValidation = require('jquery-validation');
//var jqueryValidationUnobtrusive = require('jquery-validation-unobtrusive');
var ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
    devtool: "source-map",
    entry: {
        'main': [
            'jquery',
            'jquery-validation',
            'jquery-validation-unobtrusive',
            './wwwroot/js/site.js',
            './wwwroot/js/branchedQuestion.js',
            './wwwroot/js/hiddenContent.js',            
            './wwwroot/js/gdsValidation.js',
            './wwwroot/js/frontend-toolkit/show-hide-content.js',
            './wwwroot/js/frontend-toolkit/stick-at-top-when-scrolling.js',
            './wwwroot/sass/glaa.scss',
            './wwwroot/sass/govuk-overrides.scss',
            './wwwroot/sass/sidebar-navigation.scss'
        ],
        'admin': [
            './wwwroot/sass/admin/index.scss'
        ]
    },
    externals: {
        // require("jquery") is external and available
        //  on the global var jQuery
        "jquery": "jQuery"
    },
    module: {
        rules: [
          {
            test: /\.scss$/,
            use: ExtractTextPlugin.extract({
                fallback: "style-loader",
                use: ["css-loader", "sass-loader"]
            })
        },
        {
          test: /\.(png|jpg|gif)$/,
          use: [
            {
              loader: 'file-loader',
              options: {
                name: '[name].[ext]',
                outputPath: '/public/images/'
              }
            }
          ]
        }
      ]
    },
    output: {
        path: path.join(__dirname, '/wwwroot/'),
        filename: './js/[name].min.js',
        // export to AMD, CommonJS, or window
        libraryTarget: 'var'
    },
    plugins: [
      new ExtractTextPlugin({
          filename: "./css/[name].min.css",
          allChunks: true
        }),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery'
        })
    ]
};
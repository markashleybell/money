const { ProvidePlugin } = require('webpack');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    output: {
        filename: '[name].js',
        path: __dirname + '/wwwroot/js/dist',
        library: 'Money'
    },
    entry: {
        'money': './wwwroot/js/money.ts',
        'list': './wwwroot/js/list.ts'
    },
    devtool: 'source-map',
    plugins: [
        new CleanWebpackPlugin(),
        // This automatically adds aliases to the application scope for the specified packages
        // So packages which look for the 'jQuery' global alias still work within our app closure
        new ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery'
        })
    ],
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.json']
    },
    module: {
        rules: [
            // All files with a '.ts' or '.tsx' extension will be handled by 'ts-loader'
            { test: /\.tsx?$/, exclude: /node_modules/, loader: 'ts-loader' },
            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'
            { enforce: 'pre', test: /\.js$/, loader: 'source-map-loader' }
        ]
    },
    externals: {
        jquery: 'jQuery'
    }
};

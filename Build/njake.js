(function () {

    var 
        spawn = require('child_process').spawn,
        path = require('path'),
        fs = require('fs');

    var extend = function (obj) {
        var args = Array.prototype.slice.call(arguments);
        if (args.length == 0) return {};
        args.slice(1).forEach(function (source) {
            for (var prop in source)
                obj[prop] = source[prop];
        });
        return obj;
    };

    exports.exec = function (cmd, opts, callback) {
        var command = spawn(cmd, opts || [], { customFds: [0, 1, 2] });
        command.on('exit', function (code) { if (callback) callback(code); });
    };

    exports.msbuild = (function () {

        var defaults = {
            processor: 'x86',
            version: 'net2.0'
        };

        var task = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                args = [];

            if (!opt.file) fail('msbuild failed - file required');

            if (!opt._exe) {
                if (!opt.processor) fail('msbuild failed - processor required');
                if (!opt.version) fail('msbuild failed - version required');
                opt._exe = exports.getDotNetVersionPath(opt.version, opt.processor) + 'MSBuild.exe';
            }

            args.push(opt.file);

            if (opt.targets)
                opts.targets.forEach(function (target) { args.push('/t:' + target); });
            if (opt.properties) {
                for (var key in opt.properties)
                    args.push('/p:' + key + '=' + opt.properties[key])
            }

            args.push.apply(args, opt._parameters || []);

            exports.exec(opt._exe, args, function (code) {
                if (code !== 0) fail('msbuild failed')
                callback ? callback(code) : complete();
            });
        };

        task.setDefaults = function (opts) {
            extend(defaults, opts);
            return defaults;
        };

        return task;

    })();

    exports.xunit = (function () {

        var defaults = {};

        var task = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                args = [];

            if (!opt._exe) fail('xunit failed - _exe required');
            if (!opt.assembly) fail('xunit failed - assembly required');

            args.push(opt.assembly);

            args.push.apply(args, opt._parameters || []);

            exports.exec(opt._exe, args, function (code) {
                if (code !== 0) fail('xunit failed')
                callback ? callback(code) : complete();
            });
        };

        task.setDefaults = function (opts) {
            extend(defaults, opts);
            return defaults;
        };

        return task;

    })();

    exports.nunit = (function () {

        var defaults = {};

        var task = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                args = [];

            if (!opt._exe) fail('nunit failed - _exe required');
            if (!opt.assemblies) failed('nunit failed - assemblies required');

            opt.assemblies.forEach(function (assembly) { args.push(assembly); });

            args.push.apply(args, opt._parameters || []);

            exports.exec(opt._exe, args, function (code) {
                if (code !== 0) fail('nunit failed');
                callback ? callback(code) : complete();
            });
        };

        task.setDefaults = function (opts) {
            extend(defaults, opts);
            return defaults;
        };

        return task;

    })();

    exports.nuget = (function () {

        var defaults = {
            _exe: 'NuGet.exe'
        };

        var task = {};

        task.pack = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                args = [];

            args.push('pack');

            if (!opt._exe) fail('nuget.pack failed - _exe required');
            if (!opt.nuspec) fail('nuget.pack failed - nuspec required');
            args.push(opt.nuspec);

            if (opt.outputDirectory) {
                args.push('-OutputDirectory')
                args.push(opt.outputDirectory);
            }

            if (opt.verbose) args.push('-Verbose');

            if (opt.version) {
                args.push('-Version');
                args.push(opt.version);
            }

            if (opt.properties) {
                for (var key in opt.properties) {
                    args.push('-Properties');
                    args.push(key + '=' + opt.properties[key])
                }
            }

            args.push.apply(args, opt._parameters || []);

            exports.exec(opt._exe, args, function (code) {
                if (code !== 0) fail('nuget.pack failed')
                callback ? callback(code) : complete();
            });
        };

        task.push = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                args = [];

            args.push('push');

            if (!opt._exe) fail('nuget.push failed - _exe required');
            if (!opt.package) fail('nuget.push failed - package required');
            args.push(opt.package);

            if (opt.apiKey) args.push(opt.apiKey);
            if (opt.createOnly) args.push('-CreateOnly');
            if (opt.source && opt.source !== '') {
                args.push('-Source');
                args.push(opt.source);
            }

            args.push.apply(args, opt._parameters || []);

            exports.exec(opt._exe, args, function (code) {
                if (code !== 0) fail('nuget.push failed');
                callback ? callback(code) : complete();
            });
        };

        task.setDefaults = function (opts) {
            extend(defaults, opts);
            return defaults;
        };

        task.sources = {
            symbolSource: 'http://nuget.gw.symbolsource.org/Public/NuGet'
        };

        return task;

    })();

    exports.assemblyinfo = (function () {
        var defaults = {
            language: 'c#',
            namespaces: ['System.Reflection', 'System.Runtime.InteropServices']
        };

        var supportedLangagues = ['c#'];

        var task = function (opts, callback) {
            var opt = extend({}, defaults, opts),
                contents = '',
                assemblyValue;

            if (!opt.file) fail('assemblyinfo failed - file required');
            if (!opt.language) fail('assemblyinfo failed - language required');

            if (opt.language === 'c#') {
                if (opt.namespaces) {
                    opt.namespaces.forEach(function (ns) {
                        contents += 'using ' + ns + ';\r\n';
                    });
                }
                if (opt.assembly) {
                    for (var key in opt.assembly) {
                        assemblyValue = opt.assembly[key];
                        if (typeof assemblyValue === 'function') {
                            contents += assemblyValue();
                            continue;
                        }
                        contents += '[assembly: ' + key + '(';
                        if (typeof assemblyValue === 'boolean')
                            contents += assemblyValue ? 'true' : 'false';
                        else
                            contents += '\"' + assemblyValue + '\"';
                        contents += ')]\r\n';
                    }
                }
                fs.writeFileSync(opt.file, contents);
            } else {
                fail('assemblyinfo failed - unsupported language. choose ' + supportedLangagues);
            }

            callback ? callback(0) : complete();
        };

        task.setDefaults = function (opts) {
            extend(defaults, opts);
            return defaults;
        }

        return task;

    })();

    exports.getWinDir = function () {
        var winDir = process.env.WINDIR;
        return path.normalize((winDir.substr(-1) === '/' || winDir.substr(-1) === '\\') ? winDir : (winDir + '/')); // append / if absent
    };

    exports.dotNetVersionMapper = {
        'processor': {
            'x86': 'Framework',
            'x64': 'Framework64'
        },
        'version': {
            'net1.0': '1.0.3705',
            'net1.1': '1.1.4322',
            'net2.0': '2.0.50727',
            'net3.5': '3.5',
            'net4.0': '4.0.30319'
        }
    };

    exports.getDotNetVersionPath = function (version, processor) {

        // http://msdn.microsoft.com/en-us/library/dd414023.aspx
        // http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package#Grouping_Assemblies_by_Framework_Version

        // make it processor instead of bit, just incase MS releases FrameworkARM ;)

        var actualProcessor = exports.dotNetVersionMapper['processor'][processor];
        var actualVersion = exports.dotNetVersionMapper['version'][version];
        if (typeof actualProcessor === 'undefined' || typeof actualVersion === 'undefined') {
            fail('specified .NET framework is not supported : ' + version + '(' + processor + ')');
            return;
        }

        var netFrameworkPath = exports.getWinDir() +
                                    'Microsoft.Net\\' +
                                    exports.dotNetVersionMapper['processor'][processor] + '\\v' +
                                    exports.dotNetVersionMapper['version'][version] + '\\';
        return netFrameworkPath;
    };

})();
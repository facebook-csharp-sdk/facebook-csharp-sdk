var fs = require('fs'),
    path = require('path'),
    exec = require('child_process').exec,
    njake = require('./Build/njake'),
    msbuild = njake.msbuild,
    xunit = njake.xunit,
    nuget = njake.nuget,
    assemblyinfo = njake.assemblyinfo,
    config = {
        rootPath: __dirname,
        version: fs.readFileSync('VERSION', 'utf-8').split('\r\n')[0],
        fileVersion: fs.readFileSync('VERSION', 'utf-8').split('\r\n')[1]
    };
    docConfig = {
        docConfigFilePath: "doc-config.json",
        outputDir: "doc_output"
    };

console.log('Facebook C# SDK v' + config.version + ' (' + config.fileVersion + ')')

msbuild.setDefaults({
    properties: { Configuration: 'Release' },
    processor: 'x86',
    version: 'net4.0'
})

xunit.setDefaults({
    _exe: 'Source/packages/xunit.runners.1.9.0.1566/tools/xunit.console.clr4.x86'
})

nuget.setDefaults({
    _exe: 'Source/.nuget/NuGet.exe',
    verbose: true
})

assemblyinfo.setDefaults({
    language: 'c#'
})

desc('Build all binaries, run tests and create nuget and symbolsource packages')
task('default', ['build', 'test', 'nuget:pack', 'sdkreference:generate'])

directory('Dist/')

namespace('build', function () {

    desc('Build .NET 4.5 binaries')
    task('net45', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-Net45.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build .NET 4.0 binaries')
    task('net40', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-Net40.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build .NET 3.5 binaries')
    task('net35', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-Net35.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Windows Store binaries')
    task('winstore', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-WindowsStore.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Windows Phone 7.1 binaries')
    task('wp71', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-WP7.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Windows Phone 8 binaries')
    task('wp8', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-WP8.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Windows Phone 8.1 Silverlight binaries')
    task('wp81sl', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-WP8.1-SL.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Windows Phone 8.1 .NET CORE binaries')
    task('win81universal', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-Universal-8.1.sln',
            targets: ['Build']
        })
    }, { async: true })

    desc('Build Silverlight 5 binaries')
    task('sl5', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-SL5.sln',
            targets: ['Build']
        })
    }, { async: true })

    task('all', ['build:net45', 'build:net40', 'build:net35', 'build:wp71', 'build:wp8', 'build:sl5', 'build:winstore', 'build:wp81sl', 'build:win81universal'])

    task('mono', function (xbuildPath) {
        msbuild({
            _exe: xbuildPath || 'xbuild',
            file: 'Source/Facebook-Net40.sln',
            targets: ['Build'],
            properties: { TargetFrameworkProfile: '' }
        })
    }, { async: true })

})

task('build', ['build:all'])

namespace('clean', function () {

    task('net45', function () {
        msbuild({
            file: 'Source/Facebook-Net40.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('net40', function () {
        msbuild({
            file: 'Source/Facebook-Net40.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('net35', function () {
        msbuild({
            file: 'Source/Facebook-Net35.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('winstore', function () {
        msbuild({
            file: 'Source/Facebook-WindowsStore.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('wp71', function () {
        msbuild({
            file: 'Source/Facebook-WP7.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('wp8', function () {
        msbuild({
            file: 'Source/Facebook-WP8.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('wp81sl', function () {
        msbuild({
            file: 'Source/Facebook-WP8.1-SL.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('win81universal', function () {
        msbuild({
            file: 'Source/Facebook-Universal-8.1.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('sl5', function () {
        msbuild({
            file: 'Source/Facebook-SL5.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('all', ['clean:net45', 'clean:net40', 'clean:net35', 'clean:wp71', 'clean:wp8', 'clean:sl5', 'clean:winstore', 'clean:wp81sl', 'clean:win81universal'])

})

desc('Clean all')
task('clean', ['clean:all'], function () {
    jake.rmRf('Working/')
    jake.rmRf('Dist/')
})

namespace('tests', function () {

    task('net45', ['build:net45'], function () {
        xunit({
            assembly: 'Bin/Tests/Release/Facebook.Tests.dll'
        })
    }, { async: true })

    task('all', ['tests:net45'])

})

desc('Run tests')
task('test', ['tests:all'])

directory('Dist/NuGet', ['Dist/'])
directory('Dist/SymbolSource', ['Dist/'])

namespace('nuget', function () {

    namespace('pack', function () {

        task('nuget', ['Dist/NuGet', 'build'], function () {
            nuget.pack({
                nuspec: 'Build/NuGet/Facebook/Facebook.nuspec',
                version: config.fileVersion,
                outputDirectory: 'Dist/NuGet'
            })
        }, { async: true })


        task('symbolsource', ['Dist/SymbolSource', 'build'], function () {
            nuget.pack({
                nuspec: 'Build/SymbolSource/Facebook/Facebook.nuspec',
                version: config.fileVersion,
                outputDirectory: 'Dist/SymbolSource'
            })
        }, { async: true })

        task('all', ['nuget:pack:nuget', 'nuget:pack:symbolsource'])

    })

    namespace('push', function () {

        desc('Push nuget package to nuget.org')
        task('nuget', function(apiKey) {
            nuget.push({
                apiKey: apiKey,
                package: path.join(config.rootPath, 'Dist/NuGet/Facebook.' + config.fileVersion + '.nupkg')
            })
        }, { async: true })

        desc('Push nuget package to symbolsource')
        task('symbolsource', function(apiKey) {
            nuget.push({
                apiKey: apiKey,
                package: path.join(config.rootPath, 'Dist/SymbolSource/Facebook.' + config.fileVersion + '.nupkg'),
                source: nuget.sources.symbolSource
            })
        }, { async: true })

    })

    desc('Create NuGet and SymbolSource pacakges')
    task('pack', ['nuget:pack:all'])

})

namespace('assemblyinfo', function () {

    task('facebook', function () {
        assemblyinfo({
            file: 'Source/Facebook/Properties/AssemblyInfo.cs',
            assembly: {
                notice: function () {
                    return '// Do not modify this file manually, use jakefile instead.\r\n';
                },
                AssemblyTitle: 'Facebook',
                AssemblyDescription: 'Facebook C# SDK',
                AssemblyCompany: 'The Outercurve Foundation',
                AssemblyProduct: 'Facebook C# SDK',
                AssemblyCopyright: 'Copyright (c) 2011, The Outercurve Foundation.',
                ComVisible: false,
                AssemblyVersion: config.version,
                AssemblyFileVersion: config.fileVersion
            }
        })
    }, { async: true })

})


namespace('sdkreference', function () {

    desc('Generate the SDK Reference documentation')
    task('generate',['sdkreference:clean'], function () {
        console.log('generate doc');
        console.log('Generating using the "%s" config file.', path.resolve(docConfig.docConfigFilePath));
        console.log('Output directory: "%s".', path.resolve(docConfig.outputDir) + '\\');

        if (!fs.existsSync(docConfig.outputDir)){
            fs.mkdirSync(docConfig.outputDir);
            
        }

        var command = 'netdoc fromConfig ' + path.resolve(docConfig.docConfigFilePath) + ' ' + path.resolve(docConfig.outputDir) + '\\' ;
        exec(command, function dir(error, stdout, stderr) { console.log(stdout); });
        
    }, { async: true })

    desc('Remove the SDK Reference documentation')
    task('clean', function () {
        console.log('clean doc');
        if (fs.existsSync(docConfig.outputDir)){
            var files = fs.readdirSync(docConfig.outputDir);
            for(var i = 0; i < files.length; i++) {
                fs.unlinkSync(path.join(docConfig.outputDir,files[i]));
            }

            fs.rmdirSync(docConfig.outputDir);
        }
    })

})



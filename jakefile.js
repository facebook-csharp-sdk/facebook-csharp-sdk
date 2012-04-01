var fs = require('fs'),
	njake = require('./Build/njake'),
	msbuild = njake.msbuild,
    xunit = njake.xunit,
	nuget = njake.nuget,
	assemblyinfo = njake.assemblyinfo,
	config = {
	    version: fs.readFileSync('VERSION')
	};

console.log('Facebook C# SDK v' + config.version)

msbuild.setDefaults({
    properties: { Configuration: 'Release' },
    processor: 'x86',
    version: 'net4.0'
})

xunit.setDefaults({
    _exe: 'Tools/xunit-1.8/xunit.console.clr4.x86'
})

nuget.setDefaults({
    _exe: 'Source/.nuget/NuGet.exe',
    verbose: true
})

assemblyinfo.setDefaults({
    language: 'c#'
})

desc('Build all binaries, run tests and create nuget and symbolsource packages')
task('default', ['build', 'test', 'nuget:pack'])

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

    desc('Build WinRT(Metro) binaries')
    task('winrt', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-WinRT.sln',
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

    desc('Build Silverlight 5 binaries')
    task('sl5', ['assemblyinfo:facebook'], function () {
        msbuild({
            file: 'Source/Facebook-SL5.sln',
            targets: ['Build']
        })
    }, { async: true })

    task('all', ['build:net45', 'build:net40', 'build:net35', 'build:wp71', 'build:sl5', 'build:winrt'])

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

    task('winrt', function () {
        msbuild({
            file: 'Source/Facebook-WinRT.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('wp71', function () {
        msbuild({
            file: 'Source/Facebook-WP7.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('sl5', function () {
        msbuild({
            file: 'Source/Facebook-SL5.sln',
            targets: ['Clean']
        })
    }, { async: true })

    task('all', ['clean:net45', 'clean:net40', 'clean:net35', 'clean:wp71', 'clean:sl5', 'clean:winrt'])

})

task('clean', ['clean:all'])

namespace('tests', function () {

    task('net40', ['build:net40'], function () {
        xunit({
            assembly: 'Bin/Tests/Release/Facebook.Tests.dll'
        })
    }, { async: true })

    task('all', ['tests:net40'])

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
                version: config.version,
                outputDirectory: 'Dist/NuGet'
            })
        }, { async: true })


        task('symbolsource', ['Dist/SymbolSource', 'build'], function () {
            nuget.pack({
                nuspec: 'Build/SymbolSource/Facebook/Facebook.nuspec',
                version: config.version,
                outputDirectory: 'Dist/SymbolSource'
            })
        }, { async: true })

        task('all', ['nuget:pack:nuget', 'nuget:pack:symbolsource'])

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
                AssemblyFileVersion: config.version
            }
        })
    }, { async: true })

})

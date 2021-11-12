@echo off

echo See https://github.com/facebook-csharp-sdk/facebook-csharp-sdk.github.com/blob/master/docs/build.md for information on how to build

PUSHD "%~dp0"

jake clean

:END
POPD

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"

cd ..
msbuild IOST.sln /target:IOST:Rebuild /property:Configuration=Release /property:Platform="Any CPU"
msbuild IOST.sln /target:IOST_Net45:Rebuild /property:Configuration=Release /property:Platform=x64
msbuild IOST.sln /target:IOST_Net45:Rebuild /property:Configuration=Release /property:Platform=x86

pause
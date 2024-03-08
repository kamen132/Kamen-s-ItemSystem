
set currentDir=%~dp0

set LUBAN_DLL=%currentDir%Tools\Luban\Luban.dll
set CONF_ROOT=%currentDir%DataTables

set parentDir= %currentDir:~0,-7%

set UnityCodeOutPut=%parentDir%\Assets\Scripts\Game\Config
set UnityDataOutPut=%parentDir%\Assets\StreamingAssets\Config



dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-simple-json ^
    -d json  ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=G:\KamenFramework\Assets\Scripts\Game\Config ^
    -x outputDataDir=G:\KamenFramework\Assets\StreamingAssets\Config

pause
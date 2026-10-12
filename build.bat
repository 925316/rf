@echo off
setlocal

cd /d "%~dp0"

set "CONFIG=Release"
set "SLN=rf.sln"

if not exist "lib\BepInEx.dll" (
    echo [ERROR] lib\BepInEx.dll missing. Download the official x86 pack:
    echo   https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x86_5.4.23.5.zip
    echo then extract BepInEx\core\BepInEx.dll and 0Harmony.dll into lib\ and retry.
    pause
    exit /b 1
)

where dotnet >nul 2>nul
if errorlevel 1 (
    echo [ERROR] dotnet not found in PATH
    pause
    exit /b 1
)

echo Building %SLN% (%CONFIG%)...
dotnet build "%SLN%" -c %CONFIG%
if errorlevel 1 (
    echo.
    echo [FAILED] Build failed, see errors above.
    pause
    exit /b 1
)

if not exist "Build\%CONFIG%\rf.dll" (
    echo [FAILED] Build\%CONFIG%\rf.dll not found.
    pause
    exit /b 1
)

echo.
echo [DONE] Output: Build\%CONFIG%\rf.dll
echo Copy it manually to BepInEx\plugins\rf\ inside your Ravenfield folder.
pause

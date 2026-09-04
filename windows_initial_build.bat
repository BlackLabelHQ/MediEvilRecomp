@echo off
title MediEvilRecomp Initial Build

REM =============================================== INIT ==============================================
set "DISC=disc"
set "CONFIG=config"
set "RECOMPONE=RecompOne"
set "RECOMPILER=%RECOMPONE%\RecompOne.Recompiler"
set "MISSING=0"
set "MEDIEVILJSON=%CONFIG%/MediEvil.json"
REM ===================================================================================================

REM ============================================= Credits =============================================
if /I not "%~1"=="nocredits" (
	echo ========
	echo Credits:
	echo ========
	echo.
	echo This project was made possible by:
	echo.
    echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
)
REM ===================================================================================================

REM ================================= Did You Mean To Run This File? ==================================
if exist "generated\" (
    echo.
    echo Note:
    choice /m "You've probably already initialized this project before. Are you sure you meant to run this?"
    if errorlevel 2 (
        echo Cancelled.
	echo.
        pause
	exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Pull and Initialize Dependencies, Build C# Solutions, and Build from MediEvil.json!
echo ============================================================================================

REM ======================================== Update Submodules ========================================
echo.
echo ^> Updating Submodules...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo Failed to update submodules! The project will not work correctly without them!
    echo.
    choice /m "Do you want to continue anyway? Hint: Probably not, but if you had them already and they just failed to update..."

    if errorlevel 2 (
        echo Cancelled.
	echo.
        pause
	exit /b 1
    )

    echo Continuing anyway... Hopefully it still works!
)

echo.
REM ===================================================================================================

REM ======================================== Check for .NET 10+ ========================================
echo ^> Checking for .NET 10+...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo ERROR: .NET 10 SDK or higher is required!
    echo Please install .NET 10 SDK before continuing.
    echo Download it from:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo You have it! Aren't you such a good human?
echo.
REM ===================================================================================================

REM ========================================= Build RecompOne =========================================
echo.
echo ^> Building RecompOne...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Failed to build RecompOne! That's a huge problem. Can't continue, sorry!
    pause
    exit /b 1
)

echo Build of RecompOne's Recompiler and Runtime complete!
echo.
REM ===================================================================================================

REM ======================================== Check Disc Files =========================================
echo.
echo ^> Checking disc files...

if not exist "%DISC%\MediEvil (USA) (Track 1)" (
    echo Missing: MediEvil ^(USA^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (USA) (Track 2)" (
    echo Missing: MediEvil ^(USA^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (USA).cue" (
    echo Missing: MediEvil ^(USA^).cue
    set "MISSING=1"
)


echo Disc files found and properly named!
echo.
REM ===================================================================================================

REM ========================================== Check Config ===========================================
echo.
echo ^> Checking configuration file...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo ERROR: Missing configuration file!
    echo Expected:
    echo %CD%\config\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Config file found!
echo.

REM ======================================== Generate C# Code =========================================
echo.
echo ^> Generating Recompiled C# Code

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo Failed to generate C# code!
    pause
    exit /b 1
)

echo.
REM ===================================================================================================


REM ========================================= Build MediEvilRecomp ====================================
echo.
echo ^> Building MediEvilRecomp...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Failed to build MediEvilRecomp! That's a very huge problem. Can't continue, sorry!
    pause
    exit /b 1
)

echo Build of MediEvilRecomp complete!
echo.
REM ===================================================================================================

REM ======================================== How To Run Game ==========================================
echo.
echo ^> Running the Game Instructions!

echo Contratulations, you've successfully built MediEvilRecomp! Use windows_run.bat to play the game!

echo.
REM ===================================================================================================

echo.
pause

@echo off
title MediEvilRecomp 

REM ============================================= Credits =============================================
	echo ========
	echo Credits:
	echo ========
	echo.
	echo This project was made possible by:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo.
REM =================================================================================================

REM ========================================= Run MediEvilRecomp ====================================
echo.
echo ^> Running MediEvilRecomp...

if not exist "generated\" (
    echo Initial setup probably wasn't run, so we're running it for you... Aren't we nice?
    echo.
    echo Running windows_initial_build.bat...
    echo.
    call windows_initial_build.bat nocredits

    REM Stop if the build failed
    if errorlevel 1 (
        echo Initial build failed.
        pause
        exit /b 1
    )
)

dotnet run

echo.
REM =================================================================================================

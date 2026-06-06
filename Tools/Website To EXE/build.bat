@echo off
title Electron App Builder
color 0A
cls

echo ============================================
echo   Electron App Builder
echo ============================================
echo.

REM Pruefe ob Node.js installiert ist
node --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [FEHLER] Node.js ist nicht installiert!
    echo Bitte installiere Node.js von: https://nodejs.org
    echo.
    pause
    exit /b 1
)

REM Pruefe ob config.json existiert
if not exist "config.json" (
    echo [FEHLER] config.json nicht gefunden!
    echo Stelle sicher dass du build.bat im gleichen Ordner wie config.json ausfuehrst.
    echo.
    pause
    exit /b 1
)

echo Lese config.json...
echo.

REM Alles weitere ueber Node.js abwickeln
node build-helper.js
if %errorlevel% neq 0 (
    echo.
    echo [FEHLER] Build fehlgeschlagen!
    pause
    exit /b 1
)

echo.
echo ============================================
echo   BUILD ERFOLGREICH!
echo ============================================
echo.
echo Deine .exe liegt im "dist" Ordner.
echo.
pause

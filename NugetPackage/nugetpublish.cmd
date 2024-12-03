@echo off
setlocal enabledelayedexpansion

:: Set environment variables to the current directory and output folder
set SolutionDir=%cd%
set VersionFile=%SolutionDir%\version.txt
set OutputDir=%SolutionDir%\nupkgs
set NuGetExePath=%SolutionDir%\nuget.exe
set NuGetSource=https://pkgs.dev.azure.com/arindamdey/biplabhome/_packaging/biplabhome/nuget/v3/index.json
set ApiKey=AZ

:: Define list of excluded folders (case-insensitive)
set ExcludeFolders=ApiDummy Api.Orchestrator Api.Repository

:: Ensure version.txt exists
if not exist "%VersionFile%" (
    echo ERROR: %VersionFile% not found.
    exit /b 1
)

:: Read version from version.txt
for /F "delims=" %%i in (%VersionFile%) do set VERSION=%%i

:: Ensure version is set
if not defined VERSION (
    echo Version could not be read from %VersionFile%.
    exit /b 1
)

echo Building NuGet packages with version: %VERSION%

:: Ensure output directory exists and clean it
if exist "%OutputDir%" (
    echo Cleaning up old packages in %OutputDir%...
    del /f /q "%OutputDir%\*.nupkg"
)

mkdir "%OutputDir%"

:: Build and pack each project in the solution, excluding certain folders
for /R "%SolutionDir%" %%f in (*.csproj) do (
    set Exclude=0

    :: Check if the project file is in any of the excluded folders
    for %%x in (%ExcludeFolders%) do (
        echo %%~dpf | findstr /I "%%x" >nul
        if not errorlevel 1 (
            set Exclude=1
        )
    )

    :: If the project is not in the excluded folder, proceed with build and pack
    if !Exclude! equ 0 (
        echo Building and packing project: %%f
        dotnet build "%%f" --configuration Release
        if errorlevel 1 exit /b 1

        dotnet pack "%%f" --configuration Release --version-suffix !VERSION! --output "%OutputDir%"
        if errorlevel 1 exit /b 1

        :: Correctly set the package file path with version using delayed expansion
        set PackageFile=%OutputDir%\%%~nf.!VERSION!.nupkg

        echo Checking if the package file exists: !PackageFile!
        
        :: Check if the .nupkg file exists
        if exist "!PackageFile!" (
            echo Pushing NuGet package: !PackageFile!
            "%NuGetExePath%" push "!PackageFile!" -Source "%NuGetSource%" -ApiKey "%ApiKey%" -NonInteractive
            echo Package pushed to Azure DevOps feed.
        ) else (
            echo WARNING: Package file !PackageFile! not found.
        )
    ) else (
        echo Skipping project in excluded folder: %%f
    )
)
PAUSE
endlocal

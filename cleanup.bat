@echo off
setlocal

rem Delete .vs directories
for /d /r %%i in (.vs) do (
    if exist "%%i" (
        echo Deleting "%%i"
        rmdir /s /q "%%i"
    )
)

rem Delete obj directories
for /d /r %%i in (obj) do (
    if exist "%%i" (
        echo Deleting "%%i"
        rmdir /s /q "%%i"
    )
)

rem Delete bin directories
for /d /r %%i in (bin) do (
    if exist "%%i" (
        echo Deleting "%%i"
        rmdir /s /q "%%i"
    )
)

echo Temporary files and directories have been removed.
endlocal

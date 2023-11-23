@ECHO off
cls

ECHO Deleting all OBJ folders...
ECHO.

FOR /d /r . %%d in (obj) DO (
	IF EXIST "%%d" (		 	 
		ECHO %%d | FIND /I "\node_modules\" > Nul && ( 
			ECHO.Skipping: %%d
		) || (
			ECHO.Deleting: %%d
			rd /s/q "%%d"
		)
	)
)

ECHO.
ECHO.OBJ folders have been successfully deleted. Press any key to exit.
pause > nul
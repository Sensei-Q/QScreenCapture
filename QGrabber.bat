:: QGrabber.bat v1.2 (c) 2022 Sensei (aka 'Q')
:: A batch script for Windows that waits a specified number of seconds and takes a screenshot in a loop.
::
@echo off
SET QROOT="[edit-me]\"
SET PICTURES="C:\Users\[user-name-edit-me]\Pictures\Screen-shots\"
SET OPTIONS=-v -d

:: Delay in seconds.
SET DELAY=1

:: Set to 0 to have an infinite loop.
:: Set to >0 to have an finite loop.
SET COUNT=0

ECHO Screen grabbing to the %PICTURES% folder with a delay of %DELAY% second(s)..
IF %COUNT%==0 ECHO Infinite loop. Use Ctrl-C to break it.
:loop
   "%QROOT%QScreenCapture" %OPTIONS% "%PICTURES%picture.png"
   IF %COUNT%==0 GOTO next
   SET /A COUNT-=1
   IF %COUNT%==0 GOTO end
:next
   TIMEOUT %DELAY% > nul
GOTO loop
:end

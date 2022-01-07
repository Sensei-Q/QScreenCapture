:: QGrabber.bat v1.0 (c) 2022 Sensei (aka 'Q')
:: A batch script for Windows that waits a specified number of seconds and takes a screenshot in a loop.
::
@echo off
set QROOT="[edit-me]"
set PICTURES="C:\Users\[user-name-edit-me]\Pictures"
set OPTIONS=-v -d
set DELAY=1
echo Screen grabbing to folder %PICTURES% delay %DELAY% second(s)..
:loop
   %QROOT%\QScreenCapture %OPTIONS% %PICTURES%\picture.png
   TIMEOUT %DELAY% > nul
GOTO loop

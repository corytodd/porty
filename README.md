# porty
Serial Port Quick Name Pop-up

The application detects when a serial port is inserted and then displays a quick pop-up telling you which port number was assigned to it. This beats popping open devmgmt all day.


## Running

Currently this works best as an app that you start automatically on start up. You will only be notified of serial insertions *after* this service has started.


There is code to run this as a service but it is half-backed and not at all functional.


### Additional Stuff
Huge inspiration from some great SO answers:

 - http://stackoverflow.com/questions/1472633/wpf-application-that-only-has-a-tray-icon
 - http://stackoverflow.com/questions/3034741/create-popup-toaster-notifications-in-windows-with-net?answertab=votes#tab-top
 
 Service creation taken from Microsoft walk through:
 - https://msdn.microsoft.com/en-us/library/zt39148a%28v=vs.110%29.aspx


Goat icon provided by http://planetpeanut.uk/ under GPLv2 http://www.gnu.org/licenses/gpl-2.0.html

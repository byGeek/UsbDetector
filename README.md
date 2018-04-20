# UsbDetector
## Goal

c# demo code used to detect usb devices with GUID. Use WIN32 API RegisterNotification to receive device arrival/removal messages.



## Reference

From MSDN,  RegisterNotification function, 

> Applications send event notifications using the [**BroadcastSystemMessage**](https://msdn.microsoft.com/en-us/library/ms644932.aspx) function. Any application with a top-level window can receive basic notifications by processing the [**WM_DEVICECHANGE**](https://msdn.microsoft.com/en-us/library/aa363480.aspx) message. Applications can use the **RegisterDeviceNotification** function to register to receive device notifications.
>
> Services can use the **RegisterDeviceNotification** function to register to receive device notifications. If a service specifies a window handle in the *hRecipient* parameter, the notifications are sent to the window procedure. If *hRecipient* is a service status handle,**SERVICE_CONTROL_DEVICEEVENT** notifications are sent to the service control handler. For more information about the service control handler, see [**HandlerEx**](https://msdn.microsoft.com/en-us/library/ms683241.aspx).
>
> Be sure to handle Plug and Play device events as quickly as possible. Otherwise, the system may become unresponsive. If your event handler is to perform an operation that may block execution (such as I/O), it is best to start another thread to perform the operation asynchronously.
>
> Device notification handles returned by **RegisterDeviceNotification** must be closed by calling the [**UnregisterDeviceNotification**](https://msdn.microsoft.com/en-us/library/aa363475.aspx) function when they are no longer needed.
>
> The [DBT_DEVICEARRIVAL](https://msdn.microsoft.com/en-us/library/aa363205.aspx) and [DBT_DEVICEREMOVECOMPLETE](https://msdn.microsoft.com/en-us/library/aa363208.aspx) events are automatically broadcast to all top-level windows for port devices. Therefore, it is not necessary to call **RegisterDeviceNotification** for ports, and the function fails if the **dbch_devicetype** member is**DBT_DEVTYP_PORT**. Volume notifications are also broadcast to top-level windows, so the function fails if **dbch_devicetype** is **DBT_DEVTYP_VOLUME**. OEM-defined devices are not used directly by the system, so the function fails if **dbch_devicetype** is **DBT_DEVTYP_OEM**.



## StackOverflow Reference

1. <https://stackoverflow.com/questions/28998625/c-win32-not-receiving-dbt-devicearrival-or-dbt-deviceremovecomplete-on-wm-devi> 
2. <https://stackoverflow.com/questions/37318110/how-to-retrieve-readable-usb-vid-pid-using-dbcc-name-from-dev-broadcast-devicein>
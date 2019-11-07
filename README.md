# Kephas SharePoint document uploader service
This application helps organizations upload their digitalized documents to SharePoint Online.

# System requirements
The application runs on following operating systems:

* Windows
  * Windows 7 SP1 or newer, Windows Server 2008 R2 SP1 or newer
  * .NET Framework 4.6.1 or newer

* Linux
  * Ubuntu 16 or newer
  * Mono 6.4.0 or newer

# Installation instructions
  * Unzip the application package into a folder of your choice.
  * Make sure the user starting the application has read/write permissions in the folder where the application is installed.
  
# Starting the application in setup mode
  * Run the `sus.exe setup` command and follow the instructions on the screen.
    * Note: the setup mode does not try to connect to SharePoint, Exchange, or do anything with the documents in the file system. It is used to configure the connection and other settings. When the configuration is done, exit the application command and start it again in service mode.
  * To terminate the application issue the `quit` command in the application console.

# Starting the application in service mode
  * Run the `sus.exe` command without any further arguments and follow the instructions on the screen.
    * Note: the service mode requires a proper application configuration, otherwise the connection to SharePoint, Exchange, or to the file system may malfunction.
  * To terminate the application issue the `quit` command in the application console.
  
# Starting the application as a Windows service
  * Start a command prompt as administrator.
  * Execute the `sc` command to create the Windows service:
  `sc create sharepointdocuploader binPath= "\"<path-to-the-exe-file>\" service" start= auto DisplayName= "SharePoint Document Uploader"`
  * Start the service:
  `sc create sharepointdocuploader`

# Application documentation

* General considerations
  * [Application configuration](../../wiki/Application-configuration)
  * [Logging](../../wiki/Logging)
  * [Field value expressions](../../wiki/Field-value-expressions)
* Document sources
  * [Microsoft Exchange](../../wiki/Microsoft-Exchange-source)
  * [File System](../../wiki/File-system-source)
* Commands
  * [Text encryption](../../wiki/Text-encryption)
  * [Retrying failed uploads](../../wiki/Retrying-failed-uploads)
  * [Inspecting SharePoint user permissions](../../wiki/Inspecting-permissions)

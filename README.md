# Kephas SharePoint document uploader service
This application helps organizations upload their digitalized documents to SharePoint Online.

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

# Application documentation

* Microsoft SharePoint connection
* Document sources
  * [Microsoft Exchange](../../wiki/Microsoft-Exchange-source)
  * [File System](../../wiki/File-system-source)
* General considerations
  * [Text encryption](../../wiki/Text-encryption)
  * [Logging](../../wiki/Logging)

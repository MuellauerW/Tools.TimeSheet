# TimeSheet - Extract Data from Exchange Calendar #

## Revision History ##

|Date       |Author |Note|
|:----------|:------|:---|
|2016&#x2011;10&#x2011;27|wm|first version of readme.md created|
|-|wm|-|
|-|wm|-|
|-|wm|-|



# Introduction #
This repository contains the tool TimeSheet. This App extractcs calendar-items from an MS-Exchange calendar and writes them to a html-file. I use this tool for my invoicing. Whenever I work for a custumer I create an entry in my calendar and write the name of the customer into the subject field. At the end of each month I create a TimeSheet report and see how many hours I worked for this customer.

The Application is implemented as an .net WPF App that uses Caliburn Micro as an MVVM Framework and Nett for de/serializing toml configuration files. Furthermore I use Independetsoft.Exchange for sending http requests to the Exchange server. The app works with every on premise Exchange server that support Web Services or Exchange-Online Accounts.

The most interesting part of this app might be my use of a .toml file for storing configuration data. Obviously this program needs the clients login information for accessing the Exchange server and 

# .toml Configuration #
Toml is a file format created by Tom Preston-Werner you can find further documentation on github in this repository: [Tom's Obvious, Minimal Language]("https://github.com/toml-lang/toml" Tom's Obvious, Minimal Language)
A .toml File is a text-file that is easily readable and editable. It allows can be deserializet to an object graph und supports all imporant datatypes.

## the configuration file contains the following fields ##


|Field|Type |Note|
|:----------|:------|:---|
|ExchangeServerUri |string |Uri that defines the https endpoint of the webservice|
|Username |string |Username, either email address or Active Directory User |
|Password |string |Password of mailbox  |
|ProjectList |string[] |Default Project names for display in Dropdown Box|
|CalendarList |string[] |List of calendars that are scanned. If ommitted, the default calendar and all calendars below are scanned |
|Path |string |Default Path for html report file |
|Strategy |Enum |This enum defines how Subject string in calendar entries are compared <br> The values are: <br>AcceptAll: Any string is accepted, so the Selection in the combo box is essentially overruled <br>Contains: The search string must be found at any place in the subject of a calendar entry<br>StartsWith: The string is at the beginning<br>Equals: The string is exactly equal|



## TimeSheet.AppConfig.private.toml ##
I have configured a rule in the .gitignore file that forbids saving .AppConfig.private.toml files in the git repo. Instead I save only one TimeSheet.AppConfig.template.toml file in the repo.
This trick secures my secret Exchange server acount information from leaking into public github repositories. I a user clones this repo he only gets the TimeSheet.AppConfig.template.toml file and he must crate a TimeSheet.AppConfig.private.toml file manually and store his private credentials in this file. 
The toml private/template duality is the nicest trick of this software. Feel free to copy it.

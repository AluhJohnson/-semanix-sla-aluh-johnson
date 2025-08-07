BACKGROUND
==========

CyberArk is the credential vault of the bank. All applications database details like the Database IP Address, Username and Password are expected to be provided by CyberArk upon request by the client applications.

To get your Database connection details onboarded on CyberArk, you are expected to provide the following for each of the database connections in your application

1. Service Account Name - This is the username/user id with which you connect to the database
2. Service Account Password - This is the password with which you connect to the database
3. Database Name - This is the name of the database you are connecting to
4. Database IP - This is the IP address of the Database Server
5. Application IP - This is the IP Address of where the application is hosted or connecting from
6. Application Name - This is the name of the application.
7. Database Type - This is the type of database. example - SQL/Oracle

After onboarding, you will be given seven (7) parameters for each of the database connections in your application

1. App ID
2. Safe
3. Folder
4. Object
5. Reason
6. Connection Port
7. Connection Timeout

These parameter are expected to be kept on your configuration file as you will be using then to connect to CyberArk to get the IP Address, Username and Password of the Database you want to connect to.

INTEGRATION
===========

We have provided some libraries with you. Follow the steps below to get your connection details for every time you need to connect to the database from your code.

1. Add the provided libraries (DLLs) to your application

2. Instantiate an object of type  - VaultDetailsRequest as below, providing the correct details as provided after onboarding for a particular DB connection
	
    VaultDetailsRequest vaultDetailsRequest = new()
    {
        AppId = {Provide the App Id Value here},
        Safe = {Provide the Safe Value here},
        Folder = {Provide the Folder Value here},
        Object = {Provide the Object Value here},
        Reason = {Provide the Reason Value here},
        ConnectionPort = {Provide the Connection Port Value here},
        ConnectionTimeout = {Provide the Connection Timeout Value here}
    };

3. Instantiate an object of type  - VaultDetailsManager and call the 'GetDetails' method; passing the object of type - VaultDetailsRequest instantiated above.

4. This will give you an object of type  - 'VaultDetailsResponse' which contains the Username, Password and IP Address of the Database you which to connect to.

5. Append the parameters gotten in (4) above to the right places of your connection string and initiate your database connection.

6. You are equally expected to manage the exception in (3) above by doing that in a try-catch block.

SAMPLE INTEGRATION
==================

VaultDetailsRequest vaultDetailsRequest = new()
{
  AppId = {Provide the App Id Value here},
  Safe = {Provide the Safe Value here},
  Folder = {Provide the Folder Value here},
  Object = {Provide the Object Value here},
  Reason = {Provide the Reason Value here},
  ConnectionPort = {Provide the Connection Port Value here},
  ConnectionTimeout = {Provide the Connection Timeout Value here}
};

VaultDetailsManager vaultDetailsManager = new();
VaultDetailsResponse vaultDetailsResponse = new();

try
{
  vaultDetailsResponse = vaultDetailsManager.GetDetails(vaultDetailsRequest);
}
catch (Exception ex)
{
  //Ensure this exception is logged on the file using any of the open source loggers - log4net, Serilog, nlog and so on
  _logger.LogError(ex, "");
}

string connectionTemplate = _config["OracleConnectionStringCyberArk:ConnectionTemplate"];

//"ConnectionTemplate": "Server={IPADDRESS};Database=TestDB;user id={USERNAME};password={PASSWORD};"
string connectionString = connectionTemplate.Replace("{IPADDRESS}", vaultDetailsResponse.IpAddress).Replace("{USERNAME}", vaultDetailsResponse.UserName).Replace("{PASSWORD}", vaultDetailsResponse.Password);

//Initiate your connection normally as below
using (OracleConnection con = new OracleConnection(connectionString))



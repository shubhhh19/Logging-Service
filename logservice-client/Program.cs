using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class LoggingTestClient
{
    // Enum defining log levels
    private enum LogLevel
    {
        Info,
        Error,
        Warning,
        Debug,
        Custom,
        Fatal
    }

    /*
    * Function: Main
    * Description: The main entry point of the program. Establishes a connection with the server and provides a menu for user interaction.
    * Parameters: 
    *   args - Command-line arguments (string[] args)
    * Return Values: None
    */
    static void Main(string[] args)
    {
        // Check if correct number of command-line arguments are provided
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: LoggingTestClient.exe <IP_ADDRESS> <PORT>");
            Environment.Exit(1);
        }

        string serverAddress = args[0];
        int serverPort = int.Parse(args[1]);

        // Default log message format
        string logMessageFormat = "[{timestamp}] [{level}]: {message}";

        try
        {
            // Establish TCP connection with the server
            using (TcpClient client = new TcpClient(serverAddress, serverPort))
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                // Main loop for user interaction
                while (true)
                {
                    DisplayMenu();

                    Console.Write("Select an option (1-4): ");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            AutoLogs(writer, ref logMessageFormat);
                            break;
                        case "2":
                            ManualLogEntry(writer, ref logMessageFormat);
                            break;
                        case "3":
                            NoisyLogs(writer, logMessageFormat);
                            return; // Terminate the connection for option 3
                        case "4":
                            Exit(writer, logMessageFormat);
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    /*
    * Function: DisplayMenu
    * Description: Displays the menu options for the user.
    * Parameters: None
    * Return Values: None
    */
    private static void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Auto (Print all log messages and Configure Log Format)");
        Console.WriteLine("2. Manual (Enter log message and Configure Log Format)");
        Console.WriteLine("3. Noisy (Test noisy logs)");
        Console.WriteLine("4. Exit");
    }

    /*
    * Function: ConfigureLogFormat
    * Description: Allows the user to configure the log message format.
    * Parameters: 
    *   logMessageFormat - Reference to the log message format string (ref string logMessageFormat)
    * Return Values: None
    */
    private static void ConfigureLogFormat(ref string logMessageFormat)
    {
        Console.WriteLine("Current log message format: " + logMessageFormat);
        Console.WriteLine("Choose log message format:");
        Console.WriteLine("1. [{timestamp}] [{level}]: {message}");
        Console.WriteLine("2. [{level}] {timestamp}: {message}");
        Console.WriteLine("3. {message}: [{level}] {timestamp}");

        Console.Write("Select an option (1-3): ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                logMessageFormat = "[{timestamp}] [{level}]: {message}";
                break;
            case "2":
                logMessageFormat = "[{level}] {timestamp}: {message}";
                break;
            case "3":
                logMessageFormat = "{message}: [{level}] {timestamp}";
                break;
            default:
                Console.WriteLine("Invalid option. Using default log format.");
                break;
        }
    }

    /*
    * Function: AutoLogs
    * Description: Generates and sends auto log messages.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   logMessageFormat - Reference to the log message format string (ref string logMessageFormat)
    * Return Values: None
    */
    private static void AutoLogs(StreamWriter writer, ref string logMessageFormat)
    {
        ConfigureLogFormat(ref logMessageFormat);

        LogMessage(writer, LogLevel.Info, "Client connected to the server", logMessageFormat);
        LogMessage(writer, LogLevel.Info, "Auto log messages: All log messages printed at once", logMessageFormat);
        LogMessage(writer, LogLevel.Error, "Database connection failed.", logMessageFormat);
        LogMessage(writer, LogLevel.Warning, "Low disk space. Consider freeing up space.", logMessageFormat);
        LogMessage(writer, LogLevel.Debug, "Debugging information: Session ID - 696969", logMessageFormat);
    }

    /*
    * Function: ManualLogEntry
    * Description: Allows the user to enter a log message manually.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   logMessageFormat - Reference to the log message format string (ref string logMessageFormat)
    * Return Values: None
    */
    private static void ManualLogEntry(StreamWriter writer, ref string logMessageFormat)
    {
        ConfigureLogFormat(ref logMessageFormat);

        Console.Write("Enter log message: ");
        string customLogMessage = Console.ReadLine();

        Console.WriteLine("Choose log level:");
        DisplayLogLevelOptions();

        Console.Write("Select log level: ");
        string logLevelInput = Console.ReadLine();

        if (Enum.TryParse<LogLevel>(logLevelInput, out LogLevel logLevel))
        {
            LogMessage(writer, logLevel, customLogMessage, logMessageFormat);
        }
        else
        {
            Console.WriteLine("Invalid log level. Log message not sent.");
        }
    }

    /*
    * Function: DisplayLogLevelOptions
    * Description: Displays the available log level options.
    * Parameters: None
    * Return Values: None
    */
    private static void DisplayLogLevelOptions()
    {
        foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
        {
            Console.WriteLine($"{(int)level}. {level}");
        }
    }

    /*
    * Function: NoisyLogs
    * Description: Terminates the connection for option 3 and displays a message.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   logMessageFormat - Reference to the log message format string (string logMessageFormat)
    * Return Values: None
    */
    private static void NoisyLogs(StreamWriter writer, string logMessageFormat)
    {
        Console.WriteLine("Service abuse prevention system blocked overly noisy client");
        writer.Close();
        Environment.Exit(0);
    }

    /*
    * Function: Exit
    * Description: Exits the program, sends a log message, and displays a message.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   logMessageFormat - Reference to the log message format string (string logMessageFormat)
    * Return Values: None
    */
    private static void Exit(StreamWriter writer, string logMessageFormat)
    {
        LogMessage(writer, LogLevel.Info, "Client disconnected from the server", logMessageFormat);
        Console.WriteLine("Exiting the program.");
    }

    /*
    * Function: LogMessage
    * Description: Formats and sends a log message to the server.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   level - Log level (LogLevel level)
    *   message - Log message content (string message)
    *   logMessageFormat - Log message format (string logMessageFormat)
    * Return Values: None
    */
    private static void LogMessage(StreamWriter writer, LogLevel level, string message, string logMessageFormat)
    {
        string logMessage = logMessageFormat
            .Replace("{level}", level.ToString())
            .Replace("{timestamp}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"))
            .Replace("{message}", message);

        SendLogMessage(writer, logMessage);
    }

    /*
    * Function: SendLogMessage
    * Description: Sends a formatted log message to the server.
    * Parameters: 
    *   writer - StreamWriter for writing log messages to the server (StreamWriter writer)
    *   logMessage - Formatted log message (string logMessage)
    * Return Values: None
    */
    private static void SendLogMessage(StreamWriter writer, string logMessage)
    {
        writer.WriteLine(logMessage);
        writer.Flush();
    }
}

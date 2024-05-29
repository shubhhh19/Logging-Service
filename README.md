# Logging-Service

### Description:
I've developed a robust logging service along with a versatile test client. This logging service enables detailed access and diagnostic logging, crucial for monitoring and troubleshooting network applications. It's a standalone tool designed to log messages from various clients over TCP or UDP connections, storing the data in plain text files.

### Tools Used:
For the logging service, I chose Python due to its simplicity and effectiveness for network programming. I opted for Git for version control and GitHub for hosting my project repository.

### Features:
1. Network-based logging service supporting TCP or UDP connections.
2. Configuration via command-line arguments or config file for flexibility.
3. Customizable log message format to meet specific project requirements.
4. Support for simultaneous connections from multiple clients with identification.
5. Implementation of rate limiting to prevent abuse from overly noisy clients.
6. Test client developed in a different language (e.g., Java) to ensure thorough testing.
7. Manual and automated testing capabilities within the test client for comprehensive validation.

### Menu:
1. Auto (Print all log messages and Configure Log Format)
2. Manual (Enter log message and Configure Log Format)
3. Noisy (Test noisy logs)
4. Exit

- Option 1: Auto (Print all log messages and Configure Log Format)
Configures the log format.
Sends multiple log messages with various log levels to the server.
- Option 2: Manual (Enter log message and Configure Log Format)
Configures the log format.
Prompts the user to enter a custom log message and select a log level.
Sends the custom log message to the server.
- Option 3: Noisy (Test noisy logs)
Prints a message indicating that the service abuse prevention system blocked the client.
Terminates the connection.
- Option 4: Exit
Sends a log message indicating that the client is disconnecting.
Terminates the connection.


### Install and Run Instructions:
1. Clone the repository from GitHub using `git clone repo`.
2. You'll need to be connected to a VPN to run it. (I used ZeroTier)
3. Navigate to the relevant directory (`logging_service` or `test_client`).
4. Run the logging service server configuring the `config.properties` file. You can change the IP address and port number according to your VPN.
5. Execute the test server.
6. Run the client side code now.
7. Once the server is up and running, run the client from different desktop and connect to the IP and port from the terminal.
8. After connected, the menu will appear.

### Interesting Parts during the Build Process:
Implementing rate limiting in the logging service was particularly intriguing. I had to devise a mechanism to track and control the influx of log messages from various clients without compromising performance or functionality.

### Difficulties we had and how we overcame them:
One challenge was ensuring compatibility and interoperability between the logging service and the test client, given their different language implementations. However, thorough planning and communication helped in defining clear interfaces and message formats, facilitating seamless integration during testing and demonstration.

### Future Updates and Fixes:
In future updates, I aim to enhance the logging service with additional features such as log rotation for managing large log files, support for log levels, and integration with existing logging frameworks for more advanced functionality. Additionally, I plan to optimize the test client for better performance and extend its capabilities for more comprehensive testing scenarios.

### Troubleshooting:
- Cannot Access Disposed Object: Ensure that the client and server are running and that the correct IP address and port number are used.
- Compilation Errors: Verify that the Java files are correctly located in the project structure and that you are running the correct commands.
- Connection Issues: Ensure that there are no firewall rules blocking the connection and that the server is reachable from the client machine


### Screenshots:
Server running:
<img src="https://github.com/shubhhh19/Logging-Service/assets/126296317/d95b5a87-38af-4f10-af0a-8f209b2aee0f" width="1000" height="200">

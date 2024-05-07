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

### Install and Run Instructions:
1. Clone the repository from GitHub using `git clone repo`.
2. Navigate to the relevant directory (`logging_service` or `test_client`).
3. Run the logging service using `python logging_service.py <arguments>` (e.g., `python logging_service.py --port 5000`).
4. Execute the test client using `java -jar test_client.jar <options>` (e.g., `java -jar test_client.jar --host localhost --port 5000`).

### Interesting Parts during the Build Process:
Implementing rate limiting in the logging service was particularly intriguing. I had to devise a mechanism to track and control the influx of log messages from various clients without compromising performance or functionality.

### Difficulties we had and how we overcame them:
One challenge was ensuring compatibility and interoperability between the logging service and the test client, given their different language implementations. However, thorough planning and communication helped in defining clear interfaces and message formats, facilitating seamless integration during testing and demonstration.

### Future Updates and Fixes:
In future updates, I aim to enhance the logging service with additional features such as log rotation for managing large log files, support for log levels, and integration with existing logging frameworks for more advanced functionality. Additionally, I plan to optimize the test client for better performance and extend its capabilities for more comprehensive testing scenarios.

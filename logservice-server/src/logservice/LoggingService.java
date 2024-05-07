package logservice;

import java.io.*;
import java.net.*;
import java.util.Properties;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

/**
 * LoggingService - A simple logging server that accepts log messages from clients.
 */

public class LoggingService {

    // Maximum number of log messages allowed per second
    private static final int MAX_LOG_MESSAGES_PER_SECOND = 5;

    // Queue to store timestamps of incoming log messages
    private static final BlockingQueue<Long> logTimestamps = new LinkedBlockingQueue<>();

    /**
     * Main method to start the logging service.
     *
     * @param args Command line arguments (not used in this application).
     */
    public static void main(String[] args) {
        // Load configuration properties for the server
        Properties properties = loadProperties("/Users/its.shubhhh/Desktop/logservice/src/logservice/config.properties");
        String ipAddress = properties.getProperty("ipAddress");
        int port = Integer.parseInt(properties.getProperty("port"));

        // Log server startup message
        logEvent("Server started");

        try (ServerSocket serverSocket = new ServerSocket(port, 50, InetAddress.getByName(ipAddress))) {
            // Log information about the server being up and running
            logEvent("Server is running on " + ipAddress + ":" + port);

            while (true) {
                // Accept incoming client connections
                Socket clientSocket = serverSocket.accept();

                // Log information about a new client connection
                logEvent("New client connected: " + clientSocket.getInetAddress().getHostAddress());

                // Notify the user about successful client connection
                PrintWriter notifyWriter = new PrintWriter(clientSocket.getOutputStream(), true);
                notifyWriter.println("Connected to the logging service successfully.");

                // Handle the client in a separate thread
                new Thread(() -> handleClient(clientSocket)).start();
            }
        } catch (IOException e) {
            // Log error in case of an exception during server operation
            logError("Error during server operation: " + e.getMessage());
        } finally {
            // Log server disconnection message regardless of success or failure
            logEvent("Server disconnected");
        }
    }

    /**
     * Load properties from a configuration file.
     *
     * @param filename Path to the configuration file.
     * @return Properties object containing loaded configuration.
     */
    private static Properties loadProperties(String filename) {
        Properties properties = new Properties();
        try (InputStream input = new FileInputStream(filename)) {
            properties.load(input);
        } catch (IOException e) {
            // Log error in case of an exception during properties loading
            logError("Error loading properties from " + filename + ": " + e.getMessage());
        }
        return properties;
    }

    /**
     * Handle communication with a connected client.
     *
     * @param clientSocket Socket representing the connected client.
     */
    private static void handleClient(Socket clientSocket) {
        try (BufferedReader reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
             BufferedWriter logWriter = new BufferedWriter(new FileWriter("logs.txt", true));
             PrintWriter writer = new PrintWriter(clientSocket.getOutputStream(), true)) {

            String logMessage;
            while ((logMessage = reader.readLine()) != null) {
                // Implement rate limiting
                if (isAllowedToLog()) {
                    // Save the log message to the logs.txt file
                    logWriter.write(logMessage);
                    logWriter.newLine();
                    logWriter.flush();

                    // Notify the client that the server is ready for another log message
                    writer.println("Ready for next log message. Type 'exit' to stop entering.");
                } else {
                    logEvent("Rate limit exceeded. Slow down your log messages");
                }
            }
        } catch (SocketException e) {
            // Handle socket exception (client disconnect) gracefully
            logEvent("Client disconnected: " + clientSocket.getInetAddress().getHostAddress());
        } catch (IOException e) {
            // Log error in case of an exception during client handling
            logError("Error during client handling: " + e.getMessage());
        }
    }

    /**
     * Check if the server is allowed to log a message based on the rate limit.
     *
     * @return True if allowed to log, false otherwise.
     */
    private static boolean isAllowedToLog() {
        long currentTimeMillis = System.currentTimeMillis();

        if (logTimestamps.size() < MAX_LOG_MESSAGES_PER_SECOND) {
            logTimestamps.offer(currentTimeMillis);
            return true;
        } else {
            long oldestTimestamp = logTimestamps.poll();
            if (currentTimeMillis - oldestTimestamp < 1000) {
                // If more than MAX_LOG_MESSAGES_PER_SECOND messages are received within 1 second, deny logging.
                return false;
            } else {
                logTimestamps.offer(currentTimeMillis);
                return true;
            }
        }
    }

    /**
     * Log an event message.
     *
     * @param eventMessage Message describing the event.
     */
    private static void logEvent(String eventMessage) {
        System.out.println(eventMessage);
    }

    /**
     * Log an error message.
     *
     * @param errorMessage Message describing the error.
     */
    private static void logError(String errorMessage) {
        System.err.println(errorMessage);
    }
}
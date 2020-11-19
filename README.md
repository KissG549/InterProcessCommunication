# InterProcessCommunication
 
##  Build and run the system

// TODO
### Build

### Run

To run in client mode use the *-c* switch with the *server address* and the *port number* :
    
    InterProcessCommunication.exe -c 127.0.0.1:3001
    
    or

    InterProcessCommunication.exe -c localhost:3001
    
    or
    
    InterProcessCommunication.exe -c myserver.local:3001
    
To run in server mode use the *-s* switch with the *listening IP* and *port number* :

    InterProcessCommunication.exe -s 3001   \\ will listen on localhost:3001

    or 

    InterProcessCommunication.exe -s 127.0.0.1 3001 \\ will listen on localhost:3001
    
Print help

    InterProcessCommunication.exe ?

## What the system does

 * The system is able to connect to a server and exchange information via *TCP socket/Json* bidirectionaly.
 * Can also act as server
 * Process messages asynchronously
 * If you disconnected, you need to restart both end

## What will happend when I start the program?

 * Start the appropriate mode, *client* or *server*
 * As client
    * Try to connect to the specific server
    * Start a receive thread to process the incoming messages independetly
        * Deserialize incoming message
        * Convert them into objects
        * Print the incoming messages and the converted objects
    * Send **pre-defined data** to the server
    * Send **random generated data** to the server
    * **Wait for user provided data** from the console
        * Send user provided data to the server
 * As server
    * Listen on specific address and port
    * Wait for incoming connection
        * Start a receive thread to process the incoming messages independetly
        * Deserialize incoming message
        * Convert them into objects
        * Print the incoming messages and the converted objects
    * Send **pre-defined data** to the client
    * Send **random generated data** to the client
    * **Wait for user provided data** from the console
        * Send user provided data to the client

### How does it works?

 * Start the appropriate mode, *client* or *server*
 * Wait for incoming messages independently from the main thread
    * Deserialize incoming data and print it to the console
 * Send sample data from the main thread - async
    * Based on pre-defined data
    * Based on random generated data
    * Based on user provided data


## Outline

### System strengths

// TODO

### System weaknesses

// TODO

# InterProcessCommunication
 
##  Build and run the system

### Prerequisites

    Dotnet core SDK 3.1
    https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.404-windows-x64-installer

### Download

    git clone https://github.com/KissG549/InterProcessCommunication.git

### Build

    dotnet build    

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

**!!!You have to run a server and also a client!!!**

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

    * Non-blocking message processing, running on independent Thread
    * Objects can be easily serialize with Json
    * Send data in Json format, simple to implement and modify
    * Can easily add new objects to the serialization
    * Works with multiple objects at the same time
    * Client-server mode, participants can be on the same machine or on different network either (with some restrictions)
    * Using TCP, which is reliable
    * Bidirectional data exchange

### System weaknesses

    * Handles one connection 
    * Thread signaling not used in this implementation
    * Data types should be the same on the server and the client side to being convertable
    * Variable names are used for the serialization and deserialization
    * Network communication is not encrypted
    * TCP could be slower than UDP
    * Need to restart the both end if the connection lost
    * Doesn't support containerization/virtualization yet

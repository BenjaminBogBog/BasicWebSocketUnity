This is my attempt at creating a basic websocket client-server relationship using Nodejs + Unity. 
Currently it just handles the transfer basic string messages which can be passed into basic value types or however you code the parsing of it, so really it can only be used to create chat clients right now or something similar. However in the future I might attempt a more complex structure to it. Which was the original plan, to communicate simple data, however I got a little curious and wanted to see how it would perform as a netcode so I'm trying to add more to it.

Basic Scene Set-Up:

Server:
Open cmd and navigate to Assets/_Script/WebSocketServer and then type the commands "npm i" and then "npm start", this will open the server and your server should be up at "localhost://PORT//"

Client (UNITY):
In your scene,
- Create an empty GameObject
- Attach the script NetworkConnection.cs to it
- Call Connect() from the NetworkConnection class

Usage:

method: 
- NetworkConnection.Send(string message) - Send messages to any listening clients.

events:
- NetworkConnection.OnConnected(int id) - (id: client's id) - called when the client connects to the web socket.
- NetworkConnection.OnDisconnect() - called when the client disconnects from the web socket.
- NetworkConnection.OnMessageReceived(string action, string message) - (action: specified action by sender, message: message sent by sender) - called when the client receives a message from the web socket.
- NetworkConnection.OnTick() - called every tick.

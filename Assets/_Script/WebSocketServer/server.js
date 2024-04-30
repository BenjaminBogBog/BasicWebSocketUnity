const WebSocket = require('ws')
const fs = require('fs');
const path = require('path');

const wss = new WebSocket.Server({ port: 8080 },()=>{
    console.log('server started')
})

let id = 0;
let clientIds = [];

wss.on('connection', function connection(ws) {

   let clientId = id++;
   clientIds.push(clientId);

   console.log('connection received! ' + clientId);

   SendDesignatedId(clientId);
   RegisterTick();

   SendMessage("spawn", '');

   ws.on('message', (data) => {
      console.log('data received \n %o',data)

      let dataString = new Buffer.from(data).toString();
      console.log(dataString);
   })
})
wss.on('listening',()=>{
   console.log('listening on 8080')
})

function SendDesignatedId(id){
   clientIds.forEach((clientId)=>{
      if(clientId == id){
         SendMessage('id', id);
      }
   })
}

function RegisterTick(){
   setInterval(()=>{
      SendMessage('tick', '');
   },1000/60)
}

function SendMessage(action, message){
   wss.clients.forEach((client)=>{
      client.send(`${action}:${message}`)
   })
}
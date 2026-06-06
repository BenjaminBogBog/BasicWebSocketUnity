using BogBog.Utility;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using System.Threading;

public class NetworkConnection : Singleton<NetworkConnection>
{
    public delegate void OnConnected(int id);
    public event OnConnected onConnected;

    public delegate void OnMessageReceived(string action, string message);
    public event OnMessageReceived onMessageReceived;

    public delegate void OnDisconnected();
    public event OnDisconnected onDisconnected;

    private bool _connected;
    public bool Connected => _connected;

    [Header("Connection Settings")]

    [SerializeField] private string serverAddress;

    private WebSocket ws;
    public void Connect()
    {
        ws = new WebSocket(serverAddress);

        ws.Connect();

        ws.OnOpen += (sender, e) =>
        {
            // Waiting for message.
        };

        ws.OnClose += (sender, e) =>
        {
            onDisconnected?.Invoke();
        };

        ws.OnMessage += (sender, e) =>
        {
            MainThreadWorker.Instance.AddAction(() =>
            {
                IdentifyMessage(e.Data);
            });
            
        };
    }

    private void OnDisable()
    {
        ws.Close();
        
    }

    private void IdentifyMessage(string rawString){

        string action = rawString.Split(":")[0];
        string message = rawString.Split(":")[1];

        if (action == "tick"){
            return;
        }

        if(action == "id"){
            Debug.Log("ID: " + message);
            onConnected?.Invoke(int.Parse(message));
            _connected = true;
            return;
        }

        onMessageReceived?.Invoke(action, message);
    }

    public void SendToAll(string message){
        Debug.Log("Send: " + message);
        
        string action = "all";
        string fullMessage = $"{action}:{message}";
        ws.Send(fullMessage);
    }
    
    public void SendToServer(string message){
        Debug.Log("Send: " + message);
        
        string action = "server";
        string fullMessage = $"{action}:{message}";
        ws.Send(fullMessage);
    }
    
    public void SendToPlayer(int playerId, string message){
        Debug.Log("Send: " + message);
        
        string action = $"player";
        string fullMessage = $"{action}:{playerId}:{message}";
        ws.Send(fullMessage);
    }
}

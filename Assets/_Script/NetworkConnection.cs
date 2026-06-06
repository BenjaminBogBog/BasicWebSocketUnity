using System;
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
    
    public delegate void OnRoomCreated(NetworkRoom room);
    public event OnRoomCreated onRoomCreated;
    
    public delegate void OnRoomJoined(NetworkRoom room);
    public event OnRoomJoined onRoomJoined;
    
    public delegate void OnRoomUpdated(NetworkRoom room);
    public event OnRoomUpdated onRoomUpdated;

    public delegate void OnRoomLeft();
    public event OnRoomLeft onRoomLeft;
    
    private bool _connected;
    public bool Connected => _connected;

    [Header("Connection Settings")]

    [SerializeField] private string serverAddress;
    
    #if UNITY_EDITOR
    public int testRoomJoinId = 0;
    #endif

    private WebSocket _ws;
    public void Connect()
    {
        if(_ws != null && _ws.IsAlive)
        {
            Debug.LogWarning("Already connected to server.");
            return;
        }
        _ws = new WebSocket(serverAddress);

        _ws.Connect();

        _ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to Server");
            // Waiting for message.
        };

        _ws.OnClose += (sender, e) =>
        {
            onDisconnected?.Invoke();
        };

        _ws.OnMessage += (sender, e) =>
        {
            MainThreadWorker.Instance.AddAction(() =>
            {
                IdentifyMessage(e.Data);
            });
            
        };
    }

    private void OnDisable()
    {
        if(_ws is { IsAlive: true })
            _ws.Close();
    }

    private void IdentifyMessage(string rawString){

        int separatorIndex = rawString.IndexOf(':');

        string action = rawString.Substring(0, separatorIndex);
        string message = rawString.Substring(separatorIndex + 1);

        if (action == "tick"){
            return;
        }

        if(action == "id"){
            Debug.Log("ID: " + message);
            onConnected?.Invoke(int.Parse(message));
            _connected = true;
            return;
        }
        
        if(action == "room_created"){
            Debug.Log("Room Created: " + message.ToString());
            
            NetworkRoom newRoom = JsonUtility.FromJson<NetworkRoom>(message);
            onRoomCreated?.Invoke(newRoom);
            return;
        }
        
        if(action == "room_joined"){
            Debug.Log("Room Joined: " + message);
            
            NetworkRoom joinedRoom = JsonUtility.FromJson<NetworkRoom>(message);
            onRoomJoined?.Invoke(joinedRoom);
            return;
        }

        if (action == "update_room")
        {
            Debug.Log("Room Updated: " + message);
            
            NetworkRoom updatedRoom = JsonUtility.FromJson<NetworkRoom>(message);
            onRoomUpdated?.Invoke(updatedRoom);
            return;
        }
        
        if(action == "room_left"){
            Debug.Log("Left Room");
            onRoomLeft?.Invoke();
            return;
        }
        
        if(action == "error"){
            Debug.LogError("Error: " + message);
            return;
        }
        

        onMessageReceived?.Invoke(action, message);
    }

    public void SendToAll(string message){
        Debug.Log("Send: " + message);
        
        string action = "all";
        string fullMessage = $"{action}:{message}";
        _ws.Send(fullMessage);
    }
    
    public void SendToServer(string message){
        Debug.Log("Send: " + message);
        
        string action = "server";
        string fullMessage = $"{action}:{message}";
        _ws.Send(fullMessage);
    }
    
    public void SendToPlayer(int playerId, string message){
        Debug.Log("Send: " + message);
        
        string action = $"player";
        string fullMessage = $"{action}:{playerId}:{message}";
        _ws.Send(fullMessage);
    }


    public void CreateRoom()
    {
        onRoomCreated += RoomCreated;
        SendToServer($"create_room");
    }

    public void JoinRoom(int roomId)
    {
        SendToServer($"join_room,{roomId}");
    }
    
    public void LeaveRoom()
    {
        SendToServer("leave_room");
    }

    private void RoomCreated(NetworkRoom room)
    {
        onRoomCreated -= RoomCreated;
        Debug.Log("Room Created with ID: " + room.id);
    }
}

#region Network Data Classes

[Serializable]
public class NetworkClient
{
    public int id;
}

[Serializable]
public class NetworkRoom
{
    public int id;
    public List<NetworkClient> clients;
}

#endregion

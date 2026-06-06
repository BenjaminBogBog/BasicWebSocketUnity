using BogBog.Utility;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using DG.Tweening;

public class NetworkManager : Singleton<NetworkManager> {

    public int LocalId = -1;
    private List<NetworkBehaviour> networkBehaviours;
    
    [Header("Server Settings")]
    [SerializeField] private GameObject playerPrefab;

    public NetworkRoom currentRoom;

    private void Start() {

        networkBehaviours = new List<NetworkBehaviour>();
        NetworkConnection.Instance.onConnected += OnConnected;
        NetworkConnection.Instance.onMessageReceived += OnMessageReceived;
        NetworkConnection.Instance.onDisconnected += OnDisconnected;
        
        // ROOM MANAGEMENT
        NetworkConnection.Instance.onRoomCreated += OnRoomCreated;
        NetworkConnection.Instance.onRoomJoined += OnRoomJoined;
        NetworkConnection.Instance.onRoomUpdated += OnRoomUpdated;
        NetworkConnection.Instance.onRoomLeft += OnRoomLeft;
    }

    

    private void OnDestroy()
    {
        NetworkConnection.Instance.onConnected -= OnConnected;
        NetworkConnection.Instance.onMessageReceived -= OnMessageReceived;
        NetworkConnection.Instance.onDisconnected -= OnDisconnected;
        
        // ROOM MANAGEMENT
        NetworkConnection.Instance.onRoomCreated -= OnRoomCreated;
        NetworkConnection.Instance.onRoomJoined -= OnRoomJoined;
        NetworkConnection.Instance.onRoomUpdated -= OnRoomUpdated;
        NetworkConnection.Instance.onRoomLeft -= OnRoomLeft;
    }
    
    private void OnRoomLeft()
    {
        Debug.Log("Left Room");
        currentRoom = null;
    }
    
    private void OnRoomUpdated(NetworkRoom room)
    {
        Debug.Log($"Room Updated: {room.id}");
        
        Debug.Log("Current Players in Room:");
        foreach (var player in room.clients)
        {
            Debug.Log($"- Player ID: {player.id}");
        }
        
        currentRoom = room;
    }

    private void OnRoomJoined(NetworkRoom room)
    {
        Debug.Log($"Joined Room: {room.id}");
        currentRoom = room;
    }

    private void OnRoomCreated(NetworkRoom room)
    {
        Debug.Log($"Room Created: {room.id}");
        currentRoom = room;
    }

    private void OnConnected(int id){

        Debug.Log("Connected to Server");

        if(!NetworkConnection.Instance.Connected)
            LocalId = id;

        if(playerPrefab == null)
        {
            Debug.LogWarning("Player Prefab not assigned in NetworkManager");
            return;
        }
        GameObject player = Spawn(playerPrefab, Vector3.zero, Quaternion.identity);

        if (!NetworkConnection.Instance.Connected)
            player.GetComponent<NetworkBehaviour>().isLocalPlayer = true;

        Debug.Log("Player Values Initialized");
    }

    private void OnMessageReceived(string action,string message){
        Debug.Log($"(MESSAGE RECEIVED) Action: {action} | Message: {message}");
    }

    private void OnDisconnected(){
        Debug.Log("Disconnected from Server");
    }

    private void OnTick(){

    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation){

        GameObject newSpawn = Instantiate(prefab, position, rotation);

        Debug.Log("Spawning...");

        if (newSpawn.GetComponent<NetworkBehaviour>() == null){
            Debug.LogError("NetworkBehaviour not found in prefab");
            return null;
        }

        List<NetworkBehaviour> list = new List<NetworkBehaviour>();

        if(newSpawn.GetComponentsInChildren<NetworkBehaviour>() != null)
        {
            list.AddRange(newSpawn.GetComponentsInChildren<NetworkBehaviour>());
        }

        foreach(NetworkBehaviour behaviour in list)
        {
            if (behaviour != null)
            {
                // Wait 1 tick before calling OnSpawn
                DOVirtual.DelayedCall(1 / 60f, behaviour.OnSpawn);
            }
        }

        return newSpawn;
    }

    public void Despawn(GameObject obj){
        networkBehaviours.Remove(obj.GetComponent<NetworkBehaviour>());
        Destroy(obj);
    }
}
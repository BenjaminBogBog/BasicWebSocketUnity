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

    private void Start() {

        networkBehaviours = new List<NetworkBehaviour>();
        NetworkConnection.Instance.onConnected += OnConnected;
        NetworkConnection.Instance.onMessageReceived += OnMessageReceived;
        NetworkConnection.Instance.onDisconnected += OnDisconnected;
        NetworkConnection.Instance.onTick += OnTick;
        NetworkConnection.Instance.Connect();
    }

    private void OnConnected(int id){

        Debug.Log("Connected to Server");

        if(!NetworkConnection.Instance.Connected)
            LocalId = id;

        GameObject player = Spawn(playerPrefab, Vector3.zero, Quaternion.identity);

        if (!NetworkConnection.Instance.Connected)
            player.GetComponent<NetworkBehaviour>().isLocalPlayer = true;

        Debug.Log("Player Values Initialized");
    }

    private void OnMessageReceived(string action,string message){
        Debug.Log("Message Received : "+message);
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
                DOVirtual.DelayedCall(1 / 60, behaviour.OnSpawn);
            }
        }

        return newSpawn;
    }

    public void Despawn(GameObject obj){
        networkBehaviours.Remove(obj.GetComponent<NetworkBehaviour>());
        Destroy(obj);
    }
}
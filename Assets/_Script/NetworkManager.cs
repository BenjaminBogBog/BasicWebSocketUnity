using BogBog.Utility;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

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
        LocalId = id;
    }

    private void OnMessageReceived(string message){
        Debug.Log("Message Received : "+message);

        if (message.Equals("spawn")) 
        {
            try
            {
                Debug.Log("Spawning Player: " + playerPrefab.name);
                GameObject player = Spawn(playerPrefab, Vector3.zero, Quaternion.identity);
                player.GetComponent<NetworkBehaviour>().isLocalPlayer = true;
                player.GetComponent<NetworkBehaviour>().playerId = LocalId;
                Debug.Log("Spawn");
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
            
            
        }
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

        if(newSpawn.GetComponents<NetworkBehaviour>().Length > 0)
        {
            networkBehaviours.AddRange(newSpawn.GetComponents<NetworkBehaviour>());
        }

        if(newSpawn.GetComponentsInChildren<NetworkBehaviour>() != null)
        {
            networkBehaviours.AddRange(newSpawn.GetComponentsInChildren<NetworkBehaviour>());
        }

        return newSpawn;
    }

    public void Despawn(GameObject obj){
        networkBehaviours.Remove(obj.GetComponent<NetworkBehaviour>());
        Destroy(obj);
    }
}
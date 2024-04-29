using UnityEngine;

public class NetworkBehaviour : MonoBehaviour {
    
    public int playerId;
    public bool isLocalPlayer = false;
    public virtual void OnTick(){
        
    }
}
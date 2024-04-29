using UnityEngine;

public class NetworkTransform : NetworkBehaviour {

    public override void OnTick()
    {
        base.OnTick();

        // Send position to server
        if(isLocalPlayer){
            NetworkConnection.Instance.Send($"position: " + transform.position);
        }
    }
}
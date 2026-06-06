using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkConnection))]
public class EditorNetworkConnection : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkConnection myScript = (NetworkConnection)target;
        if (GUILayout.Button("Connect"))
        {
            myScript.Connect();
        }
        
        if (GUILayout.Button("Send Test Message"))
        {
            myScript.SendToAll("TEST MESSAGE");
        }

        if (GUILayout.Button("Create Room"))
        {
            myScript.CreateRoom();
        }
        
        if(GUILayout.Button("Join Room"))
        {
            Debug.Log("Trying to join room with ID: " + myScript.testRoomJoinId);
            myScript.JoinRoom(myScript.testRoomJoinId);
        }
        
        if(GUILayout.Button("Leave Room"))
        {
            myScript.LeaveRoom();
        }
    }
}
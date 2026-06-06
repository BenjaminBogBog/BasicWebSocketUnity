using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkConnection))]
public class EditorNetworkConnection : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkConnection myScript = (NetworkConnection)target;
        if (GUILayout.Button("Send Test Message"))
        {
            myScript.SendToAll("TEST MESSAGE");
        }
    }
}
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoChunk))]
 public class YourScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AutoChunk script = target as AutoChunk;
        if (GUILayout.Button("JoinChunks", GUILayout.Width(200)))
        {
            script.DeleteChunkPrefabs();
            script.CreateChunkPrefabs();
            script.JoinChunks();
        }
        DrawDefaultInspector();
    }
}
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeGraph))]
public class NodeGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NodeGraph nodeGraph= (NodeGraph)target;

        if (GUILayout.Button("Generate Nodes")) {
            nodeGraph.GenerateNodes();
        }
    }
}

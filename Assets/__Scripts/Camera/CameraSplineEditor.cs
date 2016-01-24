using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(CameraSpline))]
public class CameraSplineEditor : Editor {
    static int splineNodeIndex = 0;
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        CameraSpline csScript = (CameraSpline)target;
        
        GUILayout.BeginHorizontal("box");
        splineNodeIndex = EditorGUILayout.Popup(splineNodeIndex, csScript.GetNodeNameList());
        if (GUILayout.Button("Move Camera")) {
            Camera.main.transform.position = csScript.nodeList[splineNodeIndex].transform.position;
            Camera.main.transform.rotation = csScript.nodeList[splineNodeIndex].transform.rotation;
        }
        
        if (GUILayout.Button("Update Node")) {
            csScript.nodeList[splineNodeIndex].transform.position = Camera.main.transform.position;
            csScript.nodeList[splineNodeIndex].transform.rotation = Camera.main.transform.rotation;
        }
        GUILayout.EndHorizontal();
    }
}

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
            Vector3 newPos = csScript.nodeList[splineNodeIndex].transform.position;
            newPos.z -= csScript.nodeList[splineNodeIndex].followDistance;
            Camera.main.transform.position = newPos;
            Camera.main.transform.rotation = csScript.nodeList[splineNodeIndex].transform.rotation;
        }
        
        if (GUILayout.Button("Update Node")) {
            Vector3 newPos = Camera.main.transform.position;
            newPos.z += csScript.nodeList[splineNodeIndex].followDistance;
            csScript.nodeList[splineNodeIndex].transform.position = newPos;
            csScript.nodeList[splineNodeIndex].transform.rotation = Camera.main.transform.rotation;
        }
        GUILayout.EndHorizontal();
    }
}

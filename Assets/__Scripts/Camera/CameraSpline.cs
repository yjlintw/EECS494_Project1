using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSpline : MonoBehaviour {

    public CameraSplineNode[] nodeList;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public string[] GetNodeNameList() {
        List<string> nodeNames = new List<string>();
        for (int i = 0; i < nodeList.Length; i++) {
            CameraSplineNode node = nodeList[i];
            nodeNames.Add(node.gameObject.name);   
        }
        return nodeNames.ToArray();
    }
}

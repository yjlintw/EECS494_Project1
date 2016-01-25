using UnityEngine;
using System.Collections;

public class CSSplitNode : CameraSplineNode {
    public CameraSplineNode rightNode;
    public CameraSplineNode leftNode;
    
    public bool splitAfter = true;
	void Update () {
        CameraSplineNode tmpNode = rightNode;     
        if (Crash.S.transform.position.x > transform.position.x) {
            tmpNode = rightNode;
        } else {
            tmpNode = leftNode;
        }
        
        if (splitAfter) {
            nextNode = tmpNode;
        } else {
            previousNode = tmpNode;
        }
	}
}

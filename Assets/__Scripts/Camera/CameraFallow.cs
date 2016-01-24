using UnityEngine;
using System.Collections;

public class CameraFallow : MonoBehaviour {

    public float followDistance = 7.5f;
    public float backOffset = 5.0f;
    public float tweenSpeed = 5.0f;
    public CameraSplineNode currentNode;
    public float offset = 0;
    private bool farAngleFlag = false;
	// Update is called once per frame
    void Awake() {
        offset = followDistance;
    }
    
	void Update () {
        float iV = Input.GetAxis("Vertical");
        if (iV < 0) {
            farAngleFlag = true;        
        } else if (iV > 0) {
            farAngleFlag = false;
        }
        
        if (farAngleFlag) {
            offset += tweenSpeed * Time.deltaTime;
            if (offset >= followDistance + backOffset) {
                offset = followDistance + backOffset;
            }
        } else {
            offset -= tweenSpeed * Time.deltaTime;
            if (offset < followDistance) {
                offset = followDistance;
            }
        }
        
        // Vector3 pos = Camera.main.transform.position;
        // pos.z = Crash.S.transform.position.z - offset;
        // pos.y = Crash.S.transform.position.y + 6.0f;
        // pos.x = Crash.S.transform.position.x;
        // Camera.main.transform.position = pos;
        float sectorDistance = Mathf.Abs(currentNode.transform.position.z - currentNode.nextNode.transform.position.z);
        float moveDistance = Mathf.Abs(Crash.S.transform.position.z - currentNode.transform.position.z);
        float movePercentage = moveDistance / sectorDistance;
        
        Vector3 newPosition = Vector3.Lerp(currentNode.transform.position, currentNode.nextNode.transform.position, movePercentage);
        // Debug.Log(movePercentage);
        newPosition.z = Crash.S.transform.position.z - offset;
        Camera.main.transform.position = newPosition;
        
        Quaternion newRotation = Quaternion.Lerp(currentNode.transform.rotation, currentNode.nextNode.transform.rotation, movePercentage);
        Camera.main.transform.rotation = newRotation;
        
        // check next node
        if (Crash.S.transform.position.z > currentNode.nextNode.transform.position.z) {
            currentNode = currentNode.nextNode;
        } else if (Crash.S.transform.position.z < currentNode.transform.position.z) {
            currentNode = currentNode.previousNode;
        }
	}
}

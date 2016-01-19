using UnityEngine;
using System.Collections;

public class CameraFallow : MonoBehaviour {

    public float followDistance = 7.5f;
    public float backOffset = 5.0f;
    public float tweenSpeed = 5.0f;
    private float offset = 0;
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
        
        Vector3 pos = Camera.main.transform.position;
        pos.z = Crash.S.transform.position.z - offset;
        pos.y = Crash.S.transform.position.y + 6.0f;
        pos.x = Crash.S.transform.position.x;
        Camera.main.transform.position = pos;
       
       
	}
}

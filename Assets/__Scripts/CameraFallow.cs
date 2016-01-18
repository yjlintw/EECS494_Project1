using UnityEngine;
using System.Collections;

public class CameraFallow : MonoBehaviour {

    public float followDistance = 7.5f;
	// Update is called once per frame
	void Update () {
	   Vector3 pos = Camera.main.transform.position;
       pos.z = Crash.S.transform.position.z - followDistance;
       pos.y = Crash.S.transform.position.y + 6.0f;
       pos.x = Crash.S.transform.position.x;
       Camera.main.transform.position = pos;
	}
}

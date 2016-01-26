using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == Tags.CRASH) {
            Crash.S.KillCrash();
        }
    }
}

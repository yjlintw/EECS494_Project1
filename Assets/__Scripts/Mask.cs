using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour {
    bool consumed = false;
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH && !consumed) {
            consumed = true;
            Crash.S.IncrementMasks ();
			Destroy(this.gameObject);
		}
	}

}

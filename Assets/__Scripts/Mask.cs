using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH) {
			Destroy(this.gameObject);
			Crash.S.IncrementMasks ();
		}
	}

}

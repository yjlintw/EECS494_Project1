using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH) {
			Display.S.IncrementLives();
			Destroy(this.gameObject);
		}
	}
}

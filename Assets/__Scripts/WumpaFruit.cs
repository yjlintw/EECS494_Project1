using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == Tags.CRASH) {
            Display.S.IncrementFruit();
            Destroy(this.gameObject);
        }
    }
}

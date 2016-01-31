using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    protected BoxCollider boxCol;

	// Use this for initialization
	void Start () {
	   boxCol = gameObject.GetComponent<BoxCollider>();
	}
	void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == Tags.CRASH) {
            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .2f;
            if(Crash.S.falling && landed) {
                Crash.S.LandOnCrate();
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

    public GameObject item;
    public float bounceHeight = 7f;
    
    protected BoxCollider boxCol;

	// Use this for initialization
	void Start () {
	   boxCol = gameObject.GetComponent<BoxCollider>();
	}
	
    
    void OnCollisionEnter(Collision col) {
        Debug.Log("OnCollision");
        if (col.gameObject.tag == Tags.CRASH) {
            bool landed = Crash.S.collider.bounds.min.y <= boxCol.bounds.max.y + .01f;
            if(Crash.S.falling && landed) {
                if (Crash.S.jumping) {
                    BreakBox();
                    Crash.S.Bounce(bounceHeight);
                } else {
                    Crash.S.LandOnCrate();
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == Tags.SPIN) {
            if (Crash.S.spinning) {
                BreakBox();
                return;
            }
        }
    }
    
    void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == Tags.SPIN && Crash.S.spinning) {
            BreakBox();
        }
    }
    
    void OnCollisionStay(Collision col) {
        // if (col.gameObject.tag == Tags.CRASH && Crash.S.spinning) {
        //     BreakBox();
        // }
    }
    
    protected virtual void BreakBox() {
        Vector3 pos = transform.position;
        Destroy(this.gameObject);
        if (item != null) {
            Instantiate(item, pos, Quaternion.identity);
        }
    }
}

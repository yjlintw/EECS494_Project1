using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

    public GameObject item;
    public float bounceHeight = 7f;
    
    protected BoxCollider boxCol;
    bool broken = false;

	// Use this for initialization
	void Start () {
	    boxCol = gameObject.GetComponent<BoxCollider>();
	}
	
    
    void OnCollisionEnter(Collision col) {
        Debug.Log("OnCollision");
        if (col.gameObject.tag == Tags.CRASH) {
            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .2f;
            Vector3 relativeVec = transform.InverseTransformPoint(Crash.S.transform.position);
            Debug.Log("RelativeVec" + relativeVec);
            if(Crash.S.falling && landed && relativeVec.y > 0.5f) {
                if (Crash.S.jumping) {
                    BreakBox();
                    Crash.S.Bounce(bounceHeight);
                } else {
                    Crash.S.LandOnCrate();
                }
            }
        } else if (col.gameObject.tag == Tags.ENEMY) {
            BreakBox();
        }
    }
    
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == Tags.SPIN) {
            if (Crash.S.spinning) {
                BreakBox();
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
        if (broken) {
            return;
        }
        
        broken = true;
        Vector3 pos = transform.position;
        Destroy(this.gameObject);
        if (item != null) {
            Instantiate(item, pos, Quaternion.identity);
        }
    }
}

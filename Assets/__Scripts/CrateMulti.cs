using UnityEngine;
using System.Collections;

public class CrateMulti : Crate {
	public int 		fruitRemaining = 10;


	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH) {
			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;
			Vector3 relativeVec = transform.InverseTransformPoint(Crash.S.transform.position);
            if((Crash.S.falling && landed && relativeVec.y > 0.5f) || (Crash.S.jumping && relativeVec.y > 0.5f)) {
				Crash.S.Bounce (bounceHeight);
                // Debug.Log(fruitRemaining);
				if (fruitRemaining > 0) 
				{
					Vector3 pos = transform.position;
					pos += transform.up;
					Instantiate (item, pos, Quaternion.identity);
					fruitRemaining--;
				}
				else
				{
					BreakBox ();
				}
			}

		} else if (col.gameObject.tag == Tags.ENEMY) {
            BreakBox();
        }
	}
    
    protected override void BreakBox() {
        Destroy(this.gameObject);
    }
}

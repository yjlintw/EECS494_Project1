using UnityEngine;
using System.Collections;

public class CrateBounce : Crate{

	public int fruitRemaining = 10;

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH) {
			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;
			if (Crash.S.falling && landed) {
				Crash.S.Bounce (bounceHeight);
				if (fruitRemaining > 0) 
				{
					Vector3 pos = transform.position;
					pos.y += 1;
					Instantiate (item, pos, Quaternion.identity);
					fruitRemaining--;
				}


			}

		}
	}

}

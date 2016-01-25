using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ControlledCrates {
	public CrateOutline 	outline;
	public GameObject		crate;
}

public class CrateSwitch : Crate {
	public ControlledCrates[] controlledCrates;
	public Material activatedMat;

	bool activated = false;

	void OnCollisionEnter(Collision col) {
		
		
		if (col.gameObject.tag == Tags.CRASH) {
			if (Crash.S.spinning || Crash.S.invincible) {
				ActivateSwitch ();
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;
			if(Crash.S.falling && landed) {
				if (Crash.S.jumping) {
					ActivateSwitch ();
                }
 				Crash.S.LandOnCrate();

			}
		}
	}
	void OnCollisionStay(Collision col) {
		if (col.gameObject.tag == Tags.CRASH && Crash.S.spinning) {
			ActivateSwitch ();
		}
	}
	public void ActivateSwitch() {
        if(activated) return;
		activated = true;
		GetComponent<Renderer> ().material = activatedMat;
		foreach(ControlledCrates cratePair in controlledCrates) {
			CrateOutline outline = cratePair.outline;
			GameObject activatedCrate = cratePair.crate;
			activatedCrate.transform.position = outline.transform.position;
			Destroy (outline.gameObject);
			Instantiate (activatedCrate);

		}
	}


}

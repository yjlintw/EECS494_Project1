using UnityEngine;
using System.Collections;

public class CrateSwitch : Crate {

    public Material deactivatedMat;
	public Material activatedMat;
	public bool activated = false;
    public GameObject controlledObj;

	void OnCollisionEnter(Collision col) {
		if (activated)
			return;
		
		if (col.gameObject.tag == Tags.CRASH) {
			if (Crash.S.spinning) {
				ActivateSwitch ();
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y <= boxCol.bounds.max.y + .01f;
			if(Crash.S.falling && landed) {
				if (Crash.S.jumping) {
					ActivateSwitch ();
				} else {
					Crash.S.LandOnCrate();
				}
			}
		}
	}
	public void ActivateSwitch() {
		activated = true;
		GetComponent<Renderer> ().material = activatedMat;
        controlledObj.SetActive(activated);
	}
    
    public void DeactivateSwitch() {
        activated = false;
        GetComponent<Renderer>().material = deactivatedMat;
        controlledObj.SetActive(activated);
    }
}

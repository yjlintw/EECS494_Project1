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
    
    private List<GameObject> bridge = new List<GameObject>();
	public Material activatedMat;
    public Material deactivatedMat;

	bool activated = false;
    

	void OnCollisionEnter(Collision col) {
		
		
		if (col.gameObject.tag == Tags.CRASH) {
			if (Crash.S.spinning || Crash.S.invincible) {
				ActivateSwitch ();
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .2f;
            
            Debug.Log("Crash: " +  Crash.S.collider.bounds.min.y + ", Switch BOX: " + boxCol.bounds.max.y);

			if(Crash.S.falling && landed) {
				if (Crash.S.jumping) {
					ActivateSwitch ();
                }
                Crash.S.LandOnCrate();
			}
		}
	}
    
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == Tags.SPIN) {
			if (Crash.S.spinning || Crash.S.invincible) {
				ActivateSwitch ();
			}
        }
    }
    
    void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == Tags.SPIN && Crash.S.spinning) {
            ActivateSwitch ();
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
            activatedCrate.transform.rotation = outline.transform.rotation;
            activatedCrate.GetComponent<Rigidbody>().useGravity = false;
            outline.gameObject.SetActive(false);
			GameObject tmp = Instantiate<GameObject>(activatedCrate);
            bridge.Add(tmp);

		}
	}
    
    public void DeactivateSwitch() {
        if (!activated) {
            return;
        }
        activated = false;
        GetComponent<Renderer>().material = deactivatedMat;
        foreach(ControlledCrates cratePair in controlledCrates) {
			CrateOutline outline = cratePair.outline;
            outline.gameObject.SetActive(true);
		}
        
        foreach(GameObject crate in bridge) {
            Destroy(crate.gameObject);
        }
        
        bridge = new List<GameObject>();
    }
}

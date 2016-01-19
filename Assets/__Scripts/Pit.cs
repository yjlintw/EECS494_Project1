using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == Tags.CRASH) 
		{
			Crash.S.KillCrash ();
		}

	}
}

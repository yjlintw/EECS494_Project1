﻿using UnityEngine;
using System.Collections;

public class CrateMulti : Crate {
	public int 		fruitRemaining = 10;


	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == Tags.CRASH) {
			if (Crash.S.spinning || Crash.S.invincible) {
				BreakBox();
				return;
			}
			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;
			if (Crash.S.falling && landed) {
				Crash.S.Bounce (3f);
				if (fruitRemaining > 0) 
				{
					Vector3 pos = transform.position;
					pos.y += 1;
					Instantiate (item, pos, Quaternion.identity);
					fruitRemaining--;
				}
				else
				{
					BreakBox ();
				}
			}

		}
	}
}

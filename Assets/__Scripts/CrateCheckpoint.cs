﻿using UnityEngine;
using System.Collections;

public class CrateCheckpoint : Crate
{

	protected override void BreakBox ()
	{
		base.BreakBox ();
		// Vector3 checkpoint = new Vector3 (transform.position.x, 1, transform.position.z);
        Vector3 checkpoint = transform.position;
		Crash.S.checkpoint = checkpoint;
	}
}
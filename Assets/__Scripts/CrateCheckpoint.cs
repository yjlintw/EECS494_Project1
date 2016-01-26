using UnityEngine;

public class CrateCheckpoint : Crate
{

	protected override void BreakBox ()
	{
		base.BreakBox ();
		Vector3 checkpoint = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
		Crash.S.checkpoint = checkpoint;
	}
}

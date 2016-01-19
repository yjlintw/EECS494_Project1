using UnityEngine;
using System.Collections;

public class CrateMystery : Crate {

	public GameObject itemLikely;
	public GameObject itemUnlikely;


	protected override void BreakBox() {
		Vector3 pos = transform.position;
		Destroy(this.gameObject);
		if (item == null)
		{
			float roll = Random.value;
			if (roll < 0.1f) {
				item = itemUnlikely; 
			} else {
				item = itemLikely;
			}
		}
		Instantiate(item, pos, Quaternion.identity);
	}
}

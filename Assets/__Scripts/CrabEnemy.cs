using UnityEngine;
using System.Collections;

public class CrabEnemy : Enemy {

	
	// Update is called once per frame
	public override void Move() {
        if (launched) {
            if (Time.time - launchTime > launchDuration) {
                Destroy(this.gameObject);
            } else {
                rigid.velocity = Vector3.forward * launchSpeed;
            }
        } else {
            rigid.velocity = Vector3.right * speed;
        }
	}
    
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == Tags.WALL) {
            speed *= -1;
        } else if (col.gameObject.tag == Tags.CRASH) {
            
            if (Crash.S.spinning) {
                LaunchEnemy();
                return;
            }
            
            bool killEnemy = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - 0.1f;
            
            if (Crash.S.falling && killEnemy) {
                Destroy(this.gameObject);
                Crash.S.Bounce(3f);
            }
        }
    }
}

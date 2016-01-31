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
        if (launched && col.gameObject.tag == Tags.WALL && rigid.velocity.magnitude < 0.5f) {
            Destroy(this.gameObject);
            return;
        }
        
        if (col.gameObject.tag == Tags.WALL) {
            Debug.Log("Crab, collide wall");
            speed *= -1;
        } else if (col.gameObject.tag == Tags.CRASH) {
            if(Crash.S.invincible) {
                LaunchEnemy();
                return;
            } else if (Crash.S.hasMask()) {
                Crash.S.TakeHit();
                Destroy(this.gameObject);
                return;
            }
            
            Vector3 relativeVec = transform.InverseTransformPoint(Crash.S.collider.bounds.min);
            // Debug.Log(relativeVec);
            
			bool killEnemy = Crash.S.falling && relativeVec.y >= -0.5f;

            if (Crash.S.spinning) {
                LaunchEnemy();
                return;
            }
			else if (Crash.S.falling && killEnemy) {
				Destroy(this.gameObject);
				Crash.S.Bounce(3f);
			}
			else {
				Crash.S.TakeHit();
			}
        } else if (col.gameObject.tag == Tags.ENEMY) {
            Destroy(this.gameObject);
        }
    }
}

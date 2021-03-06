﻿using UnityEngine;
using System.Collections;

public class TurtleEnemy : Enemy {

    public GameObject[] patrolPoints;
    public float arrivedThreshold = 0.01f;
    public float UNFLIP_TIME = 1.0f;
    
    [Header("Dynamic Fields")]
    public int targetPointIndex = 0;
    public float debug_diff;
    public bool flipped = false;
    public bool turning = false;
    public float unflipTimer = 0;
	// Use this for initialization
	public override void Move () {
        if (launched) {
            if (Time.time - launchTime > launchDuration) {
                Destroy(this.gameObject.transform.parent.gameObject);
            } else {
                rigid.velocity = Vector3.forward * launchSpeed;
            }
        } else if (!flipped) {
            if (Arrived(targetPointIndex)) {
                targetPointIndex = (targetPointIndex + 1 ) % patrolPoints.Length;
                turning = true;
            } else if (turning){
                rigid.velocity = Vector3.zero;
                Vector3 target = patrolPoints[targetPointIndex].transform.position;
                Vector3 direction = target - transform.position;
                
                Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * 2, 1.0f);
                rigid.rotation = Quaternion.LookRotation(newDir);
                if (Vector3.Angle(newDir, direction) < 0.1f) {
                    turning = false;
                }
            } else {
                Vector3 target = patrolPoints[targetPointIndex].transform.position;
                
                Vector3 direction = target - transform.position;
                direction.Normalize();
                rigid.velocity = direction * speed;
                rigid.rotation = Quaternion.LookRotation(direction);
            }
        } else if (flipped) {
            rigid.velocity = Vector3.zero;
            if (Time.time - unflipTimer > UNFLIP_TIME) {
                flipped = false;
                Flip(flipped);
            }
        }
	    
	}
    
    private bool Arrived(int pointIndex) {
        Vector3 target = patrolPoints[pointIndex].transform.position;
        Vector3 diff = target - transform.position;
        debug_diff = diff.magnitude;
        
        if (diff.magnitude < arrivedThreshold) {
            return true;
        }
        return false;
    }
    
    void OnCollisionEnter(Collision col) {
        if (launched && col.gameObject.tag == Tags.WALL && rigid.velocity.magnitude < 0.5f) {
            Destroy(this.gameObject);
            return;
        }
        if (col.gameObject.tag == Tags.CRASH) {
            if(Crash.S.invincible) {
                LaunchEnemy();
                return;
            } else if (Crash.S.hasMask()) {
                Destroy(this.gameObject);
                Crash.S.TakeHit();
                return;
            }
            Vector3 relativeVec = transform.InverseTransformPoint(Crash.S.transform.position);
            
            bool jumpOnEnemy = Crash.S.falling && Mathf.Abs(relativeVec.y) > 0.5f;
            if (Crash.S.spinning) {
                LaunchEnemy();
                flipped = true;
                Flip(flipped);
                return;
            } else if (Crash.S.falling && jumpOnEnemy) {
                if (!flipped) {
                    flipped = true;
                    Flip(flipped);
                    unflipTimer = Time.time;
                    Crash.S.Bounce(3f);
                } else {
                    unflipTimer = Time.time;
                    Crash.S.Bounce(7f);
                }
                
            } else if (!flipped) {
                Crash.S.TakeHit();
            }
        }  else if (col.gameObject.tag == Tags.ENEMY) {
            Debug.Log("touch enemy: Turtle");
            Destroy(this.gameObject);
        }
    }
    
    public void Flip(bool flip) {
        Vector3 rotationAngle = rigid.rotation.eulerAngles;
        rotationAngle.z = flip?180:0;
        rigid.rotation = Quaternion.Euler(rotationAngle);
    }
}

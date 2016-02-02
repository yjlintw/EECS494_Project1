using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour {
    public float launchSpeed = 50f;
    public float launchDuration = 1.5f;
    bool consumed = false;
    bool launched = false;
    float launchTime = 0f;
    
    Rigidbody rigid;
    float INIT_DURATION = 0;
    
    float startTime = 0;
    void Start() {
        startTime = Time.time;
        rigid = GetComponent<Rigidbody>();
        // rigid.AddForce(transform.up * 1.0f, ForceMode.Force);
    }
    
    void SetInitDuration(float time) {
        INIT_DURATION = time;
    }
    void FixedUpdate () {
        if (launched) {
            if (Time.time - launchTime > launchDuration) {
                Destroy(this.gameObject);
            } else {
                rigid.velocity = Vector3.forward * launchSpeed;
            }
        }
    }
	void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag == Tags.CRASH && !consumed) {
                if(Crash.S.spinning)
                {
                    LaunchFruit();
                    return;
                }
                
                consumed = true;
                Display.S.IncrementFruit();
                Destroy(this.gameObject);
            }
      
    }
    
    void OnCollisionStay(Collision col) {
    // void OnTriggerStay(Collider col) {
        // if ((Time.time - startTime) > INIT_DURATION) {
            if (col.gameObject.tag == Tags.CRASH && !consumed) {
                 if(Crash.S.spinning)
                {
                    LaunchFruit();
                    return;
                }
                consumed = true;
                Display.S.IncrementFruit();
                Destroy(this.gameObject);
            }
        // }
    }
    
    public void LaunchFruit() {
        launched = true;
        launchTime = Time.time;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.velocity = Vector3.forward * launchSpeed;
    }
}

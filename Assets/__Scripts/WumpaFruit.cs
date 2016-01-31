using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour {
    bool consumed = false;
    float INIT_DURATION = 0;
    
    float startTime = 0;
    void Start() {
        startTime = Time.time;
        Rigidbody rigid = GetComponent<Rigidbody>();
        // rigid.AddForce(transform.up * 1.0f, ForceMode.Force);
    }
    
    void SetInitDuration(float time) {
        INIT_DURATION = time;
    }

	void OnCollisionEnter(Collision col) {
    // void OnTriggerEnter(Collider col) {
        // if ((Time.time - startTime) > INIT_DURATION) {
            if (col.gameObject.tag == Tags.CRASH && !consumed) {
                consumed = true;
                Display.S.IncrementFruit();
                Destroy(this.gameObject);
            }
        // }
    }
    
    void OnCollisionStay(Collision col) {
    // void OnTriggerStay(Collider col) {
        // if ((Time.time - startTime) > INIT_DURATION) {
            if (col.gameObject.tag == Tags.CRASH && !consumed) {
                consumed = true;
                Display.S.IncrementFruit();
                Destroy(this.gameObject);
            }
        // }
    }
}

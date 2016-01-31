using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

    protected BoxCollider boxCol;
    public ParticleSystem ps;
    
	// Use this for initialization
	void Start () {
	    boxCol = gameObject.GetComponent<BoxCollider>();
        ps.Stop();
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision col) {
        Debug.Log("OnCollision");
        if (col.gameObject.tag == Tags.CRASH) {
            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .2f;
            Vector3 relativeVec = transform.InverseTransformPoint(Crash.S.transform.position);
            if(landed && relativeVec.y > 0.5f) {
                ps.Play();
                Invoke("NextLevel", 2f);
            }
        }
    }
    
    void NextLevel() {
        SceneManager.LoadScene(1);
    }    
    
}

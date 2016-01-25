using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {
    public GameObject spawnPrefab;
    public bool useGravity = true;
    private GameObject spawnedGameObject;
	// Use this for initialization
	void Start () {
        spawnedGameObject = null;
	    Spawn();
	}
    
    public void Spawn() {
        if (spawnedGameObject == null) {
            spawnedGameObject = (GameObject)Instantiate(spawnPrefab, gameObject.transform.position, transform.rotation);
            spawnedGameObject.GetComponent<Rigidbody>().useGravity = useGravity;
        }
    }
}

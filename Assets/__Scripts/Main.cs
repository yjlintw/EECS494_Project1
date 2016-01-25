using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public GameObject pauseMenu;
	bool paused = false;

	void Start() {
		pauseMenu.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Return))
		{
			if (paused)
			{
				pauseMenu.SetActive (false);
				paused = false;
				Time.timeScale = 1;
			}
			else 
			{
				pauseMenu.SetActive (true);
				paused = true;
				Time.timeScale = 0;
			}

		}

	}
}

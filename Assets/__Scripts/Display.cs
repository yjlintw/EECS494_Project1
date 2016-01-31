using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Display : MonoBehaviour {

    public static Display S;
    public int      maxLives;
    public int      numFruit = 0;
    public int      numLives = 3;
    public Text     livesText;
    public Text     fruitText;
    public Image    livesIcon;
    public Image    fruitIcon;
    
    
    float livesTimer;
    float livesDuration = 2f;
    float fruitTimer;
    float fruitDuration = 2f;
    
    bool showLives = false;
    bool showFruit = false;
    
    void Awake() {
        S = this;
    }

	// Use this for initialization
	void Start () {
	   livesText = transform.FindChild("NumLives").GetComponent<Text>();
       fruitText = transform.FindChild("NumFruits").GetComponent<Text>();
       livesIcon = transform.FindChild("LivesIcon").GetComponent<Image>();
       fruitIcon = transform.FindChild("FruitIcon").GetComponent<Image>();
	}
    
    void Update() {
        if (showFruit) {
            fruitText.gameObject.SetActive(true);
            fruitIcon.gameObject.SetActive(true);
        } else {
            fruitText.gameObject.SetActive(false);
            fruitIcon.gameObject.SetActive(false);
        }
        
        if (showFruit && (Time.time - fruitTimer) > fruitDuration) {
            showFruit = false;
        }
        
        if (showLives) {
            livesText.gameObject.SetActive(true);
            livesIcon.gameObject.SetActive(true);
        } else {
            livesText.gameObject.SetActive(false);
            livesIcon.gameObject.SetActive(false);
        }
        
        if (showLives && (Time.time - livesTimer) > livesDuration) {
            showLives = false;
        }
        
    }
	
    public void IncrementLives() {
        if (numLives != maxLives) {
            ++numLives;
            livesText.text = numLives.ToString();
            showLives = true;
            livesTimer = Time.time;
        }
    }
    
    public void DecrementLives() {
        if (numLives != 0) {
            --numLives;
            livesText.text = numLives.ToString();
            showLives = true;
            livesTimer = Time.time;
        } else {
            // GameOver
            showLives = true;
            livesTimer = Time.time;
        }
    }
    
    public void IncrementFruit() {
        if (numFruit != 99) {
            ++numFruit;
            fruitText.text = numFruit.ToString();
        } else {
            numFruit = 0;
            fruitText.text = "0";
            IncrementLives();
        }
        showFruit = true;
        fruitTimer = Time.time;
    }
}

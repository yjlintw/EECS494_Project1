using UnityEngine;
using System.Collections;

public class Crash : MonoBehaviour {
    
    public float        speed = 10f;
    public float        jumpVel = 3f;
    public float        spinDuration;
    public float        spinPauseDuration = 1f;
    public float        spinSpeed;
    
	public int 			maskCount = 0;
	public int			maskMax = 3;
	public float 		invincibleStartTime;
	public float		invincibleDuration = 10;
	public bool 		invincible = false;
	public bool			alive = true;
    public bool         grounded = true;
    public bool         jumping = false;
    public bool         falling = false;
    public bool         spinning = false;
    public BoxCollider  collider;
	public Vector3		checkpoint;
    
    public GameObject foot;
    public GameObject body;
    public GameObject spawningObjects;
    public CrateSwitch[] switchObjects;


    float       iH, iV;
    Vector3     vel;
    float       distToGround;
    float       groundedOffest;
    Rigidbody   rigid;
    int         groundLayerMask;
    float       spinStartTime;
    float       spinEndTime;
    bool        canSpin = true;
    
    private float jumpTimer = 0;
    public float JUMP_TIME = 2f;
    
    public static Crash S;
    
	public Material[] materials;
	public Color[]	originalColors;
    private bool jumpRelease = false;
    private bool jumpKeyDown = false;
    public GameObject akuAkuMask;
    public Quaternion currentRotation;
    Vector3 currentUp = Vector3.up;
    
    void Awake() {
        S = this;
		checkpoint = transform.position;
        currentRotation = transform.rotation;
    }
	// Use this for initialization
	void Start () {
		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for(int i = 0; i < materials.Length; i++){
			originalColors [i] = materials [i].color;
		}
	  	rigid = gameObject.GetComponent<Rigidbody>();
 		collider = gameObject.GetComponent<BoxCollider>();
       
     	distToGround = collider.bounds.extents.y + 0.1f;
        groundedOffest = collider.size.x / 1.5f;
       
        groundLayerMask = LayerMask.GetMask(Layers.GROUND);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        akuAkuMask.transform.position = transform.position + new Vector3(1, 2, 0);
	   	// Get movement input
        if (hasMask()) {
            akuAkuMask.SetActive(true);
        } else {
            akuAkuMask.SetActive(false);
        }
        if(!alive) return;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -currentUp, out hit, distToGround, groundLayerMask)){
            currentUp = hit.normal;
            if (Mathf.Abs(Vector3.Dot(currentUp, Vector3.up)) < 0.1f) {
                currentUp = Vector3.up;
            }
        }
        
      	iH = Input.GetAxis("Horizontal");
       	iV = Input.GetAxis("Vertical");
       	float jump = Input.GetAxis("Jump");
       	float spin = Input.GetAxis("Fire1");
           
       
        if (iH != 0 || iV != 0) {
            foot.SetActive(false);
            body.SetActive(true);
        } else {
            foot.SetActive(true);
            body.SetActive(false);
        }
      
       	if (canSpin && !spinning && spin > 0) {
           spinning = true;
           spinStartTime = Time.time;
           currentRotation = transform.rotation;
       	}
       
       	if (spinning && Time.time - spinStartTime > spinDuration) {
           spinning = false;
           canSpin = false;
           spinEndTime = Time.time;
           transform.rotation = currentRotation;
           
       	}
           
        if (!canSpin && Time.time - spinEndTime > spinPauseDuration && spin <= 0) {
            canSpin = true;
        }
       
      
       	// Set the x and z values of new velocity
       	
        
        if (spinning || (jumping && !falling)) {
            vel = rigid.velocity;
        } else {
            vel = Vector3.zero;
       	    vel.z += iV * speed;
       	
            vel = (transform.forward * Mathf.Abs(iV)) * speed;
            vel.x += iH * speed;
        }
        
       
       	if (spinning) {
            
           	transform.Rotate(transform.up, spinSpeed * Time.fixedTime);

       	} else if (GetArrowInput() && vel != Vector3.zero) {
            Vector3 cameraAngle = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            Vector3 globalVel = Vector3.zero;
            globalVel.z += iV * speed;
            globalVel.x += iH * speed;
        	transform.rotation = Quaternion.LookRotation(globalVel, currentUp);
            transform.Rotate(cameraAngle);
       	} else {
            transform.rotation = Quaternion.LookRotation(transform.forward, currentUp);       
        } 
           
        Debug.DrawRay(transform.position, currentUp, Color.yellow);
        Debug.DrawRay(transform.position, transform.up, Color.blue);


        
        grounded = (grounded && !jumping) || OnGround();
        falling = rigid.velocity.y < 0.1f && !grounded;
        
        if (jumping && jump <= 0) {
            jumpRelease = true;
        }

        if (jump > 0 && grounded && !jumpKeyDown) {
            vel.y = jumpVel;
            jumping = true;
            jumpTimer = Time.time;
        } else {
            if (grounded) {
                jumping = false;
                jumpRelease = false;
                vel.y = rigid.velocity.y;
            } else if (jump > 0 && (Time.time - jumpTimer) < JUMP_TIME && !jumpRelease) {
                vel.y = jumpVel;
                Debug.Log("Jump: " + jump.ToString());
            } else {
                vel.y = rigid.velocity.y;
            }
        }
        
        if (jump <= 0) {
            jumpKeyDown = false;
        } else {
            jumpKeyDown = true;
        }
       
        // Apply our new Velocity
        rigid.velocity = vel;
	}
    
    public bool hasMask() {
        return maskCount > 0;
    }
    
    public void LandOnCrate() {
        grounded = true;
        jumping = false;
        
    }
    
    public void Bounce(float bounceVel) {
        Vector3 vel = rigid.velocity;
        vel.y = bounceVel;
        rigid.velocity = vel;
    }

	public void TakeHit() {
		if (!alive)
			return;
		if (maskCount == 0)
		{
			KillCrash();
			return;
		}
		LoseMask();
	}

	public void LoseMask() {
		maskCount--;
	}
	public void KillCrash() {
		if (!alive)
			return;
		alive = false;
        maskCount = 0;
		transform.rotation = Quaternion.Euler(-90,0,0);
		Display.S.DecrementLives ();
		Invoke("Respawn", 2f);
	}
	public void Respawn() {
		alive = true;
		transform.position = checkpoint;
		rigid.freezeRotation = true;
		transform.rotation = Quaternion.identity;
        Respawn[] respawns = spawningObjects.GetComponentsInChildren<Respawn>();
        for (int i = 0; i < respawns.Length; i++) {
            respawns[i].Spawn();
        }
        for (int i = 0; i < switchObjects.Length; i++) {
            switchObjects[i].DeactivateSwitch();
        }
        
	}

	public void IncrementMasks() {
		maskCount++;
		if (maskCount == maskMax) {
			BeginAkuAku ();	
		}
		else {
			// Add/update floating mask around Crash 
		}
	}
	public void BeginAkuAku() {
		// Become invincible and put mask on Crash
		invincible = true;
		invincibleStartTime = Time.time;
		Invoke ("ShowInvincibleFlash", 0.25f);
        // akuAkuMask.SetActive(true);
	}
	public void EndAkuAku() {
		invincible = false;
		maskCount = 2;
        // akuAkuMask.SetActive(false);
	}
	void ShowInvincibleFlash() {
		foreach(Material m in materials) {
			m.color = Color.red;
		}

		Invoke ("HideInvincibleFlash", 0.25f);
	}
	void HideInvincibleFlash() {
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
		if(Time.time - invincibleStartTime > invincibleDuration)
		{
			EndAkuAku ();
		} else
		{
			Invoke ("ShowInvincibleFlash", 0.25f);
		}
	}

    bool OnGround() {
        return Physics.Raycast(transform.position, -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * -transform.right, -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * transform.right, -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * transform.forward, -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * -transform.forward, -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (-transform.forward - transform.right), -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (-transform.forward + transform.right), -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (transform.forward - transform.right), -transform.up, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (transform.forward + transform.right), -transform.up, distToGround, groundLayerMask);
    }
    
    //Returns true if an arrow key is being pressed
    bool GetArrowInput() {
        return Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow);        
    }
    
    
}

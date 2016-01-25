using UnityEngine;
using System.Collections;

public class Crash : MonoBehaviour {
    
    public float        speed = 10f;
    public float        jumpVel = 3f;
    public float        spinDuration;
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


    float       iH, iV;
    Vector3     vel;
    float       distToGround;
    float       groundedOffest;
    Rigidbody   rigid;
    int         groundLayerMask;
    float       spinStartTime;
    
    private float jumpTimer = 0;
    public float JUMP_TIME = 2f;
    
    public static Crash S;
    
	public Material[] materials;
	public Color[]	originalColors;
    public GameObject akuAkuMask;
    
    void Awake() {
        S = this;
		checkpoint = new Vector3(0,1,-10);
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
       
     	distToGround = collider.bounds.extents.y;
        groundedOffest = collider.size.x / 2f;
       
        groundLayerMask = LayerMask.GetMask(Layers.GROUND);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!alive) return;
	   	// Get movement input
      	iH = Input.GetAxis("Horizontal");
       	iV = Input.GetAxis("Vertical");
       	float jump = Input.GetAxis("Jump");
       	float spin = Input.GetAxis("Fire1");
       
      
       	if (!spinning && spin > 0) {
           spinning = true;
           spinStartTime = Time.time;
       	}
       
       	if (Time.time - spinStartTime > spinDuration) {
           spinning = false;
       	}
       
      
       	// Set the x and z values of new velocity
       	vel = Vector3.zero;
       	vel.z += iV * speed;
       	vel.x += iH * speed;
       
       	if (spinning) {
           	transform.Rotate(Vector3.up, spinSpeed * Time.fixedTime);

       	} else if (GetArrowInput() && vel != Vector3.zero) {
        	transform.rotation = Quaternion.LookRotation(vel);
       	} 
       


       falling = rigid.velocity.y < 0;
       grounded = (grounded && !jumping) || OnGround();
       

       if (jump > 0 && grounded) {
           vel.y = jumpVel;
           jumping = true;
           jumpTimer = Time.time;
       } else {
           if (grounded) {
               jumping = false;
           } else if (jump > 0 && (Time.time - jumpTimer) < JUMP_TIME) {
               vel.y = jumpVel;
               Debug.Log("Jump: " + jump.ToString());
           } else {
               vel.y = rigid.velocity.y;
           }
       }
       
       // Apply our new Velocity
       rigid.velocity = vel;
	

	
	}
    
    public void LandOnCrate() {
        grounded = true;
        
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
		transform.rotation = Quaternion.Euler(-90,0,0);
		Display.S.DecrementLives ();
		Invoke("Respawn", 2f);
	}
	public void Respawn() {
		alive = true;
		transform.position = checkpoint;
		rigid.freezeRotation = true;
		transform.rotation = Quaternion.identity;
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
        akuAkuMask.SetActive(true);
	}
	public void EndAkuAku() {
		invincible = false;
		maskCount = 2;
        akuAkuMask.SetActive(false);
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
        return Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * Vector3.left, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * Vector3.right, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * Vector3.forward, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * Vector3.back, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (Vector3.back + Vector3.left), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (Vector3.back + Vector3.right), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (Vector3.forward + Vector3.left), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffest * (Vector3.forward + Vector3.right), Vector3.down, distToGround, groundLayerMask);
    }
    
    //Returns true if an arrow key is being pressed
    bool GetArrowInput() {
        return Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow);        
    }
    
    
}

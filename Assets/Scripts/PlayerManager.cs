﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : Damageable {
	
    public float jumpForce = 10f;
    public float airModifier = 5f;
    public float restitutionScale = 1.1f;
    public float AttackLength = 10f;
    public float AttackStrength = 10f;

	private int facing = 1;

	public GameObject knifePrefab;
	private float elapsedTime;

    private bool hasDoubleJumped = false;
	private bool isInAir = false;
	public bool hasDoubleJumpPowerup = false;
	public bool hasDashPowerup = false;
	public int health;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
    }
    
    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

	void Update(){


        if(Input.GetKeyDown(KeyCode.Z)) {
            Debug.Log("Trying to attack");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies) {
                Vector3 diff = enemy.transform.position - transform.position;
                Debug.Log("Found an enemy @ " + diff);
                if (diff.magnitude < AttackLength) {
                    ((Rigidbody)enemy.GetComponent<Rigidbody>()).AddForce(diff.normalized * AttackStrength);
                    ((Damageable)enemy.GetComponent<Damageable>()).Damage(10);
                }
            }
        }
		elapsedTime += Time.deltaTime;
		if (Input.GetKey (KeyCode.F)) {
			if (elapsedTime > 0.1) {
				elapsedTime = 0;
				GameObject knife = (GameObject)Instantiate (knifePrefab, transform.position + new Vector3 (1, 1, 0), transform.rotation);
				knife.GetComponent<KnifeManager>().facing = facing;
				knife.GetComponent<KnifeManager>().setFacing(facing);
				Physics.IgnoreCollision (knife.GetComponent<Collider> (), GetComponent<Collider> ());
				knife.GetComponent<Rigidbody> ().AddForce (Vector3.right * 750 * facing);
			}
		}



		if (IsGrounded ()) {
			hasDoubleJumped = false;
			isInAir = false;
			if (Input.GetKeyDown (KeyCode.Space)) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (0f, jumpForce, 0f));
				isInAir = true;
				Debug.Log ("Jumping");
			}
		} else {
			if ((hasDoubleJumped == false && hasDoubleJumpPowerup == true)) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					
					rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);;

					GetComponent<Rigidbody> ().AddForce (new Vector3 (0f, jumpForce, 0f));
					hasDoubleJumped = true;
					Debug.Log ("Double jumping");
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.X) && IsGrounded() && hasDashPowerup == true) {
			speed = 110f;
			Invoke ("reduceSpeed", 0.15f);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Horizontal") > 0) {
			facing = 1;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			facing = -1;
		}
		Vector3 toTranslate = new Vector3 (Input.GetAxis ("Horizontal") * speed * Time.deltaTime, 0f, 0f);
            //GetComponent<Rigidbody>().transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f));
        
            //Vector3 force = new Vector3(Input.GetAxis("Horizontal")*speed * airModifier * Time.deltaTime, 0, 0);
            //if(force.x < 0 && GetComponent<Rigidbody>().velocity.x > 0 || force.x > 0 && GetComponent<Rigidbody>().velocity.x < 0) {
            //    force.x *= restitutionScale;
            //}
            //GetComponent<Rigidbody>().AddForce(force);

			GetComponent<Rigidbody> ().transform.Translate (toTranslate);

	}

	void reduceSpeed(){
		speed = 10f;	
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log("Collided with "+other.gameObject.name);
		if (other.gameObject.tag == "Lose") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		} else if (other.gameObject.tag == "Enemy") {
			//I take damage here
			Damage (7);
		} else if (other.gameObject.tag == "DoubleJump") {
			hasDoubleJumpPowerup = true;
			SpecialEffectsHelper.Instance.PowerUp (other.gameObject.transform.position);
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Dash") {
			hasDashPowerup = true;
			SpecialEffectsHelper.Instance.PowerUp (other.gameObject.transform.position);
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Level2") {
			SceneManager.LoadScene ("Level 2");
		}
		else if (other.gameObject.tag == "Level3") {
			SceneManager.LoadScene ("Level 3");
		}
		else if (other.gameObject.tag == "Level4") {
			SceneManager.LoadScene ("Level 4");
		}
		else if (other.gameObject.tag == "Level5") {
			SceneManager.LoadScene ("Final Level");
		}
	}

	void OnCollisionEnter(Collision other) {
		//Debug.Log("touched with "+other.gameObject.name);
		if (other.gameObject.tag == "JumpEnemy") {
			//Debug.Log ("adding force");
			//other.rigidbody.AddForce (new Vector3(0f, 500f, 0f));
		}else if (other.gameObject.tag == "DoubleJump") {
			hasDoubleJumpPowerup = true;
		}
	}

    protected override void OnDeath() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

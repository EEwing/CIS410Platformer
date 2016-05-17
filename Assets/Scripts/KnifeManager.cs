﻿using UnityEngine;
using System.Collections;

public class KnifeManager : MonoBehaviour {
	private int i;
	public int facing;
	// Use this for initialization
	void Start () {
		i = 0;
	}
	public void setFacing(int i){
		facing = i;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		i++;
		if (i > 300) {
			Destroy (gameObject);
		}
		transform.Rotate (new Vector3 (0, 0, -20 * facing));
	}

	void OnCollisionEnter(Collision other) {
		Destroy (gameObject);
	}
}

﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {
	public bool isStart;
	public bool isQuit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnMouseUp(){
		if (isStart) {
			SceneManager.LoadScene ("Level 1");
		}
		if (isQuit) {
			Application.Quit ();
		}
	}
}

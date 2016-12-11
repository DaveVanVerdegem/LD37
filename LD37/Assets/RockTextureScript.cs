using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTextureScript : MonoBehaviour {
	
	public Texture test;
	
	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTexture = test;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
}

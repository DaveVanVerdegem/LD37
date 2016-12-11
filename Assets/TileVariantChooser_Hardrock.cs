using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVariantChooser_Hardrock : MonoBehaviour {

	public Texture TileVariant_Hardrock;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void setTileGraphics () {
		GetComponent<Renderer> ().material.mainTexture = TileVariant_Hardrock;
	}

}
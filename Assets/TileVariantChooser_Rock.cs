using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVariantChooser_Rock : MonoBehaviour {

	public Texture TileVariant_Rock;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void setTileGraphics () {
		GetComponent<Renderer> ().material.mainTexture = TileVariant_Rock;
	}

}

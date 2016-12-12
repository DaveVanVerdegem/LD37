using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileVariantChooser_Rock : MonoBehaviour {

	public Sprite Tile_CornerBigRed;
	public Sprite Tile_CornerSmallRed;
	public Sprite Tile_OuterWall;
	public Sprite Tile_Red;
	public Sprite Tile_CornerYellow;
	public Sprite Tile_CrossDoubleRed;
	public Sprite Tile_CrossOneRed;
	public Sprite Tile_CrossYellow;
	public Sprite Tile_SingleSquare;
	public Sprite Tile_InnerWall;
	public Sprite Tile_Stump;
	public Sprite Tile_TShape;
	public Sprite Tile_TShapeRed;
	public Sprite Tile_TShapeSmallRed;
	public Sprite Tile_TShapeSmallRed_Flipped;

	private Sprite sprit;
	private float _rotationDegrees = 0.0f;

	private List<KeyValuePair<char[],Sprite>> RefArray = new List<KeyValuePair<char[],Sprite>>();

	//private List<string,Texture> TileVariants;

	// Use this for initialization
	void Start () {

		Initialize();
	}

	void Initialize()
	{
		addElement(new char[] { '1', '0', '1', '0', '0', '0', '0', '0' }, Tile_TShapeRed);
		addElement(new char[] { '1', '0', '1', '0', '1', '0', '1', '0' }, Tile_CrossYellow);
		addElement(new char[] { '1', '0', '1', '0', '0', '0', '1', '0' }, Tile_CrossOneRed);

		addElement(new char[] { '1', '0', '1', '0', '1', '1', 'X', '1' }, Tile_CornerYellow);

		addElement(new char[] { '0', '0', '1', '0', '1', '1', '0', '0' }, Tile_TShapeSmallRed);
		addElement(new char[] { '0', '0', '0', '1', '1', '0', '1', '0' }, Tile_TShapeSmallRed_Flipped);

		addElement(new char[] { '0', '0', '0', '0', '1', '0', '0', '0' }, Tile_CornerBigRed);
		addElement(new char[] { 'X', '1', 'X', '1', 'X', '1', 'X', '1' }, Tile_SingleSquare);
		addElement(new char[] { 'X', '1', 'X', '1', 'X', '0', 'X', '1' }, Tile_Stump);
		addElement(new char[] { '0', '0', '0', '0', 'X', '1', 'X', '0' }, Tile_OuterWall);
		addElement(new char[] { 'X', '0', 'X', '1', 'X', '0', 'X', '1' }, Tile_InnerWall);

		addElement(new char[] { '0', '0', 'X', '1', 'X', '1', 'X', '0' }, Tile_CornerSmallRed);

		addElement(new char[] { '1', '0', '1', '0', 'X', '1', 'X', '0' }, Tile_TShape);
	}

	public void addElement(char[] charArray, Sprite wantedSprite){
		RefArray.Add(new KeyValuePair<char[],Sprite> (charArray, wantedSprite));
	}

	// Update is called once per frame
	void Update () {

	}

	public void setTileGraphics (bool[,] TileHood) {

		Initialize();

		_rotationDegrees = 0.0f;

		char[] TileHoodArray = getHoodString(TileHood);

		compareHood (TileHoodArray);

		if (sprit == Tile_OuterWall) {
			Debug.Log (_rotationDegrees);
		}

		GetComponent< SpriteRenderer >().sprite= sprit;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, _rotationDegrees);
		//(0.0f, 0.0f, _rotationDegrees);
		//GetComponent<Transform> ().Rotate (new Vector3(0.0f,0.0f,_rotationDegrees));
	}

	public void compareHood(char[] TileHoodArray){
		int matchStrength = 0;

		for (int ii = 0; ii < RefArray.Count; ii++) {
			for (int jj = 0; jj < 8; jj++) {

				if (TileHoodArray [jj] == RefArray [ii].Key [jj] || RefArray [ii].Key [jj] == 'X') {
					matchStrength++;
				}
			}
			if (matchStrength == 8) {

				sprit = RefArray [ii].Value;
				return;

			} else {
				matchStrength = 0;
			}
		}
			
		_rotationDegrees -= 90.0f;

		if (_rotationDegrees > -360.0f) {
			compareHood (shiftHoodString (TileHoodArray));
		} else {
			//Debug.Log ("nothing was found!");
			sprit = Tile_Red;
			return;
		}
	}

	public char[] getHoodString(bool[,] TileHood) {
		char[] TileHoodArray = { '1', '1', '1', '1', '1', '1', '1', '1' };
		int index = 0;

		for (int xx = 0; xx <= 2; xx++) {
			for (int yy = 0; yy <= 2; yy++) {
				if (!(xx == 1 & yy== 1)) {
					if (!TileHood [xx, yy]) {
						switch (xx) {
						case 0:
							index = yy;
							break;
						case 1:
							switch (yy) {
							case 0:
								index = 7;
								break;
							default: //case 2:
								index = 3;
								break;
							}
							break;
						default: //case 2:
							switch (yy) {
							case 0:
								index = 6;
								break;
							case 1:
								index= 5;
								break;
							default: //case 2:
								index = 4;
								break;
							}
							break;
						}
						TileHoodArray[index]='0';
					}
				}
			}
		}

		//Debug.Log (TileHood[0,0]);
		//Debug.Log (new string (TileHoodArray));

		return TileHoodArray;
	
	}

	public char[] shiftHoodString(char[] TileHoodArray) {

		char[] shiftedArray = shiftRight (TileHoodArray); 
		shiftedArray = shiftRight (shiftedArray); 

		return shiftedArray;
		}

	public char[] shiftRight(char[] arr) 
	{
		char[] demo = new char[arr.Length];

		for (int i = 1; i < arr.Length; i++) 
		{
			demo[i] = arr[i - 1];
		}

		demo[0] = arr[demo.Length - 1];

		return demo;
	}

}

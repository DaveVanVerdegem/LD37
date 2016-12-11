using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileVariantChooser_Rock : MonoBehaviour {

	public Texture TileVariant_CornerBigRed;
	public Texture TileVariant_CornerSmallRed;
	public Texture TileVariant_OuterWall;
	public Texture TileVariant_Red;
	public Texture TileVariant_CornerYellow;
	public Texture TileVariant_CrossDoubleRed;
	public Texture TileVariant_CrossOneRed;
	public Texture TileVariant_CrossYellow;
	public Texture TileVariant_SingleSquare;
	public Texture TileVariant_InnerWall;
	public Texture TileVariant_Stump;
	public Texture TileVariant_TShape;
	public Texture TileVariant_TShapeRed;

	//private List<string,Texture> TileVariants;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void setTileGraphics (bool[,] TileHood) {	
		Texture tex;
		float _rotationDegrees = 0.0f;

		int numberOfDiggedDiagonalNeighbours = 0;
		int numberOfDiggedNeighbours = 0;
		List<Vector2> directionOfDiggedNeighbours = new List<Vector2>();

		for (int xx=0;xx<=2;xx++){
			for (int yy=0;yy<=2;yy++){
				if (TileHood[xx,yy]){
					numberOfDiggedNeighbours++;
					directionOfDiggedNeighbours.Add(new Vector2(xx,yy));
					if ((xx + yy)%2  == 0) {
						numberOfDiggedDiagonalNeighbours++;
					}
				}
			}
		}

		//Debug.Log (numberOfDiggedDiagonalNeighbours);
		switch(numberOfDiggedNeighbours){
		case 0:
			tex=TileVariant_Red;
			break;
		//Only one digged tile...
		case 1:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 0: //There's one straight digged neighbour!
				tex=TileVariant_OuterWall;
				break;
			default: //There's one diagonal digged neighbour!
				tex = TileVariant_CornerBigRed;
				break;
			}
			break;
		case 6:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 2: //There's two diagonal digged neighbour!
				tex=TileVariant_CrossDoubleRed;
				break;
			case 3: //There's three diagonal digged neighbour!
				tex=TileVariant_Stump;
				break;
			default: //There's four diagonal digged neighbour!
				tex=TileVariant_InnerWall;
				break;
			}
			break;
		//Its a tile only connected with one side!
		case 7: 
			tex = TileVariant_Stump;
			break;
		//Its a tile surrounded by digged areas!
		case 8:
			tex = TileVariant_SingleSquare;
			break;
		default:
			tex=TileVariant_Red;
			break;

		}
			
		GetComponent<Renderer>().material.SetFloat ("_RotationDegrees", _rotationDegrees);

		GetComponent<Renderer> ().material.mainTexture = tex;
	}

}

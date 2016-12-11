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


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void setTileGraphics (bool[,] TileHood) {	
		Texture tex;
		float _rotationDegrees;
		_rotationDegrees = 0.0f;

		switch (getEquivalentKey(TileHood))
		{
		case 1: //done
			tex = TileVariant_CornerBigRed;
			_rotationDegrees = 90.0f;
			break;
		case 10: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 90.0f;
			break;
		case 100: //done
			tex = TileVariant_CornerBigRed;
			_rotationDegrees = 180.0f;
			break;
		case 1000: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 0.0f;
			break;
		case 100000: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 180.0f;
			break;
		case 1000000: //done
			tex =  TileVariant_CornerBigRed;
			_rotationDegrees = 0.0f;
			break;
		case 10000000: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 270.0f;
			break;
		case 100000000: //done
			tex = TileVariant_CornerBigRed;
			_rotationDegrees = 270.0f;
			break;
		case 100100: case 100100000: case 100100100: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 180.0f;
			break;
		case 1001: case 1001000: case 1001001: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 0.0f;
			break;
		case 11: case 110: case 111: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 90.0f;
			break;
		case 110000000: case 11000000: case 111000000: //done
			tex = TileVariant_OuterWall;
			_rotationDegrees = 270.0f;
			break;
		case 1011: case 1111: case 1001011: case 1001111: //done
			tex = TileVariant_CornerSmallRed;
			_rotationDegrees = 270.0f;
			break;
		case 11001000: case 111001000: case 11001001: case 111001001: //done
			tex = TileVariant_CornerSmallRed;
			_rotationDegrees = 180.0f;
			break;
		case 100110: case 100111: case 100100111: case 100100110: //done
			tex = TileVariant_CornerSmallRed;
			_rotationDegrees = 0.0f;
			break;
		case 110100100: case 111100100: case 110100000: //done
			tex = TileVariant_CornerSmallRed;
			_rotationDegrees = 90.0f;
			break;
		case 11001011: case 111001011: case 111001111: case 11001111: //done
			tex = TileVariant_Stump;
			_rotationDegrees = 270.0f;
			break;
		case 1000001: //done
			tex = TileVariant_TShapeRed;
			_rotationDegrees = 180.0f;
			break;
		case 11000001: //done
			tex = TileVariant_TShape;
			_rotationDegrees = 270.0f;
			break;
		case 11000011: //done
			tex = TileVariant_InnerWall;
			_rotationDegrees = 90.0f;
			break;
		case 111101111: //done
			tex = TileVariant_SingleSquare;
			_rotationDegrees = 0.0f;
			break;
		default:
			tex=TileVariant_Red;
			Debug.Log (getEquivalentKey (TileHood));
			break;
		}
			
		GetComponent<Renderer>().material.SetFloat ("_RotationDegrees", _rotationDegrees);

		GetComponent<Renderer> ().material.mainTexture = tex;
	}

	public int getEquivalentKey(bool[,] TileHood){

		int HoodKey=0;
				for (int xx=0;xx<=2;xx++){
					for (int yy=0;yy<=2;yy++){
				if (TileHood[xx,yy]){
					HoodKey += (int)Math.Pow(10,3*xx+yy);
					//Debug.Log (Math.Pow(10,3*xx+yy));
				}
			}
		}
		return HoodKey;
	}	

}

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
		int numberOfDiggedStraightNeighbours;
		int numberOfDiggedNeighbours = 0;

		int numberOfRockyDiagonalNeighbours;
		int numberOfRockyStraightNeighbours;
		int numberOfRockyNeighbours;

		List<Vector2> directionOfDiggedNeighbours = new List<Vector2>();
		List<Vector2> directionOfRockyNeighbours = new List<Vector2>();

		List<Vector2> directionOfRockyDiagonalNeighbours = new List<Vector2>();
		List<Vector2> directionOfRockyStraightNeighbours = new List<Vector2>();

		List<Vector2> directionOfDiggedDiagonalNeighbours = new List<Vector2>();
		List<Vector2> directionOfDiggedStraightNeighbours = new List<Vector2>();

		for (int xx=0;xx<=2;xx++){
			for (int yy=0;yy<=2;yy++){
				if (TileHood [xx, yy]) {
					numberOfDiggedNeighbours++;
					directionOfDiggedNeighbours.Add (new Vector2 (xx, yy));
					if ((xx + yy) % 2 == 0) {
						numberOfDiggedDiagonalNeighbours++;
						directionOfDiggedDiagonalNeighbours.Add (new Vector2 (xx, yy));
					} else {
						directionOfDiggedStraightNeighbours.Add (new Vector2 (xx, yy));
					}
				}
				else if(!(xx==1 & yy==1)) {
					directionOfRockyNeighbours.Add(new Vector2(xx,yy));
					if ((xx + yy) % 2 == 0) {
						directionOfRockyDiagonalNeighbours.Add (new Vector2 (xx, yy));
					} else {
						directionOfRockyStraightNeighbours.Add (new Vector2 (xx, yy));
					}
					}
				}
			}

		numberOfDiggedStraightNeighbours=numberOfDiggedNeighbours-numberOfDiggedDiagonalNeighbours;

		numberOfRockyDiagonalNeighbours=4-numberOfDiggedDiagonalNeighbours;
		numberOfRockyStraightNeighbours=4-numberOfDiggedStraightNeighbours;
		numberOfRockyNeighbours=8-numberOfDiggedNeighbours;

		//Debug.Log (numberOfDiggedDiagonalNeighbours);
		switch(numberOfDiggedNeighbours){
		case 0:
			tex=TileVariant_Red;
			break;
		//Only one digged tile...
		case 1:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 0: //There's one straight digged neighbour!
				tex = TileVariant_OuterWall;
				switch ((int) directionOfDiggedNeighbours [0].x) {
				case 1:
					switch ((int) directionOfDiggedNeighbours [0].y) {
					case 0:
						_rotationDegrees = 180.0f;
						break;
					default:
						_rotationDegrees = 0.0f;
						break;
					}
					break;
				case 0:
					_rotationDegrees = 270.0f;
					break;
				default:
					_rotationDegrees = 90.0f;
					break;
				}
				break;
			default: //There's only one diagonal digged neighbour!
				tex = TileVariant_CornerBigRed;
				switch ((int) directionOfDiggedNeighbours [0].x) {
				case 0:
					switch ((int) directionOfDiggedNeighbours [0].y) {
					case 0:
						_rotationDegrees = 180.0f;
						break;
					default:
						_rotationDegrees = 270.0f;
						break;
					}
					break;
				default:
					switch ((int) directionOfDiggedNeighbours [0].y) {
					case 0:
						_rotationDegrees = 90.0f;
						break;
					default:
						_rotationDegrees = 0.0f;
						break;
					}
					break;
				}
				break;
			}
			break;
		//Only two digged tiles...
		case 2:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 0:
				if (directionOfDiggedStraightNeighbours[0].y==directionOfDiggedStraightNeighbours[1].y | directionOfDiggedStraightNeighbours[0].x==directionOfDiggedStraightNeighbours[1].x){
					tex = TileVariant_InnerWall;
					if (directionOfDiggedStraightNeighbours [0].y == 1) {
						_rotationDegrees = 90.0f;
					} 
				} else {
					//STILL WORK TO DO HERE!
					tex = TileVariant_CornerSmallRed;
				}
				break;
			case 1:
				if (directionOfDiggedDiagonalNeighbours [0].y == directionOfDiggedStraightNeighbours [0].y | directionOfDiggedDiagonalNeighbours [0].x == directionOfDiggedStraightNeighbours [0].x) {
					if (directionOfDiggedStraightNeighbours [0].x == 1) {
						if (directionOfDiggedStraightNeighbours [0].y == 0) {
							_rotationDegrees = 180.0f;
						} else {
							_rotationDegrees = 0.0f;
						}
					} else {
						if (directionOfDiggedStraightNeighbours [0].x == 0) {
							_rotationDegrees = 270.0f;
						} else {
							_rotationDegrees = 90.0f;
						}
					}
					tex = TileVariant_OuterWall;
				} else {
					//STILL WORK TO DO HERE!
					tex = TileVariant_CornerSmallRed;
				}
				break;
			default:
				if (directionOfDiggedDiagonalNeighbours [0].y == directionOfDiggedDiagonalNeighbours [1].y | directionOfDiggedDiagonalNeighbours [0].x == directionOfDiggedDiagonalNeighbours [1].x) {
					tex = TileVariant_TShapeRed;
					if ((directionOfDiggedDiagonalNeighbours [0].x==2 & directionOfDiggedDiagonalNeighbours [0].y==2) | (directionOfDiggedDiagonalNeighbours [1].x==2 & directionOfDiggedDiagonalNeighbours [1].y==2)) {
						if ((directionOfDiggedDiagonalNeighbours [0].x == 0 & directionOfDiggedDiagonalNeighbours [0].y == 2) | (directionOfDiggedDiagonalNeighbours [1].x == 0 & directionOfDiggedDiagonalNeighbours [1].y == 2)) {
							_rotationDegrees = 0.0f;
						} else {
							_rotationDegrees = 90.0f;
						}
					} else {
						if ((directionOfDiggedDiagonalNeighbours [0].x == 0 & directionOfDiggedDiagonalNeighbours [0].y == 2) | (directionOfDiggedDiagonalNeighbours [1].x == 0 & directionOfDiggedDiagonalNeighbours [1].y == 2)) {
							_rotationDegrees = 270.0f;
						} else {
							_rotationDegrees = 180.0f;
						}
					}
				} else {
					tex = TileVariant_CrossDoubleRed;
					if (directionOfDiggedDiagonalNeighbours [0].x!=directionOfDiggedDiagonalNeighbours [0].y) {
						_rotationDegrees = 90.0f;
					}
						
				}
				break;
			}
			break;
		//Only fourrr digged tiles...
		case 3:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 0:
				tex = TileVariant_Stump;
				if (directionOfRockyStraightNeighbours [0].x == 1) {
					if (directionOfRockyStraightNeighbours [0].y == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				} else {
					if (directionOfRockyStraightNeighbours [0].x == 0) {
						_rotationDegrees = 180.0f;
					} else {
						_rotationDegrees = 0.0f;
					}
				}
				break;
			case 1:
				tex = TileVariant_CornerSmallRed;
				break;
			case 2:
				if (directionOfDiggedDiagonalNeighbours [0].y == directionOfDiggedDiagonalNeighbours [1].y) {
					if (directionOfDiggedStraightNeighbours [0].y==directionOfDiggedDiagonalNeighbours [0].y ){
						tex = TileVariant_OuterWall;
						if (directionOfDiggedStraightNeighbours [0].y == 0) {
							_rotationDegrees = 180.0f;
						} else {
							_rotationDegrees = 0.0f;
						}
					} else {
						tex = TileVariant_TShape;
						if (directionOfDiggedStraightNeighbours [0].y == 0) {
							_rotationDegrees = 0.0f;
						} else {
							_rotationDegrees = 180.0f;
						}
					}
				} else if(directionOfDiggedDiagonalNeighbours [0].x == directionOfDiggedDiagonalNeighbours [1].x) {
					if (directionOfDiggedStraightNeighbours [0].x == directionOfDiggedDiagonalNeighbours [0].x){
						tex = TileVariant_OuterWall;
						if (directionOfDiggedStraightNeighbours [0].x == 0) {
							_rotationDegrees = 270.0f;
						} else {
							_rotationDegrees = 90.0f;
						}
					}
					else {
						tex = TileVariant_TShape;
						if (directionOfDiggedStraightNeighbours [0].x == 0) {
							_rotationDegrees = 90.0f;
						} else {
							_rotationDegrees = 270.0f;
						}
					}

				} else {
					//STILL WORK TO DO HERE!
					tex = TileVariant_CornerSmallRed;
				}
				break;
			default:
				tex = TileVariant_CrossOneRed;
				if (directionOfRockyDiagonalNeighbours [0].x == 0) {
					if (directionOfRockyDiagonalNeighbours  [0].y == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 180.0f;
					}
				} else {
					if (directionOfRockyDiagonalNeighbours  [0].y == 0) {
						_rotationDegrees = 0.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				}
				break;
			}
			break;
		//Only fourrr digged tiles...
		case 4:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 0:
				tex = TileVariant_SingleSquare;
				break;
			case 1:
				tex = TileVariant_Stump;
				if (directionOfRockyStraightNeighbours [0].x == 1) {
					if (directionOfRockyStraightNeighbours [0].y == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				} else {
					if (directionOfRockyStraightNeighbours [0].x == 0) {
						_rotationDegrees = 180.0f;
					} else {
						_rotationDegrees = 0.0f;
					}
				}
				break;
			case 2:
				if (directionOfRockyStraightNeighbours[0].y==directionOfRockyStraightNeighbours[1].y | directionOfRockyStraightNeighbours[0].x==directionOfRockyStraightNeighbours[1].x){
					tex = TileVariant_InnerWall;
					if (directionOfRockyStraightNeighbours[0].x==directionOfRockyStraightNeighbours[1].x) {
						_rotationDegrees = 90.0f;
					}
				} else {
					//STILL WORK TO DO HERE!
					//tex = TileVariant_CornerYellow;
					tex = TileVariant_CornerSmallRed;
				}
				break;
			case 3:
				//STILL WORK TO DO HERE!
				tex = TileVariant_CrossYellow;
				break;
			default:
				tex = TileVariant_CrossYellow;
				break;
			}
			break;
		//Only five digged tiles...
		case 5:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 1:
				tex = TileVariant_SingleSquare;
				break;
			case 2:
				tex = TileVariant_Stump;
				if (directionOfRockyStraightNeighbours [0].x == 1) {
					if (directionOfRockyStraightNeighbours [0].y == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				} else {
					if (directionOfRockyStraightNeighbours [0].x == 0) {
						_rotationDegrees = 180.0f;
					} else {
						_rotationDegrees = 0.0f;
					}
				}
				break;
			case 3:
				if (directionOfRockyStraightNeighbours[0].y==directionOfRockyStraightNeighbours[1].y | directionOfRockyStraightNeighbours[0].x==directionOfRockyStraightNeighbours[1].x){
					tex = TileVariant_InnerWall;
					if (directionOfRockyStraightNeighbours[0].x==directionOfRockyStraightNeighbours[1].x) {
						_rotationDegrees = 90.0f;
					}
				} else {
					tex = TileVariant_CornerSmallRed;
					if (directionOfRockyDiagonalNeighbours [0].x == 0) {
						if (directionOfRockyDiagonalNeighbours [0].y == 0) {
							_rotationDegrees = 90.0f;
						} else {
							_rotationDegrees = 180.0f;
						}
					} else {
						if (directionOfRockyDiagonalNeighbours [0].y == 0) {
							_rotationDegrees = 0.0f;
						} else {
							_rotationDegrees = 270.0f;
						}
					}
				}
				break;
			default:
				tex = TileVariant_TShape;
				if (directionOfDiggedStraightNeighbours [0].x == 1) {
					if (directionOfDiggedStraightNeighbours [0].y == 0) {
						_rotationDegrees = 0.0f;
					} else {
						_rotationDegrees = 180.0f;
					}
				} else {
					if (directionOfDiggedStraightNeighbours [0].x == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				}
				break;
			}
			break;
		//Only SIX digged tilesssss...
		case 6:
			switch (numberOfDiggedDiagonalNeighbours) {
			case 2: //There's two diagonal digged neighbour!
				tex=TileVariant_SingleSquare;
				break;
			case 3: //There's three diagonal digged neighbour! (& 3 straight digged neighbours!)
				tex = TileVariant_Stump;
				if (directionOfRockyStraightNeighbours [0].x == 1) {
					if (directionOfRockyStraightNeighbours [0].y == 0) {
						_rotationDegrees = 90.0f;
					} else {
						_rotationDegrees = 270.0f;
					}
				} else {
					if (directionOfRockyStraightNeighbours [0].x == 0) {
						_rotationDegrees = 180.0f;
					} else {
						_rotationDegrees = 0.0f;
					}
				}
				break;
			default: //There's four diagonal digged neighbour!
				if (directionOfRockyNeighbours[0].y==directionOfRockyNeighbours[1].y | directionOfRockyNeighbours[0].x==directionOfRockyNeighbours[1].x){
					tex = TileVariant_InnerWall;
					if (directionOfRockyNeighbours [0].x == directionOfRockyNeighbours [1].x) {
						_rotationDegrees = 90.0f;
					}
				} else {
					tex=TileVariant_CornerYellow;
					if (directionOfRockyNeighbours [0].x == 0 | directionOfRockyNeighbours [1].x == 0) {
						if (directionOfRockyNeighbours [0].y == 0 | directionOfRockyNeighbours [1].y == 0) {
							_rotationDegrees = 90.0f;
						} else {
							_rotationDegrees = 180.0f;
						}
					} else {
						if (directionOfRockyNeighbours [0].y == 0 | directionOfRockyNeighbours [1].y == 0) {
							_rotationDegrees = 0.0f;
						} else {
							_rotationDegrees = 270.0f;
						}	
					}
				}
				break;
			}
			break;
		//Its a tile only connected with one side!
		case 7: 
			switch (numberOfDiggedDiagonalNeighbours) {
			case 4:
				tex = TileVariant_Stump;
				switch ((int) directionOfRockyNeighbours [0].x) {
				case 1:
					switch ((int) directionOfRockyNeighbours [0].y) {
					case 0:
						_rotationDegrees = 90.0f;
						break;
					default:
						_rotationDegrees = 270.0f;
						break;
					}
					break;
				case 0:
					_rotationDegrees = 180.0f;
					break;
				default:
					_rotationDegrees = 0.0f;
					break;
				}
				break;
			default:
				tex = TileVariant_SingleSquare;
				break;
			}
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

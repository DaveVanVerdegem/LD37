using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {
    [SerializeField] int gridWidth  = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float tileDistance = 1;
    [SerializeField] float hardrockChance = 0.2f;
    [SerializeField] bool hardrockBypresets = false;

    [SerializeField] GameObject tilePrefab = null;
    [SerializeField] TextAsset gridPreset;

	[Tooltip("This is the tile list.")]
	[SerializeField]
    private Tile[,] _tileList;



    void Start() {
        RedrawGrid();
    }

    public void RedrawGrid() {
		if (!tilePrefab)
		{
			Debug.LogWarning("No tile prefab found!", this);
			return;
		}

		_tileList = new Tile[gridWidth, gridHeight];

		// Remove the existing tiles.
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				if (_tileList[x, y] != null)
					Destroy(_tileList[x, y]);
			}
		}
		
        //preset fields
        bool preset = gridPreset==null?false:true;

        string[,] presetmap = null;
        if(preset)
		{
            string[] lines = gridPreset.text.Split(',');

            for(int y = 0; y<lines.Length;y++)
			{
                string[] tiles = lines[y].Split(']');

                for(int x = 0; x < tiles.Length-1; x++)
				{
                    if(presetmap == null)
						presetmap = new string[tiles.Length, lines.Length];

                    presetmap[x,lines.Length-1-y] = ""+tiles[x][tiles[x].Length==2?1:3];
                }
            }
        }


        ///generation
		for (int x = 0; x < gridWidth; x++)
		{
            for(int y = 0; y < gridHeight; y++)
			{
                Tile tile = Instantiate(tilePrefab).GetComponent<Tile>();
                tile.transform.localPosition = new Vector3(x*tileDistance - (gridWidth-1)*tileDistance/2, y*tileDistance - (gridHeight-1)*tileDistance/2,0);
                tile.name = ""+x+","+y;
                tile.transform.parent = transform;
				_tileList[x, y] = tile;
                
				if(!preset)
					tile.Initialize(Random.Range(0.0f,1.01f)<hardrockChance?Tile.state.Hardrock:Tile.state.Rock);
				else
				{
                    switch(presetmap[x,y])
					{
                        case "x":
							tile.Initialize(Tile.state.Dug);
							break;

                        case ".":
							if (!hardrockBypresets)
								tile.Initialize(Random.Range(0.0f,1.01f)<hardrockChance?Tile.state.Hardrock:Tile.state.Rock);
							else
								tile.Initialize(Tile.state.Rock);
							break;

                        case "!":
							if (!hardrockBypresets)
								tile.Initialize(Random.Range(0.0f,1.01f)<hardrockChance?Tile.state.Hardrock:Tile.state.Rock);
							else
								tile.Initialize(Tile.state.Hardrock);
							break;

                        case "v":
							tile.Initialize(Tile.state.Entrance);
							break;

                        case "^":
							tile.Initialize(Tile.state.Exit);
							break;

                        case "c":
							Debug.LogWarning("NYI. Type: "+presetmap[x,y]);tile.Initialize(Tile.state.Dug);
							break; //tile.Initialize(Tile.state. chest???);   break;

                        //default: Debug.LogWarning("Couldn't read preset file. Type: "+presetmap[x,y]); break;
                    }
                }
            }
        }
		
		for (int x = 0; x < gridWidth; x++)
		{
            for(int y = 0; y < gridHeight; y++)
			{
				_tileList[x, y].setNeighbouringTiles(getNeighbouringTiles(x,y));
			}
        }
    }

	
	List<KeyValuePair<int[], Tile>> getNeighbouringTiles(int gridX,int gridY){
		
		List<KeyValuePair<int[], Tile>> NeighbouringTiles = new List<KeyValuePair<int[], Tile>>();

		for(int x = -1; x <= 1; x ++){
			if (gridX + x >= 0 && gridX + x <= gridWidth - 1) {
				for (int y = -1; y <= 1; y ++) {
					if (gridY + y >= 0 && gridY + y <= gridHeight - 1 && !(y==0 && x==0)) {
						NeighbouringTiles.Add (new KeyValuePair<int[], Tile> (new int[]{x,y}, _tileList [gridX + x, gridY + y]));  
					}  
				}
			}
		}
		return NeighbouringTiles;
	}
}

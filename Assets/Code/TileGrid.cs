using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {
    [SerializeField] int gridWidth  = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float tileDistance = 1;
    [SerializeField] float hardrockChance = 0.2f;

    [SerializeField] GameObject tilePrefab;

	[Tooltip("This is the tile list.")]
	[SerializeField]
    private Tile[,] _tileList;



    void Start() {
        RedrawGrid();
    }

    public void RedrawGrid() {
		if (!tilePrefab)
		{
			Debug.LogWarning("No tile prefab found!");
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
		



		for (int x = 0; x < gridWidth; x++)
		{
            for(int y = 0; y < gridHeight; y++)
			{
                GameObject tile = Instantiate<GameObject>(tilePrefab);
                tile.transform.localPosition = new Vector3(x*tileDistance - (gridWidth-1)*tileDistance/2, y*tileDistance - (gridHeight-1)*tileDistance/2,0);
                tile.name = ""+x+","+y;
                tile.transform.parent = transform;
				_tileList[x, y] = tile.GetComponent<Tile>();

				tile.GetComponent<Tile>().Initialize(Random.Range(0.0f,1.01f)<hardrockChance?Tile.state.Hardrock:Tile.state.Rock);
            }
        }
    }
}

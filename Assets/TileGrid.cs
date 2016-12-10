using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {
    [SerializeField] int gridWidth  = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float tileDistance = 1;
    [SerializeField] int hardrockChance = 20;

    [SerializeField] GameObject tilePrefab;


    List<GameObject> tileList;



    void Start() {
        RedrawGrid();
    }

    public void RedrawGrid() {
        if(tileList!=null) {
            foreach(GameObject tile in tileList) {
                Destroy(tile);
            }
        }

        if(!tilePrefab) return;

        tileList = new List<GameObject>();

        for(int x = 0; x < gridWidth; x++) {
            for(int y = 0; y < gridHeight; y++) {
                GameObject tile = Instantiate<GameObject>(tilePrefab);
                tile.transform.localPosition = new Vector3(x*tileDistance - (gridWidth-1)*tileDistance/2, y*tileDistance - (gridHeight-1)*tileDistance/2,0);
                tile.name = ""+x+","+y;
                tile.transform.parent = transform;
                tileList.Add(tile);
                tile.GetComponent<Tile>().Initialize(Random.Range(0, 100)>hardrockChance?Tile.state.Hardrock:Tile.state.Rock);
            }
        }
    }
}

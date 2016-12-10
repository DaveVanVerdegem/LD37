using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	#region Properties

	#endregion

	#region Fields
	/// <summary>
	/// Parent transform for the grid.
	/// </summary>
	private static Transform _gridParent;

	private static Tile[,] _grid;
	#endregion

	#region Life Cycle
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	#region Methods
	/// <summary>
	/// Creates the grid.
	/// </summary>
	/// <param name="dimensions">Dimensions for the grid.</param>
	/// <param name="tilePrefab">Prefab to use for the tiles.</param>
	public static void CreateGrid(Vector2 dimensions, GameObject tilePrefab)
	{
		if(_gridParent != null)
		{
			Debug.LogWarning("Grid already created.");
			return;
		}

		_gridParent = new GameObject("Grid").transform;
		_grid = new Tile[(int)dimensions.x, (int)dimensions.y];

		//Vector2 startPosition = new Vector2(-Mathf.FloorToInt(dimensions.x/2), -Mathf.FloorToInt(dimensions.y / 2));

		for(int i = 0; i < dimensions.y; i++)
		{
			for(int j = 0; j < dimensions.x; j++)
			{
				// Create tile.
				GameObject newTile = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity, _gridParent);
				newTile.name = i + "-" + j;

				_grid[i, j] = newTile.GetComponent<Tile>();
			}
		}
	}
	#endregion
}

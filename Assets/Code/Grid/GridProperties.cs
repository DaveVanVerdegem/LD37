using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProperties : ScriptableObject
{
	[Header("Grid")]
	public Vector2 Size = new Vector2(30, 20);

	[Header("Prefabs")]
	[Tooltip("Prefab object for tiles.")]
	/// <summary>
	/// Prefab object for tiles.
	/// </summary>
	public Tile TilePrefab;
}

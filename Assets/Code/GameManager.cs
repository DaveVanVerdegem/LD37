using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Prefab to use for tiles.")]
	/// <summary>
	/// Prefab to use for tiles.
	/// </summary>
	private GameObject _tilePrefab;

	[SerializeField]
	[Tooltip("Size for the grid to create.")]
	/// <summary>
	/// Size for the grid to create.
	/// </summary>
	private Vector2 _gridSize;
	#endregion

	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		Grid.CreateGrid(_gridSize, _tilePrefab);
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion
}

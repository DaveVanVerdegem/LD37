using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Prefab to spawn chest.")]
	/// <summary>
	/// Prefab to spawn chest.
	/// </summary>
	private Chest _chestPrefab;
	#endregion

	#region Properties
	/// <summary>
	/// Ingame chest object.
	/// </summary>
	public static Chest Chest;
	#endregion

	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		// Spawn chest.
		if(Chest == null)
		{
			Chest = Instantiate(_chestPrefab);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	#region Methods

	#endregion
}

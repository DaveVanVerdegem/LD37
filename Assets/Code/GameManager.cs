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

	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	#region Methods
	/// <summary>
	/// Replaces the current chest with the new one.
	/// </summary>
	/// <param name="newChest">New chest to place.</param>
	public static void ReplaceChest(Chest newChest)
	{
		if(Chest != null)
		{
			Destroy(Chest.gameObject);
		}

		Chest = newChest;
	}
	#endregion
}

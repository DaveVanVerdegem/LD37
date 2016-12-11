using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized chest.");

		GameManager.ReplaceChest(this);

		base.Initialize(parentTile);
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : InteractableObject
{
	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized loot.");

		base.Initialize(parentTile);
	}
	#endregion
}

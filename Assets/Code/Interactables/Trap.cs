using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : InteractableObject
{
	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized trap.");

		base.Initialize(parentTile);
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion


	#region Interaction
	protected override void Interact()
	{
		Debug.Log("Activated trap.");
	}
	#endregion
}

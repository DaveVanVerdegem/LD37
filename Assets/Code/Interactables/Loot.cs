using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : InteractableObject
{
	#region Fields
	/// <summary>
	/// Amount of gold that this piece of loot holds.
	/// </summary>
	private int _gold = 0;
	#endregion


	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized loot.");
        GetComponent<Collider2D>().isTrigger = true;
        base.Initialize(parentTile);
	}
	#endregion

	#region Interaction
	protected override void Interact()
	{
		Debug.Log("Activated loot.");

		GameManager.AddGold(_gold);

		Destroy(gameObject);
	}
	#endregion

	#region Methods
	/// <summary>
	/// Set the amount of gold this loot holds.
	/// </summary>
	/// <param name="goldAmount">Amount of gold to set.</param>
	public void SetGold(int goldAmount)
	{
		_gold = goldAmount;
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[SelectionBase]
public class Tile : MonoBehaviour {
    
    [SerializeField] GameObject stateRockPrefab;
    [SerializeField] GameObject stateHardrockPrefab;
    [SerializeField] GameObject stateDugPrefab;
    [SerializeField] GameObject stateExitPrefab;
    [SerializeField] GameObject stateEntrancePrefab;

	private List<KeyValuePair<string, Tile>> NeighbouringTiles = new List<KeyValuePair<string, Tile>>();

	GameObject stateVisual;

    public enum state { Rock, Hardrock, Dug, Exit, Entrance}
    [SerializeField] state tileState = state.Rock;

	#region Properties
	//[HideInInspector]
	/// <summary>
	/// Returns true if the tile is a solid and isn't walkable.
	/// </summary>
	public bool Solid = true;
	#endregion

	#region Fields
	/// <summary>
	/// Interactable object attached to this tile.
	/// </summary>
	private InteractableObject _attachedObject;
	#endregion


	#region Life Cycle
	public void Initialize(state spawnState)
	{
		tileState = spawnState;
		switch (tileState)
		{
			case state.Rock:
				stateVisual = Instantiate(stateRockPrefab, transform.position, transform.rotation, transform);
				break;
			case state.Hardrock:
				stateVisual = Instantiate(stateHardrockPrefab, transform.position, transform.rotation, transform);
				break;
			case state.Dug:
				stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform);
				break;
		}
	}

	void OnMouseDown()
	{
		if (UIStateManager.State != UIState.Play)
			return;

		//DIG
		if (tileState == state.Rock && UIStateManager.state == "Dig")
		{
			tileState = state.Dug;
			Destroy(stateVisual);
			stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform);

			Solid = false;
		}
		//FILL
		if (tileState == state.Dug && UIStateManager.state == "Fill")
		{
			tileState = state.Rock;
			Destroy(stateVisual);
			stateVisual = Instantiate(stateRockPrefab, transform.position, transform.rotation, transform);

			Solid = true;
		}
		//ENTRANCE
		if (tileState == state.Rock && UIStateManager.state == "Place Entrance")
		{
			//TODO check if adjacent is dug
			tileState = state.Entrance;
			Destroy(stateVisual);
			stateVisual = Instantiate(stateEntrancePrefab, transform.position, transform.rotation, transform);
		}
		//EXIT
		if (tileState == state.Rock && UIStateManager.state == "Place Exit")
		{
			//TODO check if adjacent is dug
			tileState = state.Exit;
			Destroy(stateVisual);
			stateVisual = Instantiate(stateExitPrefab, transform.position, transform.rotation, transform);
		}
	}
	#endregion

	#region Methods
	public void setNeighbouringTiles(List<KeyValuePair<string, Tile>> NeighbouringTiles)
	{
		this.NeighbouringTiles = NeighbouringTiles;

	}


	public bool AttachObject(InteractableObject objectToAttach)
	{
		if (_attachedObject != null)
		{
			Debug.LogWarning("Already an object attached!");
			return false;
		}

		objectToAttach.transform.SetParent(transform);
		_attachedObject = objectToAttach;

		return true;
	}
	#endregion
}

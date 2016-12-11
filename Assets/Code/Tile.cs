using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[SelectionBase]
public class Tile : MonoBehaviour {
    
    [SerializeField] GameObject stateRockPrefab;
    [SerializeField] GameObject stateHardrockPrefab;
    [SerializeField] GameObject stateDugPrefab;
    [SerializeField] GameObject stateExitPrefab;
    [SerializeField] GameObject stateEntrancePrefab;

	private List<KeyValuePair<int[], Tile>> NeighbouringTiles = new List<KeyValuePair<int[], Tile>>();

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
			//notify the neighbours that something has changed!
			notifyNeighbours();

			Destroy(stateVisual);
			stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform);

			Solid = false;
		}
		//FILL
		if (tileState == state.Dug && UIStateManager.state == "Fill")
		{
			tileState = state.Rock;
			//notify the neighbours that something has changed!
			notifyNeighbours();

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
	public void setNeighbouringTiles(List<KeyValuePair<int[], Tile>> NeighbouringTiles)
	{
		this.NeighbouringTiles = NeighbouringTiles;

	}

	//Notify all neighbours of this tile that they should update their neighbours
	public void notifyNeighbours()
	{
		for(int i=0;i<=NeighbouringTiles.Count-1;i++){
			NeighbouringTiles[i].Value.updateTileGraphics(NeighbouringTiles[i].Key);
		}
	}

	//update the graphics
	public void updateTileGraphics(int[] Key)
	{
		if (tileState == state.Rock) {
			stateVisual.GetComponentInChildren<TileVariantChooser_Rock> ().setTileGraphics (getHood());
		} else if (tileState == state.Hardrock)  {
			stateVisual.GetComponentInChildren<TileVariantChooser_Hardrock> ().setTileGraphics (getHood());
		}
	}

	//Check the neighbourhood and return a list of booleans
	public bool[,] getHood(){
		bool[,] TileHood= new bool[3,3]{{false,false,false},{false,false,false},{false,false,false}};
		bool StatusBit;
		int[] Index = new int[2];

		for(int i=0;i<=NeighbouringTiles.Count-1;i++){
			switch (NeighbouringTiles[i].Value.tileState)
			{
			case state.Hardrock: case state.Rock:
				StatusBit = false;
				break;
			case state.Dug:
				StatusBit= true;
				break;
			default:
				StatusBit = true;
				break;
			}

			Index[0]=NeighbouringTiles [i].Key [0]+1;
			Index[1]=Mathf.Abs(NeighbouringTiles [i].Key [1]-1);

			TileHood[Index[0],Index[1]] = StatusBit;
		}
		Debug.Log (TileHood);

		return TileHood;
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

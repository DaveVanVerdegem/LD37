﻿using System.Collections;
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
	private state _tileState = state.Rock;

	#region Properties
	[HideInInspector]
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
		SetState(spawnState);
	}

	void OnMouseDown()
	{
		if (UIStateManager.State != UIState.Play)
			return;

		UpdateState(UIStateManager.state);
	}
	#endregion

	#region Methods
	void UpdateState(string newState)
	{
		if (!GameManager.GameRunning)
			return;

		//DIG
		if (_tileState == state.Rock && newState == "Dig" && GameManager.AddGold(-UIStateManager.Instance.DigPrice))
		{
			SetState(state.Dug);
		}
		//FILL
		if (_tileState == state.Dug && newState == "Fill" && GameManager.AddGold(-UIStateManager.Instance.FillPrice))
		{
			SetState(state.Rock);
		}
		//ENTRANCE
		if (_tileState == state.Rock && newState == "Place Entrance")
		{
			SetState(state.Entrance);
		}
		//EXIT
		if (_tileState == state.Rock && newState == "Place Exit")
		{
			SetState(state.Exit);
		}
	}

	/// <summary>
	/// Set the state of this tile.
	/// </summary>
	/// <param name="spawnState">State to change to.</param>
	void SetState(state spawnState)
	{
		_tileState = spawnState;
        if (_tileState == state.Dug)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

		switch (_tileState)
		{
			case state.Rock:
				// Notify the neighbours that something has changed!
				notifyNeighbours();

				Destroy(stateVisual);
				stateVisual = Instantiate(stateRockPrefab, transform.position, transform.rotation, transform);

				Solid = true;
				break;

			case state.Hardrock:
				stateVisual = Instantiate(stateHardrockPrefab, transform.position, transform.rotation, transform);

				Solid = true;
				break;

			case state.Dug:
				// Notify the neighbours that something has changed!
				notifyNeighbours();

				Destroy(stateVisual);
				stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform);

                
				Solid = false;
				break;

			case state.Exit:
				// TODO check if adjacent is dug
				Destroy(stateVisual);
				stateVisual = Instantiate(stateExitPrefab, transform.position, transform.rotation, transform);

				Solid = true;

				// Change exit tile.
				if (TileGrid.ExitTile != null)
					TileGrid.ExitTile.SetState(state.Rock);

				TileGrid.ExitTile = this;

				// Set exit spawn.
				GameManager.ExitSpawn = GetComponentInChildren<Spawner>();

				break;

			case state.Entrance:
				// TODO check if adjacent is dug
				Destroy(stateVisual);
				stateVisual = Instantiate(stateEntrancePrefab, transform.position, transform.rotation, transform);

				Solid = true;

				// Change entrance tile.
				if (TileGrid.EntranceTile != null)
					TileGrid.EntranceTile.SetState(state.Rock);

				TileGrid.EntranceTile = this;

				// Set entrance spawn.
				GameManager.EntranceSpawn = GetComponentInChildren<Spawner>();

				break;
		}
	}

	public void setNeighbouringTiles(List<KeyValuePair<int[], Tile>> NeighbouringTiles)
	{
		this.NeighbouringTiles = NeighbouringTiles;

	}

	//Notify all neighbours of this tile that they should update their neighbours
	public void notifyNeighbours()
	{
		for(int i=0;i<=NeighbouringTiles.Count-1;i++){
			NeighbouringTiles[i].Value.updateTileGraphics();
		}
	}

	//update the graphics
	public void updateTileGraphics()
	{
		if (_tileState == state.Rock)
		{
			//SetState(state.Rock);
			stateVisual.GetComponentInChildren<TileVariantChooser_Rock> ().setTileGraphics (getHood());

			
		}
		//else if (_tileState == state.Hardrock)  {
		//	stateVisual.GetComponentInChildren<TileVariantChooser_Hardrock> ().setTileGraphics (getHood());
		//}
	}

	//Check the neighbourhood and return a list of booleans
	public bool[,] getHood(){
		bool[,] TileHood= new bool[3,3]{{false,false,false},{false,false,false},{false,false,false}};
		bool StatusBit;
		int[] Index = new int[2];

		for(int i=0;i<=NeighbouringTiles.Count-1;i++){
			switch (NeighbouringTiles[i].Value._tileState)
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

		return TileHood;
	}


	public bool AttachObject(InteractableObject objectToAttach)
	{
		if (_attachedObject != null)
		{
			Debug.LogWarning("Already an object attached!");
			return false;
		}

		//objectToAttach.transform.SetParent(transform);
		_attachedObject = objectToAttach;

		return true;
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#region Enums
public enum UIState
{
	Play,
	Buy
}
#endregion

public class UIStateManager : MonoBehaviour {

	UIStateManager instance;

    public static string state;

	#region Properties
	/// <summary>
	/// Current state of the UI.
	/// </summary>
	public static UIState State = UIState.Play;
	#endregion

	#region Fields
	// Placing objects.
	/// <summary>
	/// Object currently placing in the room.
	/// </summary>
	private GameObject _objectToPlace;
	#endregion


	#region Life Cycle
	void Start()
	{
		state = "Dig";
	}

	void Update()
	{
		CheckInput();
	}
	#endregion

	#region Input
	/// <summary>
	/// Checks for any input.
	/// </summary>
	void CheckInput()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		switch (State)
		{
			case UIState.Buy:
				if (Input.GetMouseButtonDown(0))
				{
					Tile tile = Selector.HitInfo.transform.GetComponent<Tile>();

					// You can only place objects on non-solid tiles.
					if (tile == null || tile.Solid == true)
						break;

					if(tile.AttachObject(_objectToPlace.GetComponent<InteractableObject>()))
					{
						Debug.Log("Placing object.");

						_objectToPlace.GetComponent<InteractableObject>().Initialize(tile);
						_objectToPlace = null;

						ChangeState(UIState.Play);
					}

				}
				else if (Input.GetMouseButtonDown(1))
				{
					Debug.Log("Cancelling placement.");

					Destroy(_objectToPlace);
					_objectToPlace = null;

					ChangeState(UIState.Play);
				}

				break;
		}

	}
	#endregion

	#region Methods
	public void StateInput(string input)
	{
		if (_objectToPlace != null)
			Destroy(_objectToPlace);


		ChangeState(UIState.Play);
		state = input;
	}

	/// <summary>
	/// Method to cleanly change the current UI state.
	/// </summary>
	/// <param name="newUIState">New UI state to change to.</param>
	public void ChangeState(UIState newUIState)
	{
		State = newUIState;
	}

	/// <summary>
	/// Start the routine place an object.
	/// </summary>
	/// <param name="objectToPlace">Gameobject to place.</param>
	public void PlaceObject(GameObject objectToPlace)
	{
		if (_objectToPlace != null)
			Destroy(_objectToPlace);

		// Spawn object to place.
		_objectToPlace = Instantiate(objectToPlace);

		// Change the UI state.
		ChangeState(UIState.Buy);

		// Start the placement coroutine.
		StartCoroutine(PlacingObjectCoroutine());
	}

	IEnumerator PlacingObjectCoroutine()
	{
		while (State == UIState.Buy && _objectToPlace != null)
		{
			if (Selector.HitInfo.transform != null && Selector.HitInfo.transform.CompareTag("Tile"))
			{
				Vector3 objectPosition = Selector.HitInfo.transform.position + Vector3.back;

				_objectToPlace.transform.position = objectPosition;
			}

			yield return null;
		}
	}
	#endregion
}

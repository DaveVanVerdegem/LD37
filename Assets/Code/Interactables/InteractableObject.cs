using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
	#region Fields
	/// <summary>
	/// Returns true when this class is initialized.
	/// </summary>
	protected bool _initialized;

	/// <summary>
	/// Tile where this object is attached to.
	/// </summary>
	protected Tile _parentTile;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{

	}

	public virtual void Initialize(Tile parentTile)
	{
		// Assign our parent tile.
		_parentTile = parentTile;

		_initialized = true;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnMouseDown()
	{
		if (!_initialized)
			return;

		Interact();
	}
	#endregion

	#region Interaction
	protected virtual void Interact()
	{

	}
	#endregion
}

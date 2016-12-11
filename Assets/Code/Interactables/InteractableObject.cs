using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider2D))]
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

	/// <summary>
	/// Collider of this interactable object.
	/// </summary>
	private Collider2D _collider;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		_collider = GetComponent<Collider2D>();

		if (_collider == null)
			Debug.LogWarning("No collider attached!", this);

		_collider.enabled = false;
	}

	public virtual void Initialize(Tile parentTile)
	{
		// Assign our parent tile.
		_parentTile = parentTile;

		// Activate collider.
		_collider.enabled = true;

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

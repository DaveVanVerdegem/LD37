using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : InteractableObject
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Amount of shots that this trap can fire.")]
	/// <summary>
	/// Amount of shots that this trap can fire.
	/// </summary>
	private int _shotsAvailable = 1;

	[SerializeField]
	[Tooltip("Attack strength of this trap.")]
	/// <summary>
	/// Attack strength of this trap.
	/// </summary>
	private int _attackStrength = 1;
	#endregion

	#region Fields
	/// <summary>
	/// Amount of shots fired by this trap.
	/// </summary>
	private int _shotsFired = 0;

	private TrapDamageCollider _damageCollider;
	#endregion

	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized trap.");

		_damageCollider = GetComponentInChildren<TrapDamageCollider>();
		if(_damageCollider == null)
			Debug.LogWarning("No damage collider found!", this);

		_damageCollider.Initialize(this);

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

		FireShot();
	}
	#endregion

	#region Methods
	public void DoDamage(Character characterToDamage)
	{
		Debug.Log("Doing damage to " + characterToDamage, characterToDamage);

		// Do damage.
		characterToDamage.GetComponent<Character>().ReceiveDamage(gameObject, _attackStrength);
	}

	void FireShot()
	{
		// Only fire a shot when this trap isn't active yet.
		if (_damageCollider.Active)
			return;

		Debug.Log("Firing shot.");

		// Activate damage collider.
		_damageCollider.Activate();

		// Fire shot.
		_shotsFired++;

		if (_shotsFired >= _shotsAvailable)
		{
			Debug.Log("All shots fired.");
			Destroy(gameObject);
		}
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapDamageCollider : MonoBehaviour
{
	#region Properties
	public bool Active = false;
	#endregion

	#region Fields
	/// <summary>
	/// Damage collider for this trap.
	/// </summary>
	private Collider2D _damageCollider;

	/// <summary>
	/// Trap parent of this damage checker.
	/// </summary>
	private Trap _trap;
	#endregion

	#region Life Cycle
	void Start()
	{
		_damageCollider = GetComponent<Collider2D>();

		if(_damageCollider == null)
		{
			Debug.LogWarning("No collider attached!", this);
			return;
		}

		//_damageCollider.isTrigger = true;
		_damageCollider.enabled = false;
	}

	public void Initialize(Trap trap)
	{
		_trap = trap;
	}

	void OnTriggerEnter2D(Collider2D triggeredCollider)
	{
		Character character = triggeredCollider.GetComponent<Character>();

		if (character != null)
		{
			// Fire shot.
			_trap.DoDamage(character);

			// Deactivate trap.
			Deactivate();
		}
	}

	/// <summary>
	/// Activate trap.
	/// </summary>
	public void Activate()
	{
		Active = true;
		_damageCollider.enabled = true;
	}

	/// <summary>
	/// Deactivate trap.
	/// </summary>
	private void Deactivate()
	{
		Active = false;
		_damageCollider.enabled = false;
	}
	#endregion
}

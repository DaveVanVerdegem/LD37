using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : InteractableObject
{
	#region Enums
	public enum Type
	{
		Area,
		Projectile
	}
	#endregion

	#region Inspector Fields
	[SerializeField]
	[Tooltip("Type of trap.")]
	/// <summary>
	/// Type of trap.
	/// </summary>
	private Type _type = Type.Area;

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

	[SerializeField]
	[Tooltip("Prefab used for projectile traps.")]
	/// <summary>
	/// Prefab used for projectile traps.
	/// </summary>
	private Projectile _projectilePrefab;
	#endregion

	#region Fields
	/// <summary>
	/// Amount of shots fired by this trap.
	/// </summary>
	private int _shotsFired = 0;

	/// <summary>
	/// Damage collider component of this trap. Used for area traps.
	/// </summary>
	private TrapDamageCollider _damageCollider;

	/// <summary>
	/// Spawner for this trap. Used for projectile traps.
	/// </summary>
	private Spawner _spawner;
	#endregion

	#region Life Cycle
	public override void Initialize(Tile parentTile)
	{
		Debug.Log("Initialized trap.");

		if(_type == Type.Area)
		{
			_damageCollider = GetComponentInChildren<TrapDamageCollider>();
			if (_damageCollider == null)
				Debug.LogWarning("No damage collider found!", this);

			_damageCollider.Initialize(this);
		}

		if(_type == Type.Projectile)
		{
			_spawner = GetComponentInChildren<Spawner>();
			if (_spawner == null)
				Debug.LogWarning("Nospawner found!", this);
		}

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

		PrepareShot();
	}
	#endregion

	#region Methods
	/// <summary>
	/// Damage a character.
	/// </summary>
	/// <param name="characterToDamage">Character to damage.</param>
	public void DoDamage(Character characterToDamage)
	{
		Debug.Log("Doing damage to " + characterToDamage, characterToDamage);

		// Do damage.
		characterToDamage.ReceiveDamage(_attackStrength);

		SpendShot();
	}

	void PrepareShot()
	{
		switch(_type)
		{
			case Type.Area:
				// Only fire a shot when this trap isn't active yet.
				if (_damageCollider.Active)
					return;

				Debug.Log("Firing shot.");

				// Activate damage collider.
				_damageCollider.Activate();
				break;

			case Type.Projectile:
				SpendShot();
				FireProjectile();

				break;
		}
	}

	void SpendShot()
	{
		// Fire shot.
		_shotsFired++;

		if (_shotsFired >= _shotsAvailable)
		{
			Debug.Log("All shots fired.");
			Destroy(gameObject);
		}
	}

	void FireProjectile()
	{
		Projectile newProjectile = Instantiate(_projectilePrefab, _spawner.transform.position, _spawner.transform.rotation, _spawner.transform);
		newProjectile.SetAttackStrength(_attackStrength);
	}
	#endregion
}

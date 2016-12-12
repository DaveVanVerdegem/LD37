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
	[Tooltip("Normal sprite for this trap.")]
	/// <summary>
	/// Normal sprite for this trap.
	/// </summary>
	private Sprite _normalSprite;

	[SerializeField]
	[Tooltip("Sprite for loaded trap.")]
	/// <summary>
	/// Sprite for loaded trap.
	/// </summary>
	private Sprite _loadedSprite;

	[SerializeField]
	[Tooltip("Cooldown for this trap.")]
	/// <summary>
	/// Cooldown for this trap.
	/// </summary>
	private float _cooldown = 1f;

	//[Header("Area Trap")]


	[Header("Projectile Trap")]
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

	/// <summary>
	/// Sprite renderer component of this trap.
	/// </summary>
	private SpriteRenderer _spriteRenderer;

	/// <summary>
	/// Timer to use for cooldown.
	/// </summary>
	private float _timer = 0;

	/// <summary>
	/// Timer is running.
	/// </summary>
	private bool _timerRunning = false;
	#endregion


	#region Life Cycle
	private void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		if (_spriteRenderer == null)
			Debug.LogWarning("No sprite renderer found!", this);

		SwitchSprite(_normalSprite);
	}

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
		if(_timerRunning)
		{
			// Run timer.
			if (_timer > 0)
			{
				_timer -= Time.deltaTime;
			}
			else
			{
				// Reset timer.
				_timer = 0;
				_timerRunning = false;

				if (_type == Type.Area)
				{
					SpendShot();
				}
			}
		}

	}
	#endregion

	#region Interaction
	protected override void Interact()
	{
		Debug.Log("Activated trap.");

		LoadShot();
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
		characterToDamage.ReceiveDamage(gameObject, _attackStrength);
        characterToDamage.TriggerHitAnimation();

        SpendShot();
	}

	void LoadShot()
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

				// Switch sprite.
				SwitchSprite(_loadedSprite);

				// Set timer.
				_timer = _cooldown;
				_timerRunning = true;

				break;

			case Type.Projectile:
                FireProjectile();
                SpendShot();
                break;
		}
	}

	void SpendShot()
	{
		// Switch sprite.
		SwitchSprite(_normalSprite);

		if(_damageCollider != null)
			_damageCollider.Deactivate();

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
        Projectile newProjectile = Instantiate(_projectilePrefab, _spawner.transform.position, _spawner.transform.rotation);
		newProjectile.SetAttackStrength(_attackStrength);
	}
	#endregion

	#region Visuals
	void SwitchSprite(Sprite sprite)
	{
		_spriteRenderer.sprite = sprite;
	}
	#endregion
}

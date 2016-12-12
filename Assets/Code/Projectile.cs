using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Speed at which the projectile moves.")]
	/// <summary>
	/// Speed at which the projectile moves.
	/// </summary>
	private float _speed = 1;
	#endregion

	#region Fields
	/// <summary>
	/// Attack strength of this projectile.
	/// </summary>
	private int _attackStrength = 1;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// Move.
		Vector3 movementVector = transform.up * Time.deltaTime * _speed;
		transform.localPosition += movementVector;
	}

	private void OnTriggerEnter2D(Collider2D triggeredCollider)
	{
		Tile tile = triggeredCollider.GetComponent<Tile>();

		if(tile != null && tile.Solid)
		{
			Debug.Log("Projectile hit a wall!", this);

			// Remove projectile.
			Destroy(gameObject);
		}

		Character character = triggeredCollider.GetComponent<Character>();

		if (character != null)
		{
			// Do damage.
			DoDamage(character);

			// Remove projectile.
			Destroy(gameObject);
		}
	}
	#endregion

	#region Methods
	/// <summary>
	/// Set the attack strength of the projectile.
	/// </summary>
	/// <param name="strength">Strength for the projectile.</param>
	public void SetAttackStrength(int strength)
	{
		_attackStrength = strength;
	}

	/// <summary>
	/// Damage a character.
	/// </summary>
	/// <param name="characterToDamage">Character to damage.</param>
	public void DoDamage(Character characterToDamage)
	{
		Debug.Log("Doing damage to " + characterToDamage, characterToDamage);
		characterToDamage.ReceiveDamage(gameObject, _attackStrength);
        characterToDamage.TriggerHitAnimation();
    }
	#endregion
}

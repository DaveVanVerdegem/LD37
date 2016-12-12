using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Effect : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Time that this effect will be displayed.")]
	/// <summary>
	/// Time that this effect will be displayed.
	/// </summary>
	private float _displayTime = .2f;
	#endregion

	#region Fields
	private float _timer = 0;
	#endregion

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timer += Time.deltaTime;

		if (_timer > _displayTime)
			Destroy(gameObject);
	}
}

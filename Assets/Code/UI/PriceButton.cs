using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class PriceButton : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	[Tooltip("Object to check price for.")]
	/// <summary>
	/// Object to check price for.
	/// </summary>
	private InteractableObject _object;

	[SerializeField]
	[Tooltip("Price to use when no object is set.")]
	/// <summary>
	/// Price to use when no object is set.
	/// </summary>
	private int _price = 1;
	#endregion

	#region Fields
	/// <summary>
	/// Button component.
	/// </summary>
	private Selectable _selectable;
	#endregion


	#region Life Cycle
	private void OnEnable()
	{
		GameManager.GoldUpdatedEvent += CheckPrice;
	}

	// Use this for initialization
	void Awake()
	{
		_selectable = GetComponent<Selectable>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnDisable()
	{
		GameManager.GoldUpdatedEvent -= CheckPrice;
	}
	#endregion

	#region Methods
	void CheckPrice()
	{
		if(_object == null)
			_selectable.interactable = (_price <= GameManager.Gold);
		else
			_selectable.interactable = (_object.Price <= GameManager.Gold);
	}
	#endregion
}

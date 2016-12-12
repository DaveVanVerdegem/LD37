using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCounter : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Sprite renderer of the cross.")]
	/// <summary>
	/// Sprite renderer of the cross.
	/// </summary>
	private Image _cross;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region Methods
	public void SwitchCross(bool enabled)
	{
		if(_cross == null)
		{
			Debug.LogWarning("No cross renderer set.", this);
			return;
		}

		_cross.enabled = enabled;
	}
	#endregion
}

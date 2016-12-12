using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCounters : MonoBehaviour
{
	#region Fields
	[SerializeField]
	/// <summary>
	/// 
	/// </summary>
	private List<HeroCounter> _heroCounters = new List<HeroCounter>();
	#endregion

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	#region Methods
	public void TickOffHeroes(int tickedOff)
	{
		for(int i = 0; i < _heroCounters.Count; i++)
		{
			_heroCounters[i].SwitchCross(i < tickedOff);
		}
	}
	#endregion
}

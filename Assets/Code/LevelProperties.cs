using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties : ScriptableObject
{
	[Tooltip("Preset level map.")]
	/// <summary>
	/// Preset level map.
	/// </summary>
	public TextAsset LevelPreset;

	[Tooltip("Heroes that will spawn this level.")]
	/// <summary>
	/// Heroes that will spawn this level.
	/// </summary>
	public List<Character> SpawnList = new List<Character>();
}

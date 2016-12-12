using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LD37/Level Properties")]
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

	[Tooltip("Spawn interval for spawning heroes.")]
	/// <summary>
	/// Spawn interval for spawning heroes.
	/// </summary>
	public float SpawnInterval = 10f;
}

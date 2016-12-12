using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Inspector Fields
	[Header("Scenes")]
	[Tooltip("Main menu scene.")]
	/// <summary>
	/// Main menu scene.
	/// </summary>
	public Object MainMenuScene;

	[Tooltip("Game scene.")]
	/// <summary>
	/// Game scene.
	/// </summary>
	public Object GameScene;


	[Header("Game")]
	[SerializeField]
	[Tooltip("Gold amount that player starts with.")]
	/// <summary>
	/// Gold amount that player starts with.
	/// </summary>
	private int _startingGold = 10;

	[Tooltip("Limit of heroes that can pass.")]
	/// <summary>
	/// Limit of heroes that can pass.
	/// </summary>
	public int HeroLimit = 5;

	[SerializeField]
	[Tooltip("Prefab to spawn chest.")]
	/// <summary>
	/// Prefab to spawn chest.
	/// </summary>
	private Chest _chestPrefab;

	[Tooltip("Properties of this level.")]
	/// <summary>
	/// Properties of this level.
	/// </summary>
	public LevelProperties LevelProperties;
	#endregion

	#region Properties
	/// <summary>
	/// Instance reference of this gamemanager singleton.
	/// </summary>
	public static GameManager Instance;

	/// <summary>
	/// True when the game is running.
	/// </summary>
	public static bool InGame = false;

	/// <summary>
	/// Current amount of gold saved up.
	/// </summary>
	public static int Gold = 0;

	/// <summary>
	/// Ingame chest object.
	/// </summary>
	public static Chest Chest;

	/// <summary>
	/// Spawner for the entrance.
	/// </summary>
	public static Spawner EntranceSpawn;

	/// <summary>
	/// Spawner for the exit.
	/// </summary>
	public static Spawner ExitSpawn;
	#endregion

	#region Fields
	/// <summary>
	/// Index of current level.
	/// </summary>
	private static int _levelIndex = -1;

	/// <summary>
	/// Amount of heroes that have passed this room.
	/// </summary>
	private static int _heroesPassed = 0;

	/// <summary>
	/// Amount of heroes that have been killed in this room.
	/// </summary>
	private static int _heroesKilled = 0;
	#endregion

	#region Events
	public delegate void UpdateAction();
	public static event UpdateAction GoldUpdatedEvent;
	#endregion


	#region Life Cycle
	void Awake()
	{
		// Remove this if it's not the singleton.
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		// Set singleton.
		else if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

	}

	// Use this for initialization
	void Start()
	{
		AddGold(_startingGold);
	}

	// Update is called once per frame
	void Update()
	{
        // Check input.
        if (Input.GetKeyDown(KeyCode.Space) && EntranceSpawn != null && ExitSpawn != null)
        {
            // Spawn hero.
            Character Hero = SpawnCharacter(LevelProperties.SpawnList[Random.Range(0, LevelProperties.SpawnList.Count)]);
            // Hero.NewIdleMovePosition = TileGrid.EntranceTile.transform.position;
        }
		if (Input.GetKeyDown(KeyCode.Return))
			AddGold(10);

		if (Input.GetKeyDown(KeyCode.S))
			InGame = true;

		// Check scene.
		if(InGame && UIStateManager.Instance != null)
		{
			UIStateManager.Instance.DisplayNoChestAlert(Chest == null);
		}
	}
	#endregion

	#region Level
	public static void LoadLevel(string sceneToLoad)
	{
		SceneManager.LoadScene(sceneToLoad);
	}
	#endregion

	#region Methods
	/// <summary>
	/// Replaces the current chest with the new one.
	/// </summary>
	/// <param name="newChest">New chest to place.</param>
	public static void ReplaceChest(Chest newChest)
	{
		if(Chest != null)
		{
			Destroy(Chest.gameObject);
		}

		Chest = newChest;
	}
	#endregion

	#region Gold
	public static bool AddGold(int amountOfGold)
	{
		// We can't use gold that we don't have.
		if (Gold + amountOfGold < 0)
			return false;

		// Adjust the total gold value.
		Gold += amountOfGold;

		// Send notification.
		if (GoldUpdatedEvent != null)
			GoldUpdatedEvent();

		return true;
	}
	#endregion

	#region Spawning
	/// <summary>
	/// Spawn an object.
	/// </summary>
	/// <param name="spawner">Spawner to spawn the object at.</param>
	/// <param name="objectToSpawn">Object to spawn.</param>
	/// <returns>The spawned object.</returns>
	//public static Object SpawnObject(Spawner spawner, Object objectToSpawn)
	//{
	//	Object spawnedObject = Instantiate(objectToSpawn, spawner.transform.position, spawner.transform.rotation);

	//	return spawnedObject;
	//}

	
	public static Character SpawnCharacter(Character characterToSpawn)
	{
		Spawner spawner = (characterToSpawn.Group == "Hero") ? EntranceSpawn : ExitSpawn;

		return Instantiate(characterToSpawn, spawner.transform.position, spawner.transform.rotation);
	}
	#endregion

	#region Characters
	public static void HeroLeavesRoom()
	{
		_heroesPassed++;
		UIStateManager.Instance.HeroCounters.TickOffHeroes(_heroesPassed);

		if (_heroesPassed >= Instance.HeroLimit)
			UIStateManager.Instance.GameOver();
	}

	public static void HeroKilled()
	{
		_heroesKilled++;

		if ((_heroesPassed + _heroesKilled) >= Instance.LevelProperties.SpawnList.Count)
			UIStateManager.Instance.Win();
	}
	#endregion
}

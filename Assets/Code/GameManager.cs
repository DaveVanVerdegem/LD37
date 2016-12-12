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

	[SerializeField]
	[Tooltip("Prefab to spawn chest.")]
	/// <summary>
	/// Prefab to spawn chest.
	/// </summary>
	private Chest _chestPrefab;

	[SerializeField]
	[Tooltip("Character list of heroes to spawn.")]
	/// <summary>
	/// Character list of heroes to spawn.
	/// </summary>
	private List<Character> _heroes = new List<Character>();
	#endregion

	#region Properties
	/// <summary>
	/// Instance reference of this gamemanager singleton.
	/// </summary>
	public static GameManager Instance;

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
		if (Input.GetKeyDown(KeyCode.Space) && EntranceSpawn != null && ExitSpawn != null)
			// Spawn hero.
			SpawnCharacter(_heroes[Random.Range(0, _heroes.Count)]);
	}
	#endregion

	#region Level Loading
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
}

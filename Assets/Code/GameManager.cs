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
	[Tooltip("Prefab to spawn chest.")]
	/// <summary>
	/// Prefab to spawn chest.
	/// </summary>
	private Chest _chestPrefab;
	#endregion

	#region Properties
	public static GameManager Instance;

	/// <summary>
	/// Current amount of gold saved up.
	/// </summary>
	public static int Gold = 500;

	/// <summary>
	/// Ingame chest object.
	/// </summary>
	public static Chest Chest;
	#endregion

	#region Fields

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

	}

	// Update is called once per frame
	void Update()
	{

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

		UIStateManager.Instance.UpdateGoldCounter();

		Debug.Log("Current amount of gold: " + Gold);

		return true;
	}
	#endregion
}

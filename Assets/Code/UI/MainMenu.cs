using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	#region UI Interaction
	public void LoadGameScene()
	{
		GameManager.GameCanStart = true;
		GameManager.LoadLevel(GameManager.Instance.GameScene.name);
	}
	#endregion
}

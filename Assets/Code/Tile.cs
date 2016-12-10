using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Tile : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
=======
[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour {
    
    [SerializeField] GameObject stateRockPrefab;
    [SerializeField] GameObject stateHardrockPrefab;
    [SerializeField] GameObject stateDugPrefab;
    
    GameObject stateVisual;

    public enum state { Rock, Hardrock, Dug}
    [SerializeField] state tileState = state.Rock;

    
    public void Initialize(state spawnState) {
        tileState = spawnState;
        switch(tileState) {
            case state.Rock:
                stateVisual = Instantiate(stateRockPrefab, transform.position, transform.rotation, transform);
                break;
            case state.Hardrock:
                stateVisual = Instantiate(stateHardrockPrefab, transform.position, transform.rotation, transform);
                break;
            case state.Dug:
                stateVisual = Instantiate(stateDugPrefab,   transform.position, transform.rotation, transform);
                break;
        }
    }

    void OnMouseDown() {
        if(tileState==state.Rock) {
            Destroy(stateVisual);
            stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform); 
        }
    }
>>>>>>> 1e7c021f35fdc1e6b092da9dc21763f8a8df970b
}

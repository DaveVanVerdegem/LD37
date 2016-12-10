using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour {
    
    [SerializeField] GameObject stateRockPrefab;
    [SerializeField] GameObject stateHardrockPrefab;
    [SerializeField] GameObject stateDugPrefab;
    
    GameObject stateVisual;

    public enum state { Rock, Hardrock, Dug}
    [SerializeField] state tileState = state.Rock;

    
    public void Initialize(state spawnState) {
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
}

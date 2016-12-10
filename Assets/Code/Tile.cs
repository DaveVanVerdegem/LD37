using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour {
    
    [SerializeField] GameObject stateRockPrefab;
    [SerializeField] GameObject stateHardrockPrefab;
    [SerializeField] GameObject stateDugPrefab;
    [SerializeField] GameObject stateExitPrefab;
    [SerializeField] GameObject stateEntrancePrefab;
    
    GameObject stateVisual;

    public enum state { Rock, Hardrock, Dug, Exit, Entrance}
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

        //DIG
        if(tileState==state.Rock && UIStateManager.state == "Dig") {
            tileState=state.Dug;
            Destroy(stateVisual);
            stateVisual = Instantiate(stateDugPrefab, transform.position, transform.rotation, transform); 
        }
        //FILL
        if(tileState==state.Dug && UIStateManager.state == "Fill") {
            tileState=state.Rock;
            Destroy(stateVisual);
            stateVisual = Instantiate(stateRockPrefab, transform.position, transform.rotation, transform); 
        }
        //ENTRANCE
        if(tileState==state.Rock && UIStateManager.state == "Place Entrance") {
            //TODO check if adjacent is dug
            tileState=state.Entrance;
            Destroy(stateVisual);
            stateVisual = Instantiate(stateEntrancePrefab, transform.position, transform.rotation, transform); 
        }
        //EXIT
        if(tileState==state.Rock && UIStateManager.state == "Place Exit") {
            //TODO check if adjacent is dug
            tileState=state.Exit;
            Destroy(stateVisual);
            stateVisual = Instantiate(stateExitPrefab, transform.position, transform.rotation, transform); 
        }
    }
}

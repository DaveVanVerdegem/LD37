using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour {

    UIStateManager instance;

    public static string state;

    void Start() {
        state = "Dig";
    }

    public void Input(string input) {
        state = input;
    }
}

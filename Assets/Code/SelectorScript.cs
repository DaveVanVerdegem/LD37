using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorScript : MonoBehaviour {
    
    [SerializeField] bool runOnUpdate = true;
    [SerializeField] Vector3 offset =  new Vector3(0,0,-1);



	void Update () {
		if(!runOnUpdate) return;

        //raycast
        //get object
        //place selector on object
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0,0,1), out hit)) {
            if(hit.transform.tag == "Tile") {
                transform.position = hit.transform.position + offset;
            }
        }
	}
}

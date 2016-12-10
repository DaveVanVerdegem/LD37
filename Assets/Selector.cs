using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    
    [SerializeField] bool runOnUpdate = true;
    [SerializeField] Vector3 offset =  new Vector3(0,0,-1);

	#region Properties
	public static RaycastHit HitInfo;
	#endregion


	void Update () {
		if(!runOnUpdate) return;

        //raycast
        //get object
        //place selector on object

        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0,0,1), out HitInfo)) {
            if(HitInfo.transform.tag == "Tile") {
                transform.position = HitInfo.transform.position + offset;
            }
        }
	}
}

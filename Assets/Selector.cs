using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    
    [SerializeField] bool runOnUpdate = true;
    [SerializeField] Vector3 offset =  new Vector3(0,0,-1);

	#region Properties
	public static RaycastHit2D HitInfo;
	#endregion


	void Update () {
		if(!runOnUpdate)
			return;

		//raycast
		//get object
		//place selector on object

		HitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if (HitInfo.collider != null)
		{
            if(HitInfo.transform.tag == "Tile")
			{
                transform.position = HitInfo.transform.position + offset;
            }
        }
	}
}

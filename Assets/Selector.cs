using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    
    [SerializeField] bool runOnUpdate = true;
    [SerializeField] Vector3 offset =  new Vector3(0,0,-1);

	#region Properties
	public static RaycastHit2D HitInfo;
	#endregion


	void Update ()
	{
		if(!runOnUpdate)
			return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		HitInfo = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

		//HitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		//Debug.Log("Running selector...    ", this);

		//Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta);
		//Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.up, Color.yellow);

		if (HitInfo.collider != null)
		{
			//Debug.Log("Hit: " + HitInfo.collider, HitInfo.collider);

            if(HitInfo.transform.tag == "Tile")
			{
                transform.position = HitInfo.transform.position + offset;
            }
			else
			{
                //Debug.Log("Hit this instead: " + HitInfo.transform.name);
            }
        }
		else
		{
            //Debug.Log("Didn't hit a collider.");
        }
	}
}

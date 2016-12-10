/* IdleMovement.cs
 *  
 * Attach this to an object to let it move randomly in both X and Y.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovement : MonoBehaviour {

    public float MaxRadius = 2.0f;
    public float MoveSpeed = 0.1f;
    public float MaxIdleTimeSeconds = 2.0f;

    private float _currentTime = 0.0f; 
    private Vector2 _newPosition = new Vector2(0, 0);
    private float _newTime = 0.0f;

    private bool _newPositionChosen = false;

	void Start () {
        SetNewClockTime();
    }
	
	void Update () {
        _currentTime += Time.deltaTime;
        if (_currentTime > _newTime)
        {
            MoveToLocation();
        }
    }

    void MoveToLocation()
    {
        if (_newPositionChosen == false)
        {
            ChooseNewPosition();
        }
        if (Vector2.Distance(_newPosition, transform.position) == 0f)
        {
            SetNewClockTime();
        }
        else
        {
            float MaxStep = MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _newPosition, MaxStep);
        }
    }

    void SetNewClockTime()
    {
        _newPositionChosen = false;
        _currentTime = 0.0f;
        _newTime = Random.value * MaxIdleTimeSeconds;
    }

    void ChooseNewPosition()
    {   
        _newPosition = (Vector2) transform.position + Random.insideUnitCircle * MaxRadius;
        _newPositionChosen = true;
    }
}

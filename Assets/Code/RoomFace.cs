using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RoomFace : MonoBehaviour
{
	public float Speed = .5f;
	public List<Sprite> RoomFaces = new List<Sprite>();


	private SpriteRenderer _spriteRenderer;
	private float _timer = 0;
	private int _currentIndex = 0;

	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();

		IterateFace();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_timer < Speed)
		{
			_timer += Time.deltaTime;
		}
		else
		{
			_timer = 0;
			IterateFace();
		}
	}

	void IterateFace()
	{
		_currentIndex++;

		if (_currentIndex >= RoomFaces.Count)
			_currentIndex = 0;

		_spriteRenderer.sprite = RoomFaces[_currentIndex];
	}
}

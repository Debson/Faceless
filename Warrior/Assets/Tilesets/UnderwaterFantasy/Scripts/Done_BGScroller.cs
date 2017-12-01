// scrolls a quad object
using UnityEngine;
using System.Collections;

public class Done_BGScroller : MonoBehaviour
{
	public float scrollSpeed;
	public float tileWidth;
  
	private Vector3 startPosition;

	void Start ()
	{
		startPosition = transform.position;	
	}

	void Update ()
	{
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileWidth);
		transform.position = startPosition + Vector3.left * newPosition;
	}
}
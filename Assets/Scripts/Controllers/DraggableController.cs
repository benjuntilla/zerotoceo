using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DraggableController : MonoBehaviour
{
	public UnityEvent dropEvent, dragEvent;
	public float speed = 20f;
	
	private Vector3 _screenPoint;
	private Vector3 _offset;
	private SpriteRenderer _renderer;
	private Rigidbody2D _rb;

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_rb = GetComponent<Rigidbody2D>();
	}

	void OnMouseDown()
	{
		dragEvent.Invoke();
		_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
		_renderer.sortingOrder = 1;
	}
		
	void OnMouseDrag()
	{
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
		_rb.velocity = (new Vector2(cursorPosition.x, cursorPosition.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) * speed;
	}

	void OnMouseUp()
	{
		dropEvent.Invoke();
		_renderer.sortingOrder = 0;
		_rb.velocity = new Vector2(0, 0);
	}
}

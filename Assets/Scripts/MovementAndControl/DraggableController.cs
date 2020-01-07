using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DraggableController : MonoBehaviour
{
	public UnityEvent dropMethod, dragMethod;
	
	private Vector3 _screenPoint;
	private Vector3 _offset;
	private bool _isDragging;
		
	void OnMouseDown()
	{
		dragMethod.Invoke();
		_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
	}
		
	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
		transform.position = cursorPosition;
	}

	void OnMouseUp()
	{
		dropMethod.Invoke();
	}
}

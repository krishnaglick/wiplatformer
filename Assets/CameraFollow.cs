using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	private float dampTime = 0.7f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	void LateUpdate()
	{
		//This shit is weird and apparently bad. I need to recode at some point but idgaf for now.
		if(target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}
}
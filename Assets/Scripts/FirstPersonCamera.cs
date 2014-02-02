using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour 
{
	public float rotationSpeed;
	public bool lockRotation;
	
	private Vector3 rotationDirection;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		rotationDirection.x = -Input.GetAxisRaw("Mouse Y") * rotationSpeed;
		rotationDirection.y = Input.GetAxisRaw("Mouse X") * rotationSpeed;
		rotationDirection *= Time.deltaTime;
		
		if (!lockRotation)
		{
			transform.Rotate(rotationDirection);
			// Lock z-axis roation
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
														0);
		}
	}
}

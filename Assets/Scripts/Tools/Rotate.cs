using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
	public float rotationSpeed;
	
	public bool xRotation;
	public bool yRotation;
	public bool zRotation;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 rotationDirection = new Vector3(0, 0, 0);
		
		if (yRotation)
		{
			rotationDirection = new Vector3(0, Time.deltaTime * rotationSpeed, 0);
		}
		else if (xRotation)
		{
			rotationDirection = new Vector3(Time.deltaTime * rotationSpeed, 0, 0);
		}
		else if (zRotation)
		{
			rotationDirection = new Vector3(0, 0, Time.deltaTime * rotationSpeed);
		}
		
		transform.Rotate(rotationDirection);
	}
}

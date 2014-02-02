using UnityEngine;
using System.Collections;

public class PlayerPointer : MonoBehaviour 
{
	private float lockedYPos;

	// Use this for initialization
	void Start () 
	{
		lockedYPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.rotation = Quaternion.Euler(GameObject.Find("Main Camera").GetComponent<Transform>().localEulerAngles);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
		transform.position = new Vector3(transform.position.x, lockedYPos, transform.position.z);
	}
}

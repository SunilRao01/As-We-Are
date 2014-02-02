using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	/*
	 * HOW TO USE:
	 * - First, make sure you have the 'iTween' script somewhere in
	 *   the project hieierchy
	 * - In order to use this script, simply attach it to a door
	 * - Then you will need to attach a box collider (triggered) next
	 *   and shape the collider as an interactable zone for the player
	 * 
	 * 
	 * */

	private bool canOpen = false;
	private float doorSpeed = 2;

	private Vector3 doorRotation;
	private float timeIterator = 0;

	private Vector3 closePosition;
	private Vector3 openPosition;

	// Use this for initialization
	void Start () 
	{
		closePosition = transform.rotation.eulerAngles;

		openPosition = new Vector3(0, 140, 270);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeIterator++;

		// If door is open
		if (transform.eulerAngles == openPosition)
		{
			doorRotation = closePosition;
		}
		// If door is closed
		else if (transform.eulerAngles == closePosition)
		{
			doorRotation = openPosition;
		}


		

		if (canOpen && Input.GetKeyDown(KeyCode.E))
		{
			iTween.RotateTo(gameObject, doorRotation, doorSpeed);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			canOpen = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			canOpen = false;
		}
	}
}

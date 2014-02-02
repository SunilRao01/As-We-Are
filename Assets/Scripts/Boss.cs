using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour 
{
	private float promptAlpha;
	private string promptText;
	public GUIStyle promptStyle;
	
	private Player playerScript;
	
	private bool hasEntered = false;
	
	// Use this for initialization
	void Start () 
	{
		// Find player from hierarchy
		GameObject player = GameObject.Find("Player");
		playerScript = (Player) player.GetComponent(typeof(Player));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (hasEntered)
		{
			promptText = "Press shift to... ugh... something";
			promptAlpha = 1;
			
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				playerScript.lockMovement = true;
			}
		}
		else
		{
			promptText = "";
			promptAlpha = 0;
		}
		
		
	}
	
	void OnGUI()
	{
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, promptAlpha);
		GUI.Label(new Rect((Screen.width/2), (Screen.height/2), 500, 100), promptText, promptStyle);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			hasEntered = true;
			Debug.Log("Player has entered boss zone.");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			hasEntered = false;
			Debug.Log("Player has exited boss zone.");
		}
	}
}

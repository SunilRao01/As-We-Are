using UnityEngine;
using System.Collections;

public class Prompt : MonoBehaviour 
{
	public TextMesh label;
	
	private float promptAlpha = 0;
	public GUIStyle promptStyle;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Action handler
		if (promptAlpha == 1)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (label)
				{
					// Load respective scene
					Application.LoadLevel(label.text);
				}
				else if (gameObject.name == "save")
				{
					// TODO: Implement game saving
					Debug.Log("Game is saved (oh god if only)");
				}
				else
				{
					Application.LoadLevel("MeditationOverworld");
				}
			}
		}
	}
	
	void OnGUI()
	{
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, promptAlpha);
		
		if (label)
		{
			GUI.Label(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), "Press E to enter " + label.text, promptStyle);
		}
		else
		{
			GUI.Label(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), "Press E to enter " + gameObject.name, promptStyle);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player can entered therapy zone");
			promptAlpha = 1;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player has exited therapy zone");
			promptAlpha = 0;
		}
	}
	
	
}

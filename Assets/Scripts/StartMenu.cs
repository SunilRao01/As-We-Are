using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public GUIStyle buttonStyle;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
	

		// Start button
		if (GUI.Button(new Rect((Screen.width/2) - (Screen.width/4) - 100, (Screen.height/2) + 200, 150, 80), "Start"))
		{
			Application.LoadLevel("Overworld");
		}

		// Options button (?)

		// Exit button
		if (GUI.Button(new Rect((Screen.width/2) + (Screen.width/4), (Screen.height/2) + 200, 150, 80), "Quit"))
		{
			Application.Quit();
		}
	}
}

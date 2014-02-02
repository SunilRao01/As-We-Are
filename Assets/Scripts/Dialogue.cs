using UnityEngine;
using System.Collections;

public struct dialoguePiece
{
	public int person;
	public string dialogueText;
}

public class Dialogue : MonoBehaviour 
{
	// Dialogue Box
	public Texture2D dialogueBoxImage;
	private float _alpha = 0;
	private string dialogue;
	public GUIStyle dialogueStyle;
	public float dialogueWidth;
	public float dialogueHeight;
	
	// Variables
	public bool complete;
	
	// Scrolling Text Effect
	public float letterPause = 0.1f;
	public AudioClip letterSound1;
	public AudioClip letterSound2;
	private AudioClip sound;
	
	private string dialogueText;
	
	private bool scrollComplete;
	
	private dialoguePiece[] dialogues;
	public string[] stringArray;
	public int[] personNumber;
	private int iterator;
	
	// STARTS DIALOGUE
	public bool ready = false;
		
	// Use this for initialization
	void Start () 
	{	
		iterator = 0;
		dialogues = new dialoguePiece[stringArray.Length];
		for (int i = 0; i < stringArray.Length; i++)
		{
			dialogues[i].dialogueText = stringArray[i];
			dialogues[i].person = personNumber[i];
		}
		
		complete = false;
		scrollComplete = false;
		
		// Starting dialogue
		dialogue = dialogues[iterator].dialogueText;

		sound = letterSound2;
		
		
	}
	
	void Update () 
	{
		
		// Make everything alpha==0, complete flag set to true
		if (iterator == stringArray.Length)
		{
			_alpha = 0;
			complete = true;
		}
		
		if (!complete)
		{
			// Handles which portrait to display, which voice to use
			if (dialogues[iterator].person == 1)
			{
				sound = letterSound1;
			}
			else if (dialogues[iterator].person == 2)
			{
				sound = letterSound2;
			}
			
			// Handles Input
			if (Input.GetKeyDown(KeyCode.E) && scrollComplete)
			{
				iterator++;
				dialogueText = "";
				
				if (iterator < stringArray.Length)
				{
					dialogue = dialogues[iterator].dialogueText;
					StartCoroutine(TypeText ());
				}
			}
		}

	}
	
	public void start()
	{
		_alpha = 1;
		StartCoroutine(TypeText());
	}
	
	void OnGUI()
	{
		
			// Make GUI visible/invisible
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
			
			// Display Dialogue Box
			//GUI.DrawTexture(new Rect((Screen.width/2) - 250, (Screen.height/2) + 150, 600, 120), dialogueBoxImage);
			GUI.Box(new Rect((Screen.width/2) - 250, (Screen.height/2) + 150, 600, 120), "");
			
			// Dialogue text
			GUI.Label(new Rect((Screen.width/2) - 200, (Screen.height/2) + 170, dialogueWidth, dialogueHeight), dialogueText, dialogueStyle);
		
	}
	
	IEnumerator TypeText () 
	{
		scrollComplete = false;
		int count = 1;
		
		foreach (char letter in dialogue.ToCharArray()) 
		{
			dialogueText += letter;
			
			if (sound && count % 10 == 0)
			{
				//audio.PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			
			count++;
		}
		
		scrollComplete = true;
	}
	
	
	
}

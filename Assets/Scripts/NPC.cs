using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class NPC : MonoBehaviour 
{
	/*
	 * 
	 * HOW TO USE:
	 * 
	 * - Make sure there is a collider attached to the game object,
	 * and set it to "Is Trigger": This is the area where the player
	 * will be able to interact with the NPC
	 * 
	 * - Next we modify some values on the script in the GUI editor: (no muddling in the code required*) (* = maybe)
		 * - The "Dialogue Beginning" array is the text that the person
		 * will say BEFORE the player can pick options
		 * 
		 * - The "Dialogue Options" array are the options that the 
		 * player can choose from
		 * 
		 * - The "Dialogue Ending" array is the text that the person
		 * will say AFTER the player has picked his option
		 * 
		 * NOTES:
		 * 
		 * - The option the player has selected will be stored in "Selected
		 * Option", whiich you can see in the editor.
		 * - There is also no dialogue box image at the moment, so I'm using
		 * a transparent GUI.box, which works just as well
		 * - I have also implemented a sound effect to play after every letter,
		 * like pokemon and such. Attach it to "Letter Sound" if possible
	 * 
	 * 
	 * */

	// 3D Text Required Percentages
	public TextMesh redLabel;
	public TextMesh blueLabel;
	public TextMesh greenLabel;

	// Dialogue Box
	public Texture2D dialogueBoxImage;
	private float _alpha = 0;
	private string dialogue;
	private GUIStyle dialogueStyle;
	private float dialogueWidth = 470;
	private float dialogueHeight = 50;
	
	// Variables
	public bool complete = false;

	// Scrolling Text Effect
	public float letterPause = 0.1f;
	public AudioClip letterSound;
	private AudioClip sound;
	
	private string dialogueText;
	
	private bool scrollComplete;
	
	private dialoguePiece[] dialogues;

	public string[] dialogueBeforeOptions;
	public string[] dialogueAfterOptions;
	public string[] hiddenDialogueAfterOptions;

	public string[] dialogueOptions;
	public string[] hiddenOptions;

	private int iterator = 0;
	
	// Text input field
	private float choiceAlpha = 0;

	private Vector2 selectorPosition;

	public string selectedOption;
	private int selectorRelativePosition = 0;

	private bool startEnd = false;
	private bool startHiddenEnd = false;

	// PROMPT VARIABLES
	private float promptAlpha = 0;
	private GUIStyle promptStyle;
	private string promptText;
	
	private float dialogueAlpha = 1;
	private float hiddenOptionsAlpha = 0;
	
	private bool canEngage = false;
	
	private Vector3 originalPlayerPosition;
	
	public bool obstacleComplete = false;

	public bool hiddenUnlocked = false;

	// Pill colors: red, blue, purple, green
	public float redPillRequirementPercentage;
	public float bluePillRequirementPercentage;
	public float greenPillRequirementPercentage;

	// Pill rewards
	public float redPillReward;
	public float bluePillReward;
	public float greenPillRewards;
	public float purplePillRewards;

	void Awake()
	{
		// Set up Prompt style
		promptStyle = new GUIStyle();
		promptStyle.font = (Font) Resources.Load("DearJoeFont", typeof(Font));
		promptStyle.fontSize = 44;
		promptStyle.fontStyle = FontStyle.Normal;
		promptStyle.alignment = TextAnchor.MiddleCenter;

		// Set up 
		dialogueStyle = new GUIStyle();
		dialogueStyle.normal.textColor = Color.white;
		dialogueStyle.font = (Font) Resources.Load("Ackermann", typeof(Font));
		dialogueStyle.fontSize = 16;
		dialogueStyle.fontStyle = FontStyle.Normal;
		dialogueStyle.alignment = TextAnchor.UpperLeft;
		dialogueStyle.wordWrap = true;
		dialogueStyle.richText = true;
		dialogueStyle.stretchWidth = true;
	}

	// Use this for initialization
	void Start () 
	{	
		promptText = "Press E to engage " + gameObject.name;

		redLabel.text = redPillRequirementPercentage + "%";
		blueLabel.text = bluePillRequirementPercentage + "%";
		greenLabel.text = greenPillRequirementPercentage + "%";

		// Set up dialogue with beginningDialogue
		dialogues = new dialoguePiece[dialogueBeforeOptions.Length];
		for (int i = 0; i < dialogueBeforeOptions.Length; i++)
		{
			dialogues[i].dialogueText = dialogueBeforeOptions[i];
		}
		
		scrollComplete = false;

		iterator = 0;

		// Starting dialogue
		//dialogue = dialogues[iterator].dialogueText;

		sound = letterSound;

		// Start dialogue selector at first option
		selectorPosition.x = (Screen.width/2) - 225;
		selectorPosition.y = (Screen.height/2) + 200;
	}
	
	void Update () 
	{

		if (complete)
		{
			canEngage = false;
			promptAlpha = 0;
		}

		/*
		 * PROMPTS
		 * */
		if (obstacleComplete)
		{
			promptAlpha = 0;
			canEngage = false;
			
			Destroy(GetComponent<ParticleSystem>());
		}
		
		// Start obstacle dialogue upon interacting with obstacle
		if (Input.GetButtonUp("Interact") && canEngage)
		{			
			// Lock camera rotation
			FirstPersonCamera cameraScript = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
			cameraScript.lockRotation = true;
			
			// Stop velocity of player
			Rigidbody playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
			playerBody.velocity = Vector3.zero;
			
			// Lock player movement
			Player playerScript = GameObject.Find("Player").GetComponent<Player>();
			playerScript.lockMovement = true;
			
			// Prevewnt engagement loop
			canEngage = false;
			
			// Remove prompt
			promptAlpha = 0;

			// Check if player can see hidden dialogue options
			if (GameObject.Find("Player").GetComponent<Player>().redPercentage >= redPillRequirementPercentage
			    && GameObject.Find("Player").GetComponent<Player>().bluePercentage >= bluePillRequirementPercentage
			    && GameObject.Find("Player").GetComponent<Player>().greenPercentage >= greenPillRequirementPercentage)
			{
				hiddenUnlocked = true;
			}
			else
			{
				hiddenUnlocked = false;
			}
			
			// Lastly, start dialogue
			startDialogue();
		}




		// Update selectorPosition with input controls
		if (choiceAlpha == 1)
		{
			if (hiddenOptionsAlpha == 0)
			{
				selectorPosition.y = (Screen.height/2) + 200 + (20 * selectorRelativePosition);

				if (selectorRelativePosition >= 0 && selectorRelativePosition < (dialogueOptions.Length))
				{
					// Handle selector movement
					if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
					{
						// Move selector down
						if (selectorRelativePosition+1 < dialogueOptions.Length)
						{
							selectorRelativePosition++;
						}
					}
					else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
					{
						// Move selector up
						if (selectorRelativePosition-1 >= 0)
						{
							selectorRelativePosition--;
						}
					}
				}
			}
			else
			{
				selectorPosition.y = (Screen.height/2) + 200 + (20 * selectorRelativePosition);
				
				if (selectorRelativePosition >= 0 && selectorRelativePosition < (dialogueOptions.Length + hiddenOptions.Length))
				{
					// Handle selector movement
					if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
					{
						// Move selector down
						if (selectorRelativePosition+1 < (dialogueOptions.Length + hiddenOptions.Length))
						{
							selectorRelativePosition++;
						}
					}
					else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
					{
						// Move selector up
						if (selectorRelativePosition-1 >= 0)
						{
							selectorRelativePosition--;
						}
					}
				}
			}
		}
		// Once you each the end of the beginning dialogue, show the options
		if (!startEnd && iterator == dialogueBeforeOptions.Length-1 && !complete)
		{
			choiceAlpha = 1;

			if (hiddenUnlocked)
			{
				hiddenOptionsAlpha = 1;
			}
		}
		// If you are in the normal end dialogue and you reach the end...
		if (startEnd && iterator == dialogueAfterOptions.Length-1 && !complete)
		{
			promptAlpha = 1;
			promptText = "Press E to exit";

			if (Input.GetKeyDown(KeyCode.E))
			{
				// DISABLE EVERYTHING
				_alpha = 0;
				choiceAlpha = 0;
				hiddenOptionsAlpha = 0;

				GameObject.Find("Player").GetComponent<Player>().lockMovement = false;
				GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().lockRotation = false;

				if (selectedOption == hiddenOptions[0])
				{
					//Give rewards to player

					// Play 'obtain' sound effect

					// Give rewards! Good job!
					GameObject.Find("Player").GetComponent<Player>().redPills += redPillReward;
					GameObject.Find("Player").GetComponent<Player>().bluePills += bluePillReward;
					GameObject.Find("Player").GetComponent<Player>().greenPills += greenPillRewards;
					GameObject.Find("Player").GetComponent<Player>().purplePills += purplePillRewards;

					GameObject.Find("Player").GetComponent<Player>().keys++;

					complete = true;
					promptAlpha = 0;
					startEnd = false;
				}
				else
				{
					// NOTE
					promptAlpha = 0;

					startEnd = false;

					Start();
				}

			}
		}

		/*
		 *  DIALOGUE PROGRESSION
		 * */

		// Progress dialogue with the 'E' key
		if (!complete)
		{
			// Handles Input
			if (Input.GetKeyDown(KeyCode.E) && scrollComplete && choiceAlpha == 0)
			{
				iterator++;
				dialogueText = "";

				if (!startEnd)
				{

					dialogue = dialogues[iterator].dialogueText;
					StartCoroutine(TypeText ());
					
				}










				// ERROR HERE WHEN YOU CHOOSE HIDDEN OPTION
				else if (startEnd)
				{
					dialogue = dialogues[iterator].dialogueText;
					StartCoroutine(TypeText ());
				}
			}

			// Handle selection
			else if (Input.GetKeyDown(KeyCode.E) && choiceAlpha == 1 && hiddenOptionsAlpha == 0)
			{
				// Get rid of choices
				choiceAlpha = 0;

				// Store selected option
				selectedOption = dialogueOptions[selectorRelativePosition];

				// Start using end dialogue
				startEnd = true;

				iterator = 0;

				// If player selected hidden option
				if (selectedOption == hiddenOptions[0])
				{
					// Set up hidden end dialogue
					dialogues = new dialoguePiece[hiddenDialogueAfterOptions.Length];
					for (int i = 0; i < hiddenDialogueAfterOptions.Length; i++)
					{
						dialogues[i].dialogueText = hiddenDialogueAfterOptions[i];
					}
				}
				else
				{
					// Set up end dialogue
					dialogues = new dialoguePiece[dialogueAfterOptions.Length];
					for (int i = 0; i < dialogueAfterOptions.Length; i++)
					{
						dialogues[i].dialogueText = dialogueAfterOptions[i];
					}
				}

				scrollComplete = false;
				
				// Starting dialogue
				dialogue = dialogues[iterator].dialogueText;

				startDialogue();
			}
			// Handle choices with hidden options
			else if (Input.GetKeyDown(KeyCode.E) && choiceAlpha == 1 && hiddenOptionsAlpha == 1)
			{
				// Get rid of choices
				choiceAlpha = 0;
				hiddenOptionsAlpha = 0;

				// Store selected option
				if (selectorRelativePosition >= dialogueOptions.Length)
				{
					selectedOption = hiddenOptions[selectorRelativePosition-dialogueOptions.Length];
				}
				else
				{
					selectedOption = dialogueOptions[selectorRelativePosition];
				}
				
				// Start using end dialogue
				startEnd = true;
				
				iterator = 0;

				if (selectedOption == hiddenOptions[0])
				{
					// Set up end dialogue
					dialogues = new dialoguePiece[hiddenDialogueAfterOptions.Length];
					for (int i = 0; i < hiddenDialogueAfterOptions.Length; i++)
					{
						dialogues[i].dialogueText = hiddenDialogueAfterOptions[i];
					}
				}
				else
				{
					// Set up end dialogue
					dialogues = new dialoguePiece[dialogueAfterOptions.Length];
					for (int i = 0; i < dialogueAfterOptions.Length; i++)
					{
						dialogues[i].dialogueText = dialogueAfterOptions[i];
					}
				}
				
				scrollComplete = false;
				
				// Starting dialogue
				dialogue = dialogues[iterator].dialogueText;
				
				startDialogue();
			}
		}

	}
	
	public void startDialogue()
	{
		scrollComplete = false;

		
		// Starting dialogue
		iterator = 0;
		dialogue = dialogues[iterator].dialogueText;
				
		_alpha = 1;
		choiceAlpha = 0;
		hiddenOptionsAlpha = 0;
		dialogueText = "";
		StartCoroutine(TypeText());
	}
	
	void OnGUI()
	{
		// Prompt
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, dialogueAlpha);
		
		// Engagement Prompt
		Color inColor = new Color(Color.black.r, Color.black.g, Color.black.b, promptAlpha);
		Color outColor = new Color(Color.white.r, Color.white.g, Color.white.b, promptAlpha);
		//GUI.Label(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), promptText, promptStyle);
		ShadowAndOutline.DrawOutline(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), promptText, promptStyle,
		                             outColor, inColor, 1);

		// Make GUI visible/invisible
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
		// Display Dialogue Box
		GUI.Box(new Rect((Screen.width/2) - 250, (Screen.height/2) + 150, 600, 200), "");
		// Dialogue text
		GUI.Label(new Rect((Screen.width/2) - 200, (Screen.height/2) + 170, dialogueWidth, dialogueHeight), dialogueText, dialogueStyle);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, choiceAlpha);

		int i = 0;
		// Display dialogue options
		for (i = 0; i < dialogueOptions.Length; i++)
		{
			// Choices
			GUI.Label(new Rect((Screen.width/2) - 200, (Screen.height/2) + 200 + (20*i), dialogueWidth, dialogueHeight), dialogueOptions[i].ToString(), dialogueStyle);
		}





		// Option selector
		GUI.Label(new Rect(selectorPosition.x, selectorPosition.y, 50, 50), ">", dialogueStyle);





		// Make GUI visible/invisible
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, hiddenOptionsAlpha);
		// Display optional dialogue options
		for (int j = 0; j < hiddenOptions.Length; j++)
		{
			GUI.Label(new Rect((Screen.width/2) - 200, (Screen.height/2) + 200 + (20*i), dialogueWidth, dialogueHeight), hiddenOptions[j].ToString(), dialogueStyle);
			i++;
		}


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
				audio.PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			
			count++;
		}
		
		scrollComplete = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
		promptText = "Press E to engage " + gameObject.name;

		if (other.CompareTag("Player") && !obstacleComplete)
		{
			// If player enters interact zone
			promptAlpha = 1;
			canEngage = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && !obstacleComplete)
		{
			promptAlpha = 0;
			canEngage = false;
		}

	}
	
}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private Vector3 movementDirection;
	private Vector3 rotationDirection;
		
	public float rotationSpeed;
	
	
	public bool lockMovement;	
	
	// New FPS Controls
	public float moveSpeed = 10.0f;
	public float moveForce;

	public float redPills;
	public float redPercentage;
	public float bluePills;
	public float bluePercentage;
	public float purplePills;
	public float purplePercentage;
	public float greenPills;
	public float greenPercentage;
	private float inventoryAlpha = 0;

	// Pause gui
	public float pauseAlpha = 0;
	public bool buttonVisible = false;
	public GUIStyle pauseStyle;

	public bool debug;

	public Texture2D redPillTexture;
	public float pillDivision;

	public GUIStyle pillCountStyle;
	private Fade fadeEffect;

	public float rateOfDrugLoss;

	public int keys;

	//public ArrayList goals;
	public System.Collections.Generic.List<string> goals;
	
	// RED, BLUE, PURPLE, green
	void Awake () 
	{
		fadeEffect = GetComponent<Fade>();
		// Lock and hide cursor
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}

	void OnGUI()
	{
		
		/*
		 * // When the game is paused, show the menu overlay
		if (showMenu)
		{	
			// Pause the game actions (i.e. Player/Enemy movement)
			Time.timeScale = 0;
			
			GUI.color = Color.white;
			GUI.Label(new Rect(Screen.width/2, (Screen.height/2)/3, 300, 100), "Paused", completeStyle);
			
			if (GUI.Button(new Rect((Screen.width/2) - 100, (Screen.height/2) + (Screen.height/4), 200, 100), "Continue", buttonStyle))
			{
				Time.timeScale = 1;
				Screen.lockCursor = true;
				Screen.showCursor = false;
				
				showMenu = false;
			}
		}
		 * 
		 * */
		// Pause Menu
		if (buttonVisible)
		{
			/*if (GUI.Button(new Rect((Screen.width/2), (Screen.height/2), 250, 80), "Resume"))
			{
				
				buttonVisible = false;
			}*/
		}
		
		// Inventory
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, inventoryAlpha - 0.5f);
		
		GUI.DrawTexture(new Rect(100, 100, (Screen.width - 100), (Screen.height - 100)), (Texture) Resources.Load("inventorybg"));
		
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, inventoryAlpha);
		
		// Red pill count
		GUI.DrawTexture(new Rect((Screen.width/2) - (Screen.width/4), (Screen.height/2) - (Screen.height/4), 200, 200), 
		                (Texture) Resources.Load("UI/Red Pill", typeof(Texture)));

		GUI.Label(new Rect((Screen.width/2) - (Screen.width/4) + 160, 
		                   (Screen.height/2) - (Screen.height/4) + 60, 100, 100), "x" + redPills, pillCountStyle);



		// Green pill count
		GUI.DrawTexture(new Rect((Screen.width/2) - (Screen.width/4) + ((Screen.width/pillDivision)), (Screen.height/2) - (Screen.height/4), 200, 200), 
		                (Texture) Resources.Load("UI/Green Pill", typeof(Texture)));

		GUI.Label(new Rect((Screen.width/2) - (Screen.width/4) + (Screen.width/pillDivision) + 160, 
		                   (Screen.height/2) - (Screen.height/4) + 60, 100, 100), "x" + greenPills, pillCountStyle);
		



		// Blue pill count
		GUI.DrawTexture(new Rect((Screen.width/2) - (Screen.width/4) + (2*(Screen.width/pillDivision)), (Screen.height/2) - (Screen.height/4), 200, 200), 
		                (Texture) Resources.Load("UI/Blue Pill", typeof(Texture)));


		GUI.Label(new Rect((Screen.width/2) - (Screen.width/4) + (2*(Screen.width/pillDivision)) + 160, 
		                   (Screen.height/2) - (Screen.height/4) + 60, 100, 100), "x" + bluePills, pillCountStyle);




		// Purple pill count
		GUI.DrawTexture(new Rect((Screen.width/2) - (Screen.width/4) + (3*(Screen.width/pillDivision)), (Screen.height/2) - (Screen.height/4), 200, 200), 
		                (Texture) Resources.Load("UI/Purple Pill", typeof(Texture)));

		GUI.Label(new Rect((Screen.width/2) - (Screen.width/4) + (3*(Screen.width/pillDivision)) + 160, 
		                   (Screen.height/2) - (Screen.height/4) + 60, 100, 100), "x" + purplePills, pillCountStyle);
	}

	void Start()
	{


	}

	void Update()
	{


		// Pause mode
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			buttonVisible = true;
		}
		
		// Decrease pill percentages over time
		if (redPercentage >= rateOfDrugLoss)
		{
			redPercentage -= rateOfDrugLoss;
		}
		if (bluePercentage >= rateOfDrugLoss)
		{
			bluePercentage -= rateOfDrugLoss;
		}
		if (greenPercentage >= rateOfDrugLoss)
		{
			greenPercentage -= rateOfDrugLoss;
		}
		
		GameObject.Find("RedPercentage").GetComponent<GUIText>().text = (Mathf.Round(redPercentage * 100f)/100f).ToString() + "%";
		GameObject.Find("RedFront").renderer.material.SetFloat("_Percent", redPercentage/100f);

		GameObject.Find("BluePercentage").GetComponent<GUIText>().text = (Mathf.Round(bluePercentage * 100f)/100f).ToString() + "%";
		GameObject.Find("BlueFront").renderer.material.SetFloat("_Percent", bluePercentage/100f);

		GameObject.Find("GreenPercentage").GetComponent<GUIText>().text = (Mathf.Round(greenPercentage * 100f)/100f).ToString() + "%";
		GameObject.Find("GreenFront").renderer.material.SetFloat("_Percent", greenPercentage/100f);
		
		handleMovement();
		handleInput();
		
		// DEBUG MODE: Increase player movement
		if (debug)
		{
			moveSpeed = 300;
		}
	}


	
	void handleMovement()
	{
		Vector3 moveDirection = new Vector3(0, 0, 0);
		
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
		{
			moveDirection = (Camera.main.transform.forward * Input.GetAxisRaw("Vertical")) + (Camera.main.transform.right * Input.GetAxisRaw("Horizontal"));
		}
		else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
		{
			moveDirection = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
		}
		else if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
		{
			moveDirection = Camera.main.transform.forward * Input.GetAxisRaw("Vertical");
		}
		
		moveDirection.y = 0;
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection.Normalize();
		moveDirection *= moveSpeed;
		
		if (!lockMovement)
		{
        	rigidbody.AddForce(moveDirection * Time.deltaTime * moveForce);
		}
	}
	

	
	private void handleInput()
	{

		// Pill popping
		if (Input.GetKeyDown(KeyCode.Alpha1) && redPills > 0)
		{
			redPills--;
			redPercentage += 10;

			fadeEffect.startFade(0.5f, Color.red, 1.5f, true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && greenPills > 0)
		{
			greenPills--;
			greenPercentage += 10;

			fadeEffect.startFade(0.5f, Color.green, 1.5f, true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && bluePills > 0)
		{
			bluePills--;
			bluePercentage += 10;

			fadeEffect.startFade(0.5f, Color.blue, 1.5f, true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && purplePills > 0)
		{
			purplePills--;

			if (redPercentage >= 10)
			{
				redPercentage -= 10;
			}
			else
			{
				redPercentage = 0;
			}

			if (bluePercentage >= 10)
			{
				bluePercentage -= 10;
			}
			else
			{
				bluePercentage = 0;
			}

			if (greenPercentage >= 10)
			{
				greenPercentage -= 10;
			}
			else
			{
				greenPercentage = 0;
			}

			fadeEffect.startFade(0.5f, Color.blue + Color.red, 1.5f, true);
		}


		// Inventory
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (inventoryAlpha == 0)
			{
				inventoryAlpha = 1;
			}
			else
			{
				inventoryAlpha = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Screen.lockCursor = false;
			Screen.showCursor = true;
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			Screen.lockCursor = true;
			Screen.showCursor = false;
		}
	}

	void OnCollisionEnter(Collision other)
	{
	
	}
}

using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
	public float promptAlpha = 0;
	public GUIStyle promptStyle;
	public string promptText;
	
	private float dialogueAlpha = 1;
	
	private bool canEngage = false;
	
	private Vector3 originalPlayerPosition;
	
	public bool obstacleComplete = false;
	
	// Use this for initialization
	void Start () 
	{
		promptText = "Press E to engage " + gameObject.name;
	}
	

	
	// Update is called once per frame
	void Update () 
	{
		
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
			
			// Start Dialogue when player sits in chair
			GetComponent<NPC>().startDialogue();
		}

	}
	
	
	
	void OnGUI()
	{
		GUI.color = new Color(GUI.color.r, GUI.color.g, dialogueAlpha);
		
		// Engagement Prompt
		Color inColor = new Color(Color.black.r, Color.black.g, Color.black.b, promptAlpha);
		Color outColor = new Color(Color.white.r, Color.white.g, Color.white.b, promptAlpha);
		//GUI.Label(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), promptText, promptStyle);
		ShadowAndOutline.DrawOutline(new Rect((Screen.width/2) - 250, (Screen.height/2) - 50, 500, 100), promptText, promptStyle,
			outColor, inColor, 1);
	}
	
	void OnTriggerEnter(Collider other)
	{
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

    var currentPosition : int = 0;
    var Delay : float = 0.1; // 10 characters per sec.
    var Text : String = "";
    var additionalLines : String[];
    var nextState: String = "";
     
    function WriteText(aText : String) 
    {
	    guiText.text = "";
	    currentPosition = 0;
	    Text = aText;
    }
     
    function Start()
    {
	    if (Application.loadedLevelName == "Intro-1")
	    {
	    	nextState = "Intro-2";
	    }
	    
	    for (var S : String in additionalLines)
	    
		    Text += "\n" + S;
		    
		    while (true)
		    {
			    if (currentPosition < Text.Length)
			    guiText.text += Text[currentPosition++];
			    yield WaitForSeconds (Delay);
		    }
    }
    
    function OnGUI()
    {
    	if (GUI.Button(Rect((Screen.width/2) + 500, (Screen.height/2), 100, 50), "Next"))
    	{
			Application.LoadLevel(nextState);    	
    	}
    }
     
    @script RequireComponent(GUIText)
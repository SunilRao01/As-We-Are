using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour 
{
	public string stopMusicAt;
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		if (Application.loadedLevelName != stopMusicAt)
		{
			DontDestroyOnLoad(this);
		}
	}
}

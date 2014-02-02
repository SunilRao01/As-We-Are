using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	private bool flash = false;
	private float targetAlpha = 0.0f;
	private Color fadeColor;
	private float fadeSpeed = 0.2f;
	
	private float alpha = 0.0f;

	private Texture2D fadeTexture;
	public int drawDepth = -1000;

	private const float espilion = 0.0001f;

	public void startFade( float inTargetAlpha, Color inFadeColor, float inFadeSpeed, bool inFlash ) {
		fadeColor = inFadeColor;
		fadeSpeed = inFadeSpeed;
		targetAlpha = inTargetAlpha;
		flash = inFlash;
	}
	
	void Start () {
		fadeTexture = new Texture2D(1, 1);
		fadeTexture.SetPixel( 0, 0, new Color(1.0f, 1.0f, 1.0f) );
		fadeTexture.Apply();
	}

	void Update() {
		if (Mathf.Abs(targetAlpha - alpha) > espilion)
		{
			if (targetAlpha > alpha)
			{
				alpha += fadeSpeed * Time.deltaTime;
				alpha = Mathf.Clamp01 (alpha);
				if (targetAlpha < alpha)
					alpha = targetAlpha;
			}
			else
			{
				alpha -= fadeSpeed * Time.deltaTime;
				alpha = Mathf.Clamp01 (alpha);
				if (targetAlpha > alpha)
					alpha = targetAlpha;
			}
		}
		else if (flash) {
			flash = false;
			targetAlpha = 0.0f;
		}
	}
	
	void OnGUI () {
		fadeColor.a = alpha;
		GUI.color = fadeColor;

		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}
}

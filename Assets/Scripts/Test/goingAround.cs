using UnityEngine;
using System.Collections;

public class goingAround : MonoBehaviour {
	private float percent = 0.0f;
	public float speed = 0.001f;
	private float direction = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		percent += speed * direction;
		if (percent > 1.05f) {
			direction = -1.0f;
			Update ();
		}
		if (percent < -0.05f) {
			direction = 1.0f;
			Update ();
		}
		renderer.material.SetFloat ("_Percent", percent);
	}
}

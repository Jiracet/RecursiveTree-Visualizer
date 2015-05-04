using UnityEngine;
using System.Collections;

public class GUICamera : MonoBehaviour {

	private Camera camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("GUI Camera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleUI(bool t) {
		if (t) {
			camera.cullingMask &= ~(1 << LayerMask.NameToLayer ("UI"));
		} else {
			camera.cullingMask |= (1 << LayerMask.NameToLayer ("UI"));
		}
	}
}

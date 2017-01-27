using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

	public Camera[] cameras;
    public CameraConfigurator[] configurators;
	private int activeCamera = 0;

    public Transform Target = null;

	// Use this for initialization
	void Start () {
		foreach (Camera camera in cameras)
			camera.enabled = false;
		cameras [0].enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Camera")) {
			cameras[activeCamera].enabled = false;
			activeCamera ++;
			if (activeCamera > cameras.Length -1)
				activeCamera = 0;
			cameras[activeCamera].enabled = true;
		}
	}
}

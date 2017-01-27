using UnityEngine;
using System.Collections;
using System;

public class CameraLookAt : CameraConfigurator {

	public Transform Target;

    public override void FollowTarget(Transform target)
    {
        Target = target;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Target);
	}
}

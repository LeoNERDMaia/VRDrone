using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour {

    public Transform HeadToFollow;

	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, HeadToFollow.rotation.eulerAngles.y, 0);
	}
}

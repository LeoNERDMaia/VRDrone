using UnityEngine;

using System.Collections;


// Adapter class to make different camera algorithms compatible
public abstract class CameraConfigurator : MonoBehaviour
{
    public abstract void FollowTarget(Transform target);
}

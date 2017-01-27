using UnityEngine;
using System.Collections;

// Basic gyroscope simulator.  Uses the zero and identity to calculate.  This one suffre from gimball lock effect
[System.Serializable]
public class BasicGyro {


	public float Pitch; // The current pitch for the given transform
	public float Roll; // The current roll for the given transform
	public float Yaw; // The current Yaw for the given transform
    public float Height; // The distance from the drone to the nearest object below
    public float Altitude; // The current altitude from the zero position
    public float Ceiling; // The current distance from ceiling.  50m is default when no ceiling is found
    public Vector3 VelocityVector; // Velocity vector
    public float VelocityScalar; // Velocity scalar value

    public void UpdateGyro(Transform transform) {
		Pitch = transform.eulerAngles.x;
		Pitch = (Pitch > 180) ? Pitch - 360 : Pitch;
		
		Roll = transform.eulerAngles.z;
		Roll = (Roll > 180) ? Roll - 360 : Roll;

		Yaw = transform.eulerAngles.y;
		Yaw = (Yaw > 180) ? Yaw - 360 : Yaw;

        Altitude = transform.position.y;

		Ray downRay = new Ray(transform.position, -Vector3.up);
		RaycastHit hit;
		if (Physics.Raycast(downRay, out hit)) {
			Height = hit.distance;
			Debug.DrawLine (transform.position, hit.point, Color.blue, 0.5f);
		}

        Ray upRay = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(upRay, out hit))
        {
            Ceiling = hit.distance;
            Debug.DrawLine(transform.position, hit.point, Color.blue, 0.5f);
        }
        else Ceiling = 50 - Altitude;

        VelocityVector = transform.GetComponent<Rigidbody>().velocity;
        VelocityScalar = VelocityVector.magnitude;
	}
}

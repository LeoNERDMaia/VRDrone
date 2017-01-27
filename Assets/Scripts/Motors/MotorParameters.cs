using UnityEngine;
using System.Collections;

// This is the Motor "Loader/Builder" class.  Need to be attached to every motor point in a drone so DroneLoader can use it.
// This class also hold values to calibrate the motor for the specific position in a drone.
public class MotorParameters : MonoBehaviour {

    public bool InvertDirection = false; // Whether the direction of the motor is counter or counterclockwise
    [Range (-1, 1)] public float PitchFactor = 0.0f; // A factor to be applied to the pitch correction
	[Range (-1, 1)] public float RollFactor = 0.0f; // A factor to be applied to the roll correction

    public GameObject Motor;
    public GameObject Propeller;

	public Motor CreateMotor(GameObject motorPrefab, GameObject propellerPrefab)
	{
        Motor = GameObject.Instantiate(motorPrefab);
        Propeller = GameObject.Instantiate(propellerPrefab);
        Propeller.name = propellerPrefab.name;
		Motor motorScript = Motor.GetComponent<Motor>();
        motorScript.InvertDirection = InvertDirection;
		motorScript.PitchFactor = PitchFactor;
		motorScript.RollFactor = RollFactor;
        motorScript.Propeller = Propeller;

        Transform trPropeller = motorScript.transform.FindChild("Propeller");
        Propeller.transform.position = trPropeller.position;
        Propeller.transform.parent = trPropeller;
        motorScript.DefinePropeller();

		return motorScript;
	}
}

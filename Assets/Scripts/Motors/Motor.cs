using UnityEngine;
using System.Collections;

// Basic motor class.  Have to be applied to a BasicControl class.  The motor only compute its force individualy.  The force application must be done by the Rigidbody class.
public class Motor : MonoBehaviour {

    public float UpForce = 0.0f; // Total force to be applied by this motor.  This may be transfered to the parent RigidBody
    public float SideForce = 0.0f; // Torque or side force applied by this motor.  This may be transfered to the parent RigidBody and get computed with others motors
	public float Power = 2; // A power multiplier.  An easy way to create more potent motors
    public float ExceedForce = 0.0f; // Negative force value when Upforce gets below 0

	public float YawFactor = 0.0f; // A factor to be applied to the side force.  Higher values get a faster Yaw movement
    public bool InvertDirection; // Whether the direction of the motor is counter or counterclockwise
	public float PitchFactor = 0.0f; // A factor to be applied to the pitch correction
	public float RollFactor = 0.0f; // A factor to be applied to the roll correction

    public float Mass = 0.0f;

	public BasicControl mainController; // Parent main controller.  Where usualy may be found the RigidBody
	public GameObject Propeller; // The propeller object.  Annimation will be done here.

	private GameObject PropellerSpin;
	private GameObject PropellerDetail;
	public float SpeedPropeller = 0;

	void Start() {
        DefinePropeller();
	}

    public void DefinePropeller()
    {
        // Detect and atach the "spin" object.  Used to create the blur effect.
        if (Propeller != null)
        {
            PropellerSpin = Propeller.transform.FindChild(Propeller.name + "Spin").gameObject;
            PropellerDetail = Propeller.transform.FindChild(Propeller.name).gameObject;
        }
    }

    // Method called by BasicControl class to calculate force value of this specific motor.  The force application itself will be done at BasicControl class
	public float UpdateForceValues() {
        float UpForceThrottle = Mathf.Clamp(mainController.ThrottleValue, 0, 1) * Power;
        float UpForceTotal = UpForceThrottle;

		UpForceTotal -= mainController.Computer.PitchCorrection * (PitchFactor * 0.003f);
		UpForceTotal -= mainController.Computer.RollCorrection * (RollFactor * 0.003f);

        //ExceedForce = Mathf.Clamp(UpForceTotal, UpForceThrottle * -1, 0);
		//UpForceTotal = Mathf.Clamp (UpForceTotal, 0, UpForceThrottle);

		UpForce = UpForceTotal;

		SideForce = PreNormalize (mainController.Controller.Yaw, YawFactor);

        SpeedPropeller = Mathf.Lerp(SpeedPropeller, UpForce * 2500.0f, Time.deltaTime);
        UpdatePropeller();
        return SpeedPropeller;
	}

    public void UpdatePropeller(float speed)
    {
        SpeedPropeller = Mathf.Lerp(SpeedPropeller, speed, Time.deltaTime);
        UpdatePropeller();
    }

    // Updates the propeller animation and motion blur effect
    private void UpdatePropeller()
    {
        PropellerDetail.SetActive((SpeedPropeller <= 12));
        Color newColor = PropellerSpin.GetComponent<Renderer>().material.color;
        newColor.a = Mathf.Clamp(SpeedPropeller / 20f, 0, 0.75f);
        PropellerSpin.GetComponent<Renderer>().material.color = newColor;
        Propeller.transform.Rotate(0.0f, SpeedPropeller * 200 * Time.deltaTime, 0.0f);
    }

	// Method to apply the factor and clamp the torque to its limit
	private float PreNormalize(float input, float factor) {
		float finalValue = input;

		if (InvertDirection)
			finalValue = Mathf.Clamp (finalValue, -1, 0);
		else
			finalValue = Mathf.Clamp (finalValue, 0, 1);

		return finalValue * (YawFactor / 500);
	}
}

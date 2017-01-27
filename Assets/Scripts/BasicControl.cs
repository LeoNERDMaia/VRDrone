using UnityEngine;
using System.Collections;

public class BasicControl : MonoBehaviour {
    [Header("Physical")]
    public float Resistance = 10;

	[Header("Control")]
	public Controller Controller;
	public float ThrottleIncrease;
	
	[Header("Motors")]
	public Motor[] Motors;
	public float ThrottleValue;

    [Header("Internal")]
    public ComputerModule Computer;
	
	public enum MainStatus {on, off, disabled};
	[Header("Status")]
	public MainStatus status = MainStatus.on;
	private float timeDisabled;

    public int PlayerID;

    private bool built = false;

    // Drone parts are created on clients only.  In order to call Ensemble method on DroneLoader, we must ensure either the loader and drone are ready to that
    void Update()
    {
        if (!built)
        {
            gameObject.name = "Drone_" + PlayerID;
            GameObject loader = null;
            DroneLoader.UpdateLoadersNames();
            loader = GameObject.Find("Loader_" + PlayerID);
            if (loader != null) // If no loader was found, wait for the next Update
            {
                loader.GetComponent<DroneLoader>().EnsembleDrone(this);
                foreach (Motor Motor in Motors)
                    Motor.mainController = this;
                built = true;
            }
        }
    }

	void FixedUpdate() {
        if (Controller != null)
        {
            if (status == MainStatus.on)
            {
                Computer.UpdateComputer(Controller.Pitch, Controller.Roll, Controller.Throttle * ThrottleIncrease);
                ThrottleValue = (Controller.CurrentThrottleMode != Controller.ThrottleMode.None) ? ThrottleValue = Computer.HeightCorrection : Controller.Throttle;
            }
            else
            {
                if (Time.time > timeDisabled + 5)
                    status = MainStatus.on;
                ThrottleValue = 0.0f;
            }
            ComputeMotors();
        } else
        {
            if (Computer != null)
                Computer.UpdateGyro();
            ComputeMotorSpeeds();
        }
	}

    private void ComputeMotors()
    {
        float yaw = 0.0f;
        Rigidbody rb = GetComponent<Rigidbody>();
        int i = 0;
        foreach (Motor motor in Motors)
        {
            motor.UpdateForceValues();
            yaw += motor.SideForce;
            i++;
            Transform t = motor.GetComponent<Transform>();
            rb.AddForceAtPosition(transform.up * motor.UpForce, t.position, ForceMode.Impulse);
        }

        if (status == MainStatus.on)
            rb.AddTorque(0, yaw, 0, ForceMode.Force);
    }

    // Method to be called from remote player's object and make the propeller spin effect without the force effects
    private void ComputeMotorSpeeds()
    {
        foreach (Motor motor in Motors)
        {
            if (Computer.Gyro.Height < 0.1)
                motor.UpdatePropeller(0.0f);
            else
                motor.UpdatePropeller(1200.0f);
        }
    }

    public void CmdImpact(string name, float velocity)
    {
        GameObject drone = GameObject.Find(name);
        drone.GetComponent<BasicControl>().RpcImpact(velocity);
    }

    public void RpcImpact(float velocity)
    {
            Disable();
    }

    public void Disable()
    {
        status = MainStatus.disabled;
        timeDisabled = Time.time;
    }

    void OnCollisionEnter(Collision col) {
		if (col.relativeVelocity.magnitude > Resistance) {
			Disable();
            BasicControl bcOther = col.gameObject.GetComponent<BasicControl>();
            if ((bcOther != null) && (col.relativeVelocity.magnitude > bcOther.Resistance))
                CmdImpact(bcOther.name, col.relativeVelocity.magnitude);
		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicHUD : MonoBehaviour {

    public BasicControl DroneControl;

    public Text AltitudeText;
    public Image AltitudeGauge;
    public Image AltitudeSetGauge;

    public Text SpeedText;
    public Image SpeedGauge;
    public float MaxSpeed;

    public Image InclinationImage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (DroneControl != null)
        {
            if (DroneControl.status == BasicControl.MainStatus.on)
                UpdateEnabled();
            else
                UpdateDisabled();
        }
	}

    private void UpdateEnabled()
    {
        UpdateAltitude();
        UpdateSpeed();
        UpdateInclinometer();
    }

    private void UpdateDisabled()
    {
        AltitudeText.text = "------";
        SpeedText.text = "------";

        UpdateInclinometer();
    }

    private void UpdateAltitude()
    {
        BasicGyro gyro = DroneControl.Computer.Gyro;
        float totalCeiling = gyro.Altitude + gyro.Ceiling;

        AltitudeText.text = gyro.Altitude.ToString("0.00") + "<size=12>m</size>" + "\n";

        AltitudeGauge.fillAmount = gyro.Altitude / totalCeiling;
    }

    private void UpdateSpeed()
    {
        BasicGyro gyro = DroneControl.Computer.Gyro;
        SpeedText.text = gyro.VelocityScalar.ToString("0.00");
        SpeedGauge.fillAmount = gyro.VelocityScalar / MaxSpeed;
    }

    private void UpdateInclinometer()
    {
        InclinationImage.transform.rotation = Quaternion.Euler(0, 0, DroneControl.Computer.Gyro.Roll * -1);
        float positionY = (DroneControl.Computer.Gyro.Pitch / DroneControl.Computer.PitchLimit) * 50;
        InclinationImage.rectTransform.localPosition = new Vector3(0, positionY, 0);
    }
}

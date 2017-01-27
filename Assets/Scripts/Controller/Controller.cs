using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public float Throttle = 0.0f;
	public float Yaw = 0.0f;
	public float Pitch = 0.0f;
	public float Roll = 0.0f;

    public enum ThrottleMode { None, LockHeight};

	[Header("Throttle command")]
	public string ThrottleCommand = "AxisY1";
	public bool InvertThrottle = true;
    public string ThrottleModeCommand = "ThrottleMode";
    private bool ThrottleModePressed = false;
    public ThrottleMode CurrentThrottleMode = ThrottleMode.LockHeight;

    [Header("Yaw Command")]
	public string YawCommand = "AxisX1";
	public bool InvertYaw = false;

	[Header("Pitch Command")]
	public string PitchCommand = "AxisY2";
	public bool InvertPitch = true;

	[Header("Roll Command")]
	public string RollCommand = "AxisX2";
	public bool InvertRoll = true;

    [Header("Right Trigger")]
    public string RightTriggerCommand = "RightTrigger";
    public bool Triggered = false;

	void Update() {
        Throttle = Input.GetAxisRaw(ThrottleCommand) * (InvertThrottle ? -1 : 1);
        Yaw = Input.GetAxisRaw(YawCommand) * (InvertYaw ? -1 : 1);
        Pitch = Input.GetAxisRaw(PitchCommand) * (InvertPitch ? -1 : 1);
        Roll = Input.GetAxisRaw(RollCommand) * (InvertRoll ? -1 : 1);
        Triggered = Input.GetAxisRaw(RightTriggerCommand) > 0;

        if ((Input.GetAxisRaw(ThrottleModeCommand) > 0) && !ThrottleModePressed)
        {
            switch (CurrentThrottleMode)
            {
                case ThrottleMode.None:
                    CurrentThrottleMode = ThrottleMode.LockHeight; break;
                case ThrottleMode.LockHeight:
                    CurrentThrottleMode = ThrottleMode.None; break;
            }
            ThrottleModePressed = true;
        }
        ThrottleModePressed = (Input.GetAxisRaw(ThrottleModeCommand) == 0) && (ThrottleModePressed);
	}

}

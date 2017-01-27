using UnityEngine;
using System.Collections;

public class DroneLoader : MonoBehaviour {

    public GameObject Drone;
    public GameObject UsedComputer;
    public GameObject UsedMotor;
    public GameObject UsedPropeller;
    public GameObject UsedGrapple;
    public GameObject UsedWeapon;

    public Controller Controller;

    public int PlayerID;

    private static int TotalPlayers = 0;

    public static void UpdateLoadersNames()
    {
        foreach (GameObject loader in GameObject.FindGameObjectsWithTag("Loader"))
        {
            DroneLoader oneDroneLoader = loader.GetComponent<DroneLoader>();
            loader.name = "Loader_" + oneDroneLoader.PlayerID;
        }
    }

    void Start()
    {
        transform.Translate(0, 1, 0);
        CmdLoadDrone();
    }

    public void EnsembleDrone(BasicControl control)
    {
        GameObject drone = control.gameObject;
        Rigidbody droneRB = drone.GetComponent<Rigidbody>();
        SetPlayerDetails(control);

        GameObject computer = Instantiate(UsedComputer);
        computer.transform.position = drone.transform.position;
        computer.transform.rotation = drone.transform.rotation;
        computer.transform.parent = drone.transform;
        control.Computer = computer.GetComponent<ComputerModule>();

        Transform transformWeapon = drone.transform.FindChild("WeaponPoint");
        GameObject camera = GameObject.FindGameObjectWithTag("FPVCamera");
        camera.transform.position = transformWeapon.position;
        camera.transform.rotation = transformWeapon.rotation;
        camera.transform.parent = transformWeapon;

        /*
        GameObject weapon = Instantiate(UsedWeapon);
        weapon.transform.position = transformWeapon.position;
        weapon.transform.rotation = transformWeapon.rotation;
        weapon.transform.parent = drone.transform;*/

        MotorParameters[] parameters = drone.GetComponentsInChildren<MotorParameters>();
        control.Motors = new Motor[parameters.Length];

        int i = 0;
        foreach (MotorParameters parameter in parameters)
        {
            Motor motor = parameter.CreateMotor(UsedMotor, UsedPropeller);
            motor.transform.position = parameter.transform.position;
            motor.transform.rotation = parameter.transform.rotation;
            motor.transform.localScale = parameter.transform.localScale;
            motor.transform.parent = parameter.transform;
            control.Motors[i] = motor;
            droneRB.mass += motor.Mass;
            i++;
        }
    }

    private void SetPlayerDetails(BasicControl control)
    {
        Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        control.Controller = Controller;

        CameraSwitch cameraSwitch = GameObject.FindGameObjectWithTag("CameraSwitch").GetComponent<CameraSwitch>();
    }

    public void CmdLoadDrone()
    {
        PlayerID = TotalPlayers;
        TotalPlayers++;
        GameObject drone = Instantiate(Drone);
        drone.name = "Drone_" + PlayerID;
        BasicControl control = drone.GetComponent<BasicControl>();
        control.PlayerID = PlayerID;
        drone.tag = "Player";
        drone.transform.position = transform.position;
        drone.transform.rotation = transform.rotation;
    }
}

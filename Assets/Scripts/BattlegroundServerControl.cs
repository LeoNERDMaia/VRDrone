using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BattlegroundServerControl : NetworkBehaviour {

    public GameObject Flag;

	// Use this for initialization
	void Start () {
	
	}

    override public void OnStartServer()
    {
        base.OnStartServer();
        Transform t = GameObject.FindGameObjectWithTag("FlagPole1").transform;
        GameObject flag = Instantiate(Flag);
        flag.transform.position = t.position;
        NetworkServer.Spawn(flag);
    }
	
	// Update is called once per frame
	void Update () {

	}
}

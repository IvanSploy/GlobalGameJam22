using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Referencias
    public static PlayerManager instance;
    [HideInInspector]
    public GameObject player;
    public GameObject shepherd;
    public GameObject wolf;
    public Transform spawn;
    [HideInInspector]
    public bool nextIsWolf = true;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
            Destroy(this);
        instance = this;

        player = Instantiate(shepherd, spawn);
        FindObjectOfType<CameraTargetSwitcher>().target = player.transform;
        FindObjectOfType<CameraTargetSwitcher>().SwitchToTarget();
    }

    // Update is called once per frame
    [ContextMenu("Cambiar Personaje")]
    public void SwitchPlayer()
    {
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        Destroy(player);
        if (nextIsWolf)
        {
            player = Instantiate(wolf, pos, rot);
        }
        else
        {
            player = Instantiate(shepherd, pos, rot);
        }
        FindObjectOfType<CameraTargetSwitcher>().target = player.transform;
        FindObjectOfType<CameraTargetSwitcher>().SwitchToTarget();
        nextIsWolf = !nextIsWolf;
    }
}

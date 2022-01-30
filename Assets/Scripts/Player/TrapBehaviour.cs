using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    //Referencias
    public GameObject trapTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            trapTarget = other.gameObject;
            gameObject.transform.parent.GetComponentInChildren<WolfIA>().ChangeState(2);
        }
    }
}

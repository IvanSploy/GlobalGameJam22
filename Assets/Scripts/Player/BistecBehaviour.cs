using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BistecBehaviour : MonoBehaviour
{
    //Referencias
    public GameObject bistecTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bistec"))
        {
            bistecTarget = other.gameObject;
            gameObject.transform.parent.GetComponentInChildren<WolfIA>().ChangeState(1);
        }
    }
}

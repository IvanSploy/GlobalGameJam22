using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BistecBehaviour : MonoBehaviour
{
    //Referencias
    private GameObject bistecTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bistec"))
        {
            bistecTarget = other.gameObject;
            Debug.Log("Encontrado");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Sheep"))
        {
            bistecTarget = null;
        }
    }

    public void GoToBistec()
    {

    }

    public void GoToNormalBehaviour()
    {
        //Implementar métodos de la parte de vico.
        Debug.Log("Funcionalidad no implementada.");
    }
}

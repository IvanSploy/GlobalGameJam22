using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfIA : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private SheepBehavior[] sheepBehaviors;
    private bool locked;
    private bool killed = false;

    private bool searchingSheep;
    // Start is called before the first frame update

    private void Awake()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }
    void Start()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
    }

    private void Update()
    {
        if(!locked) SearchSheeps();
    }

    public void ReloadSheeps()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
        killed = false;
    }

    private void SearchSheeps()
    {
        if(killed) ReloadSheeps();
        if (sheepBehaviors.Length == 0)
        {
            //SE ACABA EL JUEGO NO HAY MAS BICHOS

            return;
        }
        float distance = Vector3.Distance(sheepBehaviors[0].transform.position, transform.position);
        int numberSheep = 0;
        for (int i = 1; i < sheepBehaviors.Length; i++)
        {
            if (Vector3.Distance(sheepBehaviors[i].transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(sheepBehaviors[i].transform.position, transform.position);
                numberSheep = i;
            }
        }
        navMeshAgent.destination = sheepBehaviors[numberSheep].transform.position;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Sheep") && locked == false)
        {
            //ESTO ES PARA QUE NO SE COMA A DOS A LA VEZ
            locked = true;
            
            //METER ANIMACION LOBO
            //CREO CORRUTINA PARA QUE LA OVEJA DESAPAREZCA EN CUANTO TERMINE LA ANIMACION
            
            StartCoroutine(KillSheep(collider.gameObject));
        }
    }

    IEnumerator KillSheep(GameObject sheep)
    {
        sheep.GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(sheep);
        killed = true;
        locked = false;
    }
}

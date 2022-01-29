using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfIA : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private SheepBehavior[] sheepBehaviors;
    private bool locked;

    private bool searchingSheep;
    // Start is called before the first frame update
    void Start()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        SearchSheeps();
    }

    private void SearchSheeps()
    {
        if (sheepBehaviors == null)
        {
            //SE ACABA EL JUEGO NO HAY MAS BICHOS
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
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
        locked = false;
        SearchSheeps();
    }
}

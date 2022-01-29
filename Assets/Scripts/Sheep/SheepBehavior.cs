using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SheepBehavior : MonoBehaviour
{
    [SerializeField] private float ganasDeCagar;
    [SerializeField] private float sed;
    [SerializeField] private float chill;
    [SerializeField] private float comida;
    [SerializeField] private float limpieza;
    private float tiempoGanasDeCagar;
    private float tiempoSed;
    private float tiempoChill;
    private float tiempoComida;
    private float tiempoLimpieza;
    private bool isPlayerClose;
    private NavMeshAgent navMeshAgent;
    private PlayerController playerController;
    private int state;
    private bool sinNecesidad, conNecesidad, saciandoNecesidad;
    //0 = Sin necesidad, 1 = Con necesidad, 2 = Saciando necesidad
    private Vector3 nextPosition;
    private Coroutine countdownSheep;
    private Coroutine move;
    private bool locked;
    [SerializeField] private float count;
    [SerializeField] private float setCount;
    private Rigidbody rb;
    private bool picked;
    [SerializeField] private float fleeSpeed;
    [SerializeField] private float chillSpeed;
    [SerializeField] private Slider progressbar;
    [SerializeField] private Image backgroundColor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Sprite [] sprite;
    [SerializeField] private Image emoji;
    private void Awake()
    {        
        state = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent.speed = chillSpeed;
        progressbar = GetComponentInChildren<Slider>();
        canvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        nextPosition = transform.position;
        progressbar.maxValue = setCount;
        progressbar.minValue = 0;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(state);

        if (picked) return;
        
        if (!isPlayerClose)
        {
            if (!transform.position.Equals(nextPosition))
            {
                navMeshAgent.enabled = true;
            }
            else
            {
                navMeshAgent.enabled = false;
            }
        }
        else
        {
            navMeshAgent.enabled = true;
            PlayerCloseMovement();
            
        }
        
    }



    void SinNecesidad()
    {
        if (!sinNecesidad)
        {
            canvas.enabled = false;
            count = setCount;
            UpdateRandomTime();
            sinNecesidad = true;
            conNecesidad = false;
            saciandoNecesidad = false;
        }
        UpdateNeeds();
        if(isPlayerClose) return;
        if (Vector3.Distance(nextPosition,transform.position) <= 1f && locked == false)
            move = StartCoroutine(randomMovement());
    }

    void ConNecesidad()
    {
        if(!conNecesidad)
        {
            canvas.enabled = true;
            Color newColor = new Color(1, 0, 0.4f, 1);
            backgroundColor.color = newColor;
            countdownSheep = StartCoroutine(countdown());
            conNecesidad = true;
            sinNecesidad = false;
            saciandoNecesidad = false;
        }
        if(isPlayerClose) return;
        if (Vector3.Distance(nextPosition, transform.position) <= 1f && locked == false) {
            move = StartCoroutine(randomMovement());
        }
            
    }
    void SaciandoNecesidad()
    {
        if(!saciandoNecesidad)
        {
            canvas.enabled = true;
            Color newColor = new Color(0.5f, 1f, 0.2f, 1);
            backgroundColor.color = newColor;
            StopCoroutine(countdownSheep);
            StopCoroutine(move);
            saciandoNecesidad = true;
            sinNecesidad = false;
            conNecesidad = false;
        }

        if(count != setCount)
            count += Time.deltaTime * 3;
        if (count > setCount)
            ChangeState(0);
        progressbar.value = count;
    }

    void UpdateState(int state)
    {
        switch (state)
        {
            case 0:
                SinNecesidad();
                break;

            case 1:
                ConNecesidad();
                break;

            case 2:
                SaciandoNecesidad();
                break;

            default:
                SinNecesidad();
                break;
        }
    }

    void ChangeState(int newState)
    {
        state = newState;
    }
    
    IEnumerator randomMovement()
    {
        locked = true;
        yield return new WaitForSeconds(Random.Range(5, 12));
        
        var correctDestination = false;

        while (!correctDestination)
        {
            
            
            Vector3 origin = new Vector3(transform.position.x + Random.Range(-3, 3f), transform.position.y + 20,
                transform.position.z + Random.Range(-3, 3f));

            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    correctDestination = true;
                    navMeshAgent.destination = hit.point;
                    nextPosition = hit.point;
                    locked = false;
                }
            }
        }
       
        //nextPosition = new Vector3(transform.position.x + Random.Range(-3, 3f), transform.position.y, transform.position.z + Random.Range(-3, 3f));
    }

    void UpdateNeeds()
    {
        ganasDeCagar += tiempoGanasDeCagar * Time.deltaTime;
        sed += tiempoSed * Time.deltaTime;
        chill += tiempoChill * Time.deltaTime;
        comida += tiempoComida * Time.deltaTime;
        limpieza += tiempoLimpieza * Time.deltaTime;

        if (ganasDeCagar >= 10)
            ChangeEmoji(0);
        else if (sed >= 10)
            ChangeEmoji(1);
        else if (chill >= 10)
            ChangeEmoji(2);
        else if (comida >= 10)
            ChangeEmoji(3);
        else if (limpieza >= 10)
            ChangeEmoji(4);
    }

    void UpdateRandomTime()
    {
        ganasDeCagar = 0;
        sed = 0;
        chill = 0;
        comida = 0;
        limpieza = 0;

        tiempoGanasDeCagar = Random.Range(0.2f, 0.8f);
        tiempoSed = Random.Range(0.2f, 0.8f);
        tiempoChill = Random.Range(0.2f, 0.8f);
        tiempoComida = Random.Range(0.2f, 0.8f);
        tiempoLimpieza = Random.Range(0.2f, 0.8f);
    }

    IEnumerator countdown()
    {
        while(count > 0)
        {
            progressbar.value = count;
            count -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }   
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isPlayerClose = true;
            StopCoroutine(move);
            navMeshAgent.speed = chillSpeed;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if(state == 1)
        {
            if (collider.gameObject.CompareTag("TriggerGanasDeCagar") && ganasDeCagar >= 10)
            {
                ChangeState(2);
            }
            else if(collider.gameObject.CompareTag("TriggerSed") && sed >= 10)
            {
                ChangeState(2);
            }
            else if(collider.gameObject.CompareTag("TriggerChill") && chill >= 10)
            {
                ChangeState(2);
            }
            else if(collider.gameObject.CompareTag("TriggerComida") && comida >= 10)
            {
                ChangeState(2);
            }
            else if(collider.gameObject.CompareTag("TriggerLimpieza") && limpieza >= 10)
            {
                ChangeState(2);
            }
        }
        
    }

    void OnTriggerExit(Collider collider)
    {
        if(state == 2)
        {
            if (collider.gameObject.CompareTag("TriggerGanasDeCagar") && ganasDeCagar >= 10)
            {
                ChangeState(1);
            }
            else if(collider.gameObject.CompareTag("TriggerSed") && sed >= 10)
            {
                ChangeState(1);
            }
            else if(collider.gameObject.CompareTag("TriggerChill") && chill >= 10)
            {
                ChangeState(1);
            }
            else if(collider.gameObject.CompareTag("TriggerComida") && comida >= 10)
            {
                ChangeState(1);
            }
            else if(collider.gameObject.CompareTag("TriggerLimpieza") && limpieza >= 10)
            {
                ChangeState(1);
            }
            else
                return;
        }

        if(collider.gameObject.CompareTag("Player"))
        {
            isPlayerClose = false;
            locked = false;
            navMeshAgent.speed = fleeSpeed;
        }
    }
    
    private void PlayerCloseMovement()
    {
        Vector3 dir = transform.position + (playerController.transform.position - transform.position).normalized * -3;

        Vector3 origin = new Vector3(dir.x, transform.position.y + 20, dir.z);
        
        
        
        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                navMeshAgent.destination = hit.point;
                nextPosition = hit.point;
            }
        }
        
    }

    public void Picked()
    {
        navMeshAgent.enabled = false;
        transform.DORotate(Vector3.forward * 180, 0.3f);
        rb.isKinematic = true;
        picked = true;
        isPlayerClose = false;
    }

    public void Released()
    {
        rb.isKinematic = false;

        StartCoroutine(Recover());
    }

    private void ChangeEmoji(int numberOfEmoji)
    {
        emoji.sprite = sprite[numberOfEmoji];
        ChangeState(1);
    }

    IEnumerator Recover() {
        yield return new WaitForSeconds(3);
        var rot = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        transform.DORotate(rot, 0.5f).OnComplete(() => {
            rb.isKinematic = true;
        });
        yield return new WaitForSeconds(2);

        picked = false;
        navMeshAgent.enabled = true;
        locked = false;
        nextPosition = transform.position;
    }
}

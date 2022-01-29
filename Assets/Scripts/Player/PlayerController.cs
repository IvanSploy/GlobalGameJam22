using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    //Referencias
    private InputController input;
    private Rigidbody rigidbody;
    private VirtualJoystick joystick;
    private VirtualButton interact;

    //Atributos
    public float speed = 5;
    public bool isMoving = false;
    public Vector3 dir;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        input = new InputController();
    }

    private void Start()
    {
        //Móvil
        joystick = FindObjectOfType<VirtualJoystick>();
        joystick.OnUp.AddListener(OnStop);
        interact = FindObjectOfType<VirtualButton>();
        interact.OnClick.AddListener(OnInteract);

        //Keyboard
        input.Player.Movement.performed += (ctx) => OnStartMove();
        input.Player.Movement.canceled += (ctx) => OnStopMove();
        input.Player.Interact.performed += (ctx) => OnInteract();
    }

    private void Update()
    {
        if (joystick.isMoving)
        {
            OnMoveJoystick();
        }

        if (isMoving)
        {
            OnMoveKeyboard();
        }
    }

    //Teclado
    public void OnStartMove()
    {
        isMoving = true;
    }
    public void OnStopMove()
    {
        OnStop();
        isMoving = false;
    }
    public void OnMoveKeyboard()
    {
        Vector2 screenDir = input.Player.Movement.ReadValue<Vector2>();
        dir = Vector3.zero;
        dir.x = screenDir.x;
        dir.z = screenDir.y;
        rigidbody.velocity = (dir * speed);
        
        transform.rotation = Quaternion.LookRotation(dir);
    }
    public void OnInteract()
    {
        Debug.Log("He interaccionao");
    }
    
    //Móvil
    public void OnMoveJoystick()
    {
        rigidbody.velocity = (joystick.dir * speed);
        transform.rotation = Quaternion.LookRotation(joystick.dir);
    }
    public void OnStop()
    {
        Vector3 noVel = rigidbody.velocity;
        noVel.x = 0;
        noVel.z = 0;
        rigidbody.velocity = noVel;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}

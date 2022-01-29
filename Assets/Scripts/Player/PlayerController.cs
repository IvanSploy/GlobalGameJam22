using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    //Referencias
    private InputController input;
    private Rigidbody rigidbody;
    private VirtualJoystick joystick;

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
        joystick = FindObjectOfType<VirtualJoystick>();
        joystick.onUp.AddListener(OnStop);
        input.Player.Movement.performed += (ctx) => OnStartMove();
        input.Player.Movement.canceled += (ctx) => OnStopMove();
        input.Player.Interact.performed += (ctx) => OnInteractKeyboard();
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
    }
    public void OnInteractKeyboard()
    {

    }
    
    //Móvil
    public void OnMoveJoystick()
    {
        rigidbody.velocity = (joystick.dir * speed);
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

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
    [SerializeField] private Animator anim;

    //Atributos
    public float speed = 5;
    public bool isMoving = false;
    public Vector3 dir;

    public float movementValue;

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

        //Keyboard
        input.Player.Movement.performed += (ctx) => OnStartMove();
        input.Player.Movement.canceled += (ctx) => OnStopMove();
    }

    private void Update()
    {
        if (joystick.isMoving)
        {
            OnMoveJoystick();
            anim.SetBool("Moving", true);
        }

        if (isMoving)
        {
            OnMoveKeyboard();
            anim.SetBool("Moving", true);
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
        rigidbody.velocity = (dir * speed) + Vector3.up * rigidbody.velocity.y;
        
        var rot = Quaternion.LookRotation(dir * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
        movementValue = Mathf.Abs(dir.magnitude);
    }
    
    //Móvil
    public void OnMoveJoystick()
    {
        rigidbody.velocity = (joystick.dir * speed) + Vector3.up * rigidbody.velocity.y;

        var rot = Quaternion.Euler(Vector3.zero);
        if (!joystick.dir.Equals(Vector3.zero)) rot = Quaternion.LookRotation(joystick.dir * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
        movementValue = Mathf.Abs(joystick.dir.magnitude);
    }
    public void OnStop()
    {
        Vector3 noVel = rigidbody.velocity;
        noVel.x = 0;
        noVel.z = 0;
        rigidbody.velocity = noVel;
        anim.SetBool("Moving", false);
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

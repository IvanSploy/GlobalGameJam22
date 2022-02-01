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
    [SerializeField] private Animator anim;

    //Atributos
    public float speed = 5;
    public bool isMovingKeyboard = false;
    public bool isMovingJoystick = false;
    public Vector3 dir;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        input = new InputController();
    }

    private void Start()
    {
        //Keyboard
        input.Player.MoveKeyboard.performed += (ctx) => OnStartKeyboard();
        input.Player.MoveKeyboard.canceled += (ctx) => OnStopKeyboard();
        input.Player.MoveJoystick.performed += (ctx) => OnStartJoystick();
        input.Player.MoveJoystick.canceled += (ctx) => OnStopJoystick();
    }

    private void Update()
    {
        if (isMovingKeyboard)
        {
            OnMoveKeyboard();
            anim.SetBool("Moving", true);
        }
        if (isMovingJoystick)
        {
            OnMoveJoystick();
            anim.SetBool("Moving", true);
        }
    }

    //Teclado
    public void OnStartKeyboard()
    {
        isMovingKeyboard = true;
    }
    public void OnStopKeyboard()
    {
        OnStop();
        isMovingKeyboard = false;
    }
    public void OnMoveKeyboard()
    {
        float isSprinting = input.Player.Sprint.ReadValue<float>();
        Vector2 screenDir = input.Player.MoveKeyboard.ReadValue<Vector2>();
        dir = Vector3.zero;
        dir.x = screenDir.x;
        dir.z = screenDir.y;
        float actualSpeed = speed * 0.5f + speed * 0.5f * isSprinting;
        anim.speed = actualSpeed / speed;
        rigidbody.velocity = (-dir * actualSpeed) + Vector3.up * rigidbody.velocity.y;

        var rot = Quaternion.LookRotation(-dir * actualSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
    }

    //Joystick
    public void OnStartJoystick()
    {
        isMovingJoystick = true;
    }
    public void OnStopJoystick()
    {
        OnStop();
        isMovingJoystick = false;
    }
    public void OnMoveJoystick()
    {
        Vector2 screenDir = input.Player.MoveJoystick.ReadValue<Vector2>();
        float actualSpeed = screenDir.magnitude * speed;
        dir = Vector3.zero;
        dir.x = screenDir.x;
        dir.z = screenDir.y;
        anim.speed = actualSpeed / speed;
        rigidbody.velocity = (-dir * actualSpeed) + Vector3.up * rigidbody.velocity.y;

        var rot = Quaternion.LookRotation(-dir * actualSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
    }

    public void OnStop()
    {
        Vector3 noVel = rigidbody.velocity;
        noVel.x = 0;
        noVel.z = 0;
        rigidbody.velocity = noVel;
        anim.SetBool("Moving", false);
        anim.speed = 1f;
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

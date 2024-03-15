using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed;
    public PlayerInput pInput;
    private InputAction move;
    [SerializeField] GameManager gm;
    [SerializeField] AudioSource hammerSound;
    [SerializeField] Camera cam;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pInput = new PlayerInput();
        isGrounded = true;
    }

    private void OnEnable()
    {
        move = pInput.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    void FixedUpdate()
    {
        Vector2 readMoveValue = move.ReadValue<Vector2>();
        Vector3 movement = moveSpeed * readMoveValue.y * cam.transform.forward + moveSpeed * readMoveValue.x * cam.transform.right;
        movement.y = (isGrounded) ? 0 : -5;
        rb.velocity = movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            HammerTrap ht = collision.transform.parent.GetComponent<HammerTrap>();
            if (!ht.isStopped)
            {
                hammerSound.Play();
                gm.RestartGame();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}

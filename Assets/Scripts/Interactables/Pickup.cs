using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : Interactable
{
    private bool pickUpObject;
    private Rigidbody rb;
    private Vector3 initPosition;
    private BoxCollider boxCollider;
    private const float pickUpRange = 2.5f;

    private void Awake()
    {
        pickUpObject = false;
        rb = GetComponent<Rigidbody>();
        boxCollider = rb.GetComponent<BoxCollider>();
        initPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = initPosition;
    }

    public override bool Interact()
    {
        pickUpObject = !pickUpObject;
        rb.isKinematic = pickUpObject;
        gameObject.layer = pickUpObject ? 2 : 0;
        return pickUpObject;
    }

    private void Update()
    {
        if (pickUpObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
            {
                transform.position = hit.point;
            } else
            {
                transform.position = ray.origin + ray.direction * pickUpRange;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Physics.IgnoreCollision(boxCollider, collision.collider);
        }
    }
}

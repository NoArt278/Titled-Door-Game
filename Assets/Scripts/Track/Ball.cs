using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Stoppable
{
    GameManager gm;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void StopObject()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public override void ContinueObject()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gm.hammerCount++;
        }
    }
}

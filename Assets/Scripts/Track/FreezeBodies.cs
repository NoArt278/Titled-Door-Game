using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBodies : Stoppable
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void StopObject()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public override void ContinueObject()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}

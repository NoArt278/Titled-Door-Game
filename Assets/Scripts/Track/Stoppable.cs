using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Stoppable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StopObject();
    }

    public virtual void StopObject()
    {
    }

    public virtual void ContinueObject()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTrap : Stoppable
{
    float maxAngleDeflection = 40.0f;
    public float speedOfPendulum;
    public bool isStopped = true;
    float prevTime = 0;
    [SerializeField] int direction;

    private void Awake()
    {
        float angle = maxAngleDeflection * Mathf.Sin(speedOfPendulum * direction);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if (!isStopped) 
        {
            float angle = maxAngleDeflection * Mathf.Sin((1-(Time.time-prevTime)) * speedOfPendulum * direction);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void StopObject()
    {
        isStopped = true;
        float angle = maxAngleDeflection * Mathf.Sin(speedOfPendulum * direction);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public override void ContinueObject()
    {
        if (isStopped)
        {
            isStopped = false;
            prevTime = Time.time;
        }
    }
}

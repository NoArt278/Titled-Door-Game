using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTrigger : MonoBehaviour
{
    [SerializeField] Door entryDoor;
    private void OnTriggerEnter(Collider other)
    {
        entryDoor.OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        entryDoor.CloseDoor();
    }
}

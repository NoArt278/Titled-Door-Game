using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenEntryDoor : MonoBehaviour
{
    [SerializeField] Door entryDoor;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            entryDoor.OpenDoor();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            entryDoor.OpenDoor();
        }
    }
}

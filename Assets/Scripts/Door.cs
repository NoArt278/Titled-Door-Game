using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen;
    private float openAngle = 110f;
    private AudioSource doorAudio;
    private BoxCollider boxCol;
    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
        doorAudio = GetComponent<AudioSource>();
        isOpen = false;
    }
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorAudio.Play();
            transform.parent.Rotate(Vector3.forward, -openAngle);
            boxCol.enabled = false;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            transform.parent.Rotate(Vector3.forward, openAngle);
            isOpen = false;
            boxCol.enabled = true;
        }
    }
}

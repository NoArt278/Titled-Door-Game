using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEntryDoor : MonoBehaviour
{
    [SerializeField] Door entryDoor;
    [SerializeField] Bomb bomb;
    [SerializeField] GameManager gm;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            entryDoor.CloseDoor();
            gm.SetStartTime();
            if (bomb.gameObject.activeSelf)
            {
                bomb.StartCountdown();
            }
        }
    }
}

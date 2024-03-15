using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Transform tpPoint;
    [SerializeField] Door exitDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            gm.SetCompanion(other.transform.GetSiblingIndex());
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, tpPoint.position.z);
            exitDoor.CloseDoor();
            gm.LoadNextLevel();
        }
    }
}

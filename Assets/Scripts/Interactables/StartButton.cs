using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartButton : Interactable
{
    [SerializeField] GameManager gm;
    private AudioSource btnAudio;
    private void Awake()
    {
        btnAudio = GetComponent<AudioSource>();
    }

    public override bool Interact()
    {
        btnAudio.Play();
        StartCoroutine(pressAnimation());
        gm.RunStoppables();
        gm.pickUpsCount++;
        return false;
    }

    IEnumerator pressAnimation()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2), 0.1f);
        yield return new WaitForSeconds(0.2f);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2), 0.1f);
    }
}

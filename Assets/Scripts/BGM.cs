using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
}

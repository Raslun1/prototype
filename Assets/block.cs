using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    public float clickTime;

    public float parryWindow = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            Destroy(col.gameObject);
            if (Time.time - clickTime < parryWindow)
            {
                
                Debug.Log("parry");
            }
            else
            {
                
                Debug.Log("block");
                
            }
        }
    }
}

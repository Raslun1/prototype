using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pickUp : MonoBehaviour
{
    public Image picture;    // Start is called before the first frame update
    public TMP_Text counter;
    private int count; 
    void Start()
    {
        picture.enabled = false;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            Destroy(col.gameObject);
            picture.enabled = true;
            count++;
            counter.text = count.ToString();
        }
    }
}

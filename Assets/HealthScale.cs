using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScale : MonoBehaviour
{
    public Image healthScale;
    private int basescaleLength = 100;
    private int scaleLength = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TakeDamageScale(int amount)
    {
        scaleLength = scaleLength - amount;
        healthScale.fillAmount = scaleLength / basescaleLength;
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
}

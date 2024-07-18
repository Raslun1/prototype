using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Camera currentCam;
    public Camera nextCam;
    public GameObject nextDoor;
    private Vector2 nextDoorPosition;
    private float characterYPos;

    // Start is called before the first frame update
    void Start()
    {
        nextCam.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Character"))
        {
            currentCam.enabled = false;
            //SceneManager.LoadScene("second_stage");

            nextDoorPosition = nextDoor.transform.position;
            characterYPos = col.gameObject.transform.position[1];
            col.gameObject.transform.position = new Vector3(nextDoorPosition[0], characterYPos, nextDoorPosition[2]);
            nextCam.enabled = true;




        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class character : MonoBehaviour
// {
//     // Start is called before the first frame update
//     public GameObject block;
//     public Collider2D parryTrigger;
//     public float speed = 5f;
//     public float climbSpeed = 4f;
//     private Rigidbody2D rb;
//     private bool isClimbing;
//     private float inputVertical;
//
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         block.SetActive(Input.GetMouseButton(1));
//         float inputHorizontal = Input.GetAxis("Horizontal");
//         float inputVertical = Input.GetAxis("Vertical");
//         transform.Translate(new Vector3(inputHorizontal * speed * Time.deltaTime, 0, 0));
//         if (Math.Sign(inputHorizontal) != 0)
//         {
//             transform.localScale = new Vector3(-Math.Sign(inputHorizontal) * Math.Abs(transform.localScale.x),
//                 transform.localScale.y, transform.localScale.z);
//         }
//
//         if (Input.GetMouseButtonDown(1))
//         {
//             var filter = new ContactFilter2D();
//             filter.NoFilter();
//             var colliders = new List<Collider2D>();
//             Physics2D.OverlapCollider(parryTrigger, filter, colliders);
//             Debug.Log(colliders.Count);
//             foreach (var collider in colliders)
//             {
//                 if (collider.CompareTag("bullet"))
//                 {
//                     var rb = collider.GetComponent<Rigidbody2D>();
//                     rb.velocity = Vector2.left * rb.velocity.magnitude;
//                     Debug.Log("parry");
//                 }
//             }
//         }
//
//         if (isClimbing)
//         {
//             Climb();
//         }
//     }
//
//
//     void Climb()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         Vector2 climbVelocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
//         rb.velocity = climbVelocity;
//     }
//     
//     private void OnTriggerEnter2D(Collider2D col)
//     {
//         if (col.CompareTag("bullet"))
//         {
//             Debug.Log("popal");
//             Destroy(col.gameObject);
//         }
//
//         if (col.CompareTag("Ladder"))
//         {
//             rb.gravityScale = 0f;
//             isClimbing = true;
//         }
//     }
// }
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject block;
    public Collider2D parryTrigger;
    public float speed = 5f;
    public float climbSpeed = 4f;
    private Rigidbody2D rb;
    private bool isClimbing;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        block.SetActive(Input.GetMouseButton(1));
        float inputHorizontal = Input.GetAxis("Horizontal");
        // float inputVertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(inputHorizontal * speed * Time.deltaTime, 0, 0));
        if (Math.Sign(inputHorizontal) != 0)
        {
            transform.localScale = new Vector3(-Math.Sign(inputHorizontal) * Math.Abs(transform.localScale.x),
                transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetMouseButtonDown(1))
        {
            var filter = new ContactFilter2D();
            filter.NoFilter();
            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(parryTrigger, filter, colliders);
            Debug.Log(colliders.Count);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("bullet"))
                {
                    var rb = collider.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.left * rb.velocity.magnitude;
                    Debug.Log("parry");
                }
            }
        }
    }
    
    // void Climb()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    //     Vector2 climbVelocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
    //     rb.velocity = climbVelocity;
    // }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            Debug.Log("popal");
            Destroy(col.gameObject);
        }
        // if (col.CompareTag("Ladder"))
        // {
        //     rb.gravityScale = 0f;
        //     isClimbing = true;
        // }
    }
}
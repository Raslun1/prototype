using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject block;
    public Collider2D parryTrigger;
    public float speed = 5f;
    public float climbSpeed = 4f;
    public int maxHealth = 3;

    private int currentHealth;
    private Rigidbody2D rb;
    private bool isClimbing;
    private float inputVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleMovement();
        HandleParry();

        if (isClimbing)
        {
            Climb();
        }
    }

    private void HandleMovement()
    {
        block.SetActive(Input.GetMouseButton(1));
        float inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(inputHorizontal * speed * Time.deltaTime, 0, 0));

        if (Math.Sign(inputHorizontal) != 0)
        {
            transform.localScale = new Vector3(-Math.Sign(inputHorizontal) * Math.Abs(transform.localScale.x),
                transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleParry()
    {
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
                    if (rb != null)
                    {
                        Debug.Log("parrying");
                        rb.velocity = Vector2.left * rb.velocity.magnitude;
                    }
                    else
                    {
                        Debug.Log("Rigidbody2D not found on bullet");
                    }
                }
            }
        }
    }

    void Climb()
    {
        Vector2 climbVelocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
        rb.velocity = climbVelocity;
        
        if (Math.Abs(inputVertical) < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet"))
        {
            Debug.Log("popal");
            Destroy(col.gameObject);
            TakeDamage();
        }
        if (col.CompareTag("Ladder"))
        {
            rb.gravityScale = 0f;
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            rb.gravityScale = 1f;
            isClimbing = false;
        }
    }

    private void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Character has died");
        // Add death logic here
        Destroy(gameObject);
    }
}

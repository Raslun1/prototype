using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject block;
    public Collider2D parryTrigger;
    public Image healthScale;
    private float basescaleLength = 100;
    private float scaleLength = 100;
    public float speed = 2f;
    public float climbSpeed = 4f;
    public bool damageTaken = false;
    public int maxHealth = 100;
    public int corrosionDamage = 4;
    private int healAmount = 50;
    private int prev_n_sec;


    public Image corrosionScale;
    private float corrosionScaleLength = 100;
    public int corrosionSpeed = 20;
    private float corrosionLevel = 0;
    private float prevCorrosionLevel = 0;
    public float corrosionMaxLevel = 100;
    private bool scaleIsFull = false;

    private float timeWhenIsFull;
    private float prevTime;
    private float timer=0;
    private int prevsecondsTimer;
    private int secondsTimer;

    private int currentHealth;
    private Rigidbody2D rb;
    private bool isClimbing;
    private float inputVertical;

    private bool isInCorrosion;
    private float time_in_corrosion=0;
    private float corrosionStart;
    private bool isPause = false;

    private Animator _animator;
    private static readonly int IsParrying = Animator.StringToHash("isParrying");
    private static readonly int IsGoing = Animator.StringToHash("isGoing");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            Debug.Log(Time.timeScale);
            Time.timeScale = 0;
            isPause = true;
        } else if (isPause && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            isPause = false;
        }
        HandleMovement();
        HandleParry();

        if (isClimbing)
        {
            Climb();
        }

        if (isInCorrosion && scaleIsFull==false)
        {
            //Debug.Log(Time.time);
            time_in_corrosion = Time.time - corrosionStart;
            int n_sec = (int)time_in_corrosion;
            if (prev_n_sec!= n_sec)
            {
                corrosionLevel = corrosionLevel + corrosionSpeed;
            }
            prev_n_sec = n_sec;
            
            TakeCorrosionScale();
            if (corrosionLevel >= corrosionMaxLevel && scaleIsFull==false)
            {
                scaleIsFull = true;
                timeWhenIsFull = Time.time;
            }
        }

        if (scaleIsFull)
        {
            timer = Time.time - timeWhenIsFull;
            secondsTimer = (int)Math.Floor(timer);

            if (prevsecondsTimer != secondsTimer && scaleIsFull) {
                prevsecondsTimer = secondsTimer;
                TakeDamage(corrosionDamage, "corrosion");
            }
            
            
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
            _animator.SetBool(IsGoing, true);
            transform.localScale = new Vector3(Math.Sign(inputHorizontal) * Math.Abs(transform.localScale.x),
                transform.localScale.y, transform.localScale.z);
        }
        else
        {
            _animator.SetBool(IsGoing, false);
        }
    }

    private void HandleParry()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool(IsParrying, true);
            
            var filter = new ContactFilter2D();
            filter.NoFilter();
            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(parryTrigger, filter, colliders);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("bullet"))
                {
                    var rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Debug.Log("parrying");
                        if (transform.localScale.x < 0)
                        {
                            rb.velocity = Vector2.left * rb.velocity.magnitude;
                        } else if (transform.localScale.x > 0)
                        {
                            rb.velocity = Vector2.right * rb.velocity.magnitude;
                        }
                        
                    }
                    else
                    {
                        Debug.Log("Rigidbody2D not found on bullet");
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool(IsParrying, false);
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
            Destroy(col.gameObject);
            TakeDamage(15, "bullet");
        } else if (col.CompareTag("Ladder"))
        {
            rb.gravityScale = 0f;
            isClimbing = true;
        } else if (col.CompareTag("Corrosion"))
        {
            isInCorrosion = true;
            corrosionStart = Time.time;

        } else if (col.CompareTag("Heal"))
        {
            currentHealth = currentHealth + healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            TakeDamageScale(-healAmount);
            Destroy(col.gameObject);
        } else if (col.CompareTag("CorrosionHeal"))
        {
            corrosionScaleLength = corrosionMaxLevel;
            corrosionLevel = 0;
            corrosionScale.fillAmount = corrosionMaxLevel;
            scaleIsFull = false;
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            rb.gravityScale = 1f;
            isClimbing = false;
        } else if (col.CompareTag("Corrosion"))
        {
            isInCorrosion = false;
        }
    }




    public void TakeDamageScale(float amount)
    {
        scaleLength = scaleLength - amount;
        if (scaleLength > basescaleLength)
        {
            scaleLength = basescaleLength;
        } else if (scaleLength < 0){
            scaleLength = 0;
        }
        healthScale.fillAmount = scaleLength / basescaleLength;
    }


    
    public void TakeCorrosionScale()
    {
        corrosionScaleLength = corrosionMaxLevel - corrosionLevel;
        corrosionScale.fillAmount = corrosionScaleLength / corrosionMaxLevel;
    }
    

    private void TakeDamage(int amount, string trapType)
    {
        currentHealth = currentHealth - amount;
        if (trapType == "bullet")
        {
            TakeDamageScale(15);
        } else if (trapType == "corrosion")
        {
            TakeDamageScale(corrosionDamage);
        }
        
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
        SceneManager.LoadScene("parrying");
    }
}

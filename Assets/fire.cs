using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;

public class fire : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 1;
    private float timer;
    private bool startShooting = false;
    private bool isFirstBullet = true;
    private float prevBulletTime=0;
    private float firstShootTime;
    private AudioSource audioSource;
    private float shootingStart;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Character"))
        {
            startShooting = true;
            shootingStart = Time.time;
        }
    }

    void Update()
    {   
        
        if (startShooting)
        {
            
            if (isFirstBullet)
            {
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
                timer = shootingStart;
                isFirstBullet = false;
                audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);
            }
            else if (Time.time - timer > 1.5)
            {
                timer = Time.time;
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
                audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);
            }
        }









        /*if (Time.time > fireStart && isFirstBullet)
        {
            timer = Time.time - prevBulletTime;

            if ()
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            prevBulletTime = Time.time;
            isFirstBullet = false;



        } else if (Time.time > prevBulletTime + 1.5 && isFirstBullet == false)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            prevBulletTime = Time.time;
        }*/




        /*if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            
        }*/

        /*audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);*/
    }




}

 
public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
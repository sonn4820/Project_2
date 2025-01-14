﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioSource LavaDeath;
    public AudioSource OrbitCollide;
    public GameObject Spark;
    private bool isCreated;

    private Transform circleAttachedTo;
    private bool onCircle;
    private Vector3 moveDirection = Vector3.down; //Start by moving down.
    Rigidbody2D RB;
    float jumpForce = 2000f;
    public Orbit OrbitScript;

    //public float gForce = 6f;

    private void Update()
    {

        //If not on the circle, keep moving in moveDirection.
        if (!onCircle)
        {
            transform.position += moveDirection * 5 * Time.deltaTime;
        }
        //If on the circle, update our moveDirection vector and wait for SPACE to jump.
        else
        {
            AttachToOrbit();
            moveDirection = (transform.position - circleAttachedTo.position);
            Debug.DrawLine(transform.position, transform.position + moveDirection);
            //Jumped, so detach from circle.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                onCircle = false;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                Debug.Log("Jumped!");
                transform.SetParent(null);
                isCreated = false;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "orbit")
        {
            OrbitScript = collision.transform.GetComponent<Orbit>();
            Debug.Log("Hit Circle!");
            onCircle = true;
            circleAttachedTo = collision.transform;
            transform.SetParent(circleAttachedTo);
            OrbitCollide.Play();
            if (!isCreated)
            {
                Instantiate(Spark, transform.position, transform.rotation);
                isCreated = true;
            }
        }

        if (collision.gameObject.tag == "lava")
        {
            LavaDeath.Play();
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "wall")
        {
            LavaDeath.Play();
            Destroy(this.gameObject);
        }
    }

    public void AttachToOrbit()
    {
        Vector3 dir;
        dir = (circleAttachedTo.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(dir * OrbitScript.gForce/2);
        Debug.Log(OrbitScript.gForce);
    }
}
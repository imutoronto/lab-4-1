using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D rb;
    public bool facingRight;
    public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (facingRight)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);
    }

    void OnCollisionEnter2D(Collision2D c)
    {

        // Check if Enemy hit something not tagged ”Ground”
        if (c.gameObject.tag != "Ground")
        {
            // Flip Enemy
            flip();
        }

        if (c.gameObject.tag == "Projectile")
        {
            // Destroy Enemy if it collides with player projectile
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "NOTground")
        {
            // Flip Enemy
            flip();
        }
    }

    void flip()
    {
        // Toggle variable
        facingRight = !facingRight;
        // Make a copy of old scale
        Vector3 scaleFactor = transform.localScale;
        // Flip the localScale
        scaleFactor.x *= -1;
        // Update localScale to flipped value
        transform.localScale = scaleFactor;
    }

}
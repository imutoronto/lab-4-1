using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Used to tell GameObject (Projectile) how fast to move
    public float speed;

    // Used to tell GameObject (Projectile) how long to live without colliding with anything
    public float lifeTime;



    public

    // Use this for initialization
    void Start()
    {

        // Check if speed was set to something not 0
        if (speed == 0)
        {
            // Assign a default value if one is not set in the Inspector
            speed = 7.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.Log("speed was not set. Defaulting to " + speed);
        }

        // Check if speed was set to something not 0
        if (lifeTime <= 0)
        {
            // Assign a default value if one is not set in the Inspector
            lifeTime = 1.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.Log("lifeTime was not set. Defaulting to " + lifeTime);
        }



        // Take Rigidbody2D component and change its velocity to value passed

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);




        /*
         * temp.GetComponent<Rigidbody2D>().velocity =
         * Vector2.right * projectileForce;
         */

        // Destroy gameObject after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);

    }

    // Check for collisions with other GameObjects
    // - One or both GameObjects must have a Rigidbody2D attached
    // - Both need colliders attached
    void OnCollisionEnter2D(Collision2D c)
    {
        // Check if "Projectile" GameObject hits something not named "Player"
        // - Stops "Projectile" GameObject from being destroyed if it hits "Player" GameObject
        if (c.gameObject.tag != "Player")
        {
            // Destory GameObject Script is attached to
            Destroy(gameObject);
        }

        // Destory GameObject that this GameObject collides with
        // - 'c' can be called anything because it is a variable name
        //Destroy(c.gameObject);
    }
}
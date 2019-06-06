using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public bool flag = false;
    public bool flagEnded = false;

    // Method 1: Keeps a reference Rigidbody2D through script
    // - Not shown in Inspector
    Rigidbody2D rb;

    // Method 2: Keeps a reference Rigidbody2D through script
    // - Shown in Inspector
    public Rigidbody2D rb2;

    // Handles movement speed of Character
    // - Can be adjusted through Inspector while in Play mode
    // - Used to debug movements and test the 'speed' of the Character
    public float speed;

    //variable to look left and right
    public bool isFacingLeft;

    // How high Character jumps
    public float jumpForce;

    // Is 'Character' on ground
    // - Are they able to jump
    // - Must be grounded to jump (aka no double jump)
    public bool isGrounded;

    public LayerMask isEnemyLayer; // used for raycast check to jump on enemies

    // What is the Ground? 
    // - Player can only jump on GameObjects that are on "Ground" layer  
    public LayerMask isGroundLayer;

    // Tells script where to check if 'Characer' is on ground
    public Transform groundCheck;

    // Size of overlapping circle being checked against ground Colliders
    public float groundCheckRadius;


    // Handles animation states for Character
    // - Idle, Run, Attack...etc.
    Animator anim;

    // Handle projectile Instantiation (aka Creation)
    public Transform projectileSpawnPoint;
    public Projectile projectilePrefab;
    public float projectileForce;


    // Use this for initialization
    void Start()
    {

        // Method 1: Save a reference of Component in script
        // - Component must be added in Inspector
        rb = GetComponent<Rigidbody2D>();

        // Check if variable is set to something not 0
        if (speed <= 0)
        {
            // Set a default value to variable if not set in Inspector
            speed = 5.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("Speed not set on " + name + ". Defaulting to " + speed);
        }


        // Check if variable is set to something not 0
        if (jumpForce <= 0)
        {
            // Set a default value to variable if not set in Inspector
            jumpForce = 5.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("JumpForce not set on " + name + ". Defaulting to " + jumpForce);
        }

        // Check if variable is set to something
        if (!groundCheck)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("GroundCheck not found on " + name);
        }

        // Check if variable is set to something not 0
        if (groundCheckRadius <= 0)
        {
            // Set a default value to variable if not set in Inspector
            groundCheckRadius = 0.2f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("GroundCheckRadius not set on " + name + ". Defaulting to " + groundCheckRadius);
        }

        // Save a reference of Component in script
        // - Component must be added in Inspector
        anim = GetComponent<Animator>();

        // Check if Component exists
        if (!anim)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("Animator not found on " + name);
        }

        // Checks if projectileSpawnPoint GameObject is connected
        if (!projectileSpawnPoint)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("No projectileSpawnPoint found on GameObject");
        }

        // Checks if projectileSpawnPoint GameObject is connected
        if (!projectilePrefab)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("No projectilePrefab found on GameObject");
        }

        // Check if speed was set to something not 0
        if (projectileForce == 0)
        {
            // Assign a default value if one is not set in the Inspector
            projectileForce = 7.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.Log("projectileForce was not set. Defaulting to " + projectileForce);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (!flag)
        {
            // Checks if Left (or a) or Right (or d) is pressed
            // - "Horizontal" must exist in Input Manager (Edit-->Project Settings-->Input)
            // - Returns -1(left), 1(right), 0(nothing)
            // - Use GetAxis for value -1-->0-->1 and all decimal places. (Gradual change in values)
            float moveValue = Input.GetAxisRaw("Horizontal");

            // Check if 'groundCheck' GameObject is touching a Collider on Ground Layer
            // - Can change 'groundCheckRadius' to a smaller value for better precision or if 'Character' is smaller or bigger
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

            // Check if "Jump" button was pressed
            // - "Jump" must exist in Input Manager (Edit-->Project Settings-->Input)
            // - Configuration can be changed later
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // Prints a message to Console (Shortcut: Control+Shift+C)
                Debug.Log("Jump");

                // Zeros out force before applying a new force
                // - If force is not zeroed out, the force of gravity will have an effect on the jump
                // - Not setting velocity to 0
                //   - Gravity is -9.8 and force up would be 5 causing a force of -4.8 to be applied
                // - Setting velocity to 0
                //   - Gravity is reset to and force up would be 5 causing a force of 5.0 to be applied
                rb.velocity = Vector2.zero;

                // Unit Vector shortcuts that can be used
                // - Vector2.up --> new Vector2(0,1);
                // - Vector2.down --> new Vector2(0,-1);
                // - Vector2.right --> new Vector2(1,0);
                // - Vector2.left --> new Vector2(-1,0);

                // Applies a force in the UP direction
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            // Check if Left Control was pressed
            // - Tied to key and cannot be changed
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {

                // Call function to make pew pew
                fire();
            }
            // Move Character using Rigidbody2D
            // - Uses moveValue from GetAxis to move left or right
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);

            // Tells Animator to transition to another Clip
            // - Parameter must be created in Animator window under Parameter tab
            anim.SetFloat("speed", Mathf.Abs(moveValue));

            if ((isFacingLeft && moveValue > 0) ||
               (!isFacingLeft && moveValue < 0))
            {
                flip();
            }

            anim.SetBool("isGrounded", isGrounded);

        }
        else if (flagEnded)
        {
            transform.Translate(new Vector2(Time.deltaTime, 0));

        }

    }

    // Function used to create and fire a Projectile
    void fire()
    {
        // Creates Projectile and add its to the Scene
        // - projectPrefab is the thing to create
        // - projectileSpawnPoint is where and what rotation to use when created
        Projectile temp =
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        /*temp.GetComponent<Rigidbody2D>().velocity =
            new Vector2(projectileForce, 0);

        temp.GetComponent<Rigidbody2D>().velocity =
            Vector2.right * projectileForce;
        */

        // Apply movement speed to Projectile that is spawned
        // - Lets the projectile handle its own movement
        if (isFacingLeft)
        {
            temp.speed = -projectileForce;
        }
        else
        {
            temp.speed = projectileForce;
        }

    }

    //flip function
    void flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector2 scaleFactor = transform.localScale;
        scaleFactor.x *= -1;
        transform.localScale = scaleFactor;

    }

    //powerup destroy
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "powerup")
            Destroy(c.gameObject);

        if (c.gameObject.tag == "endoflevel")
        {
            anim.SetTrigger("flag");
            flag = true;
            rb.velocity = new Vector2(0, 0);
        }

        if (flag && c.gameObject.tag == "EndOfFlag")
        {
            anim.SetTrigger("end");
            anim.SetFloat("speed", 1);
            anim.SetBool("isGrounded", true);
            flagEnded = true;
        }


        if(flag && c.gameObject.tag == "DestroyPlayer")
        {
            Destroy(gameObject);
        }
       
    }


    //Secret Enterence
    void OnTriggerStay2D(Collider2D c)
    {

        if (c.gameObject.tag == "secret1")
        {
            if (Input.GetKeyDown("down"))
            {
                transform.position = new Vector3(-8.5f, -6.5f, transform.position.z);
            }
        }

        if (c.gameObject.tag == "secret2")
        {
            if (Input.GetKeyDown("right"))
            {
                transform.position = new Vector3(110f, -1.7f, transform.position.z);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {

        if (c.gameObject.tag == "Enemy" && !isGrounded)
        {
            //Raycast makes a "ray" or a line in this case straight down(vector2.down) from the character's position(transform.position) on 
            //the "Enemy" layer to check if a collider with the tag "Enemy" is colliding with the Ray

            //then if it does that means we jumped on top of the enemy and did NOT walk into the enemy and then we can tell the animator to start the death animation
            RaycastHit2D info = Physics2D.Raycast(transform.position, Vector2.down, 1, isEnemyLayer);
            if (info.collider.gameObject.tag == "Enemy")
            {
                Animator an = info.collider.GetComponent<Animator>();
                an.SetTrigger("kill");
                c.gameObject.GetComponent<Enemy>().speed = 0;
                Destroy(an.gameObject, 1);
                rb.AddForce(Vector2.up * (jumpForce / 2), ForceMode2D.Impulse);
            }
        }
        else if (c.gameObject.tag == "Enemy" && isGrounded)
        {
            Destroy(gameObject);
        }
    }



}

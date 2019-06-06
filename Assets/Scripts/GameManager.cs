using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Must be added if using SceneManager functions
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{


    // Creates a class variable to keep track of 'GameManager' instance
    static GameManager _instance = null;

    // Used to keep track of 'score' in game
    int _score;

    int lives = 3;

    public Text ScoreText;


    // Use this for initialization
    void Start()
    {

        // Check if 'GameManager' instance exists
        if (instance)
            // 'GameManager' already exists, delete copy
            Destroy(gameObject);
        else
        {
            // 'GameManager' does not exist so assign a reference to it
            instance = this;

            // Do not destroy 'GameManager' on Scene change
            DontDestroyOnLoad(this);
        }

        // Assign a starting score
        _score = 0;

    }

    // Update is called once per frame
    void Update()
    {


        // Check if 'Enter' was pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // If player is on 'Screen_Title' (Scene Name)
            if (SceneManager.GetActiveScene().name == "TitleScreen")
            {
                // Go to 'Level1' Scene
                // - Scene must be loaded in Build Settings or it will not work
                // - Build Settings are located at Menu Bar: Edit->Build Settings
                // - Drag the Scenes in the project into 'Scenes in Build' space
                SceneManager.LoadScene("Scene1");

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If player is on 'Screen_Title' (Scene Name)
            if (SceneManager.GetActiveScene().name == "Scene1")
            {
                // Go to 'Level1' Scene
                // - Scene must be loaded in Build Settings or it will not work
                // - Build Settings are located at Menu Bar: Edit->Build Settings
                // - Drag the Scenes in the project into 'Scenes in Build' space
                SceneManager.LoadScene("TitleScreen");

            }
        }


    }

    // Give access to private variables (instance variables)
    // - Not needed if using public variables
    // - Variable must be declared above
    // - Variable and method must be static
    public static GameManager instance
    {
        get { return _instance; }   // can also use just 'get;'
        set { _instance = value; }  // can also use just 'set;'
    }

    // Give access to private variables (instance variables)
    // - Not needed if using public variables
    public int score
    {
        get { return _score; }      // can also use just 'get;'
        set
        {
            _score = value;       // can also use just 'set;'

            // Check if 'scoreText' was set before trying to update HUD
            if (ScoreText)
                // Update HUD on every score change
                ScoreText.text = "Score: " + score;
        }
    }

}
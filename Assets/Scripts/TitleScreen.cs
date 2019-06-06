using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    public GameObject selector;


	// Use this for initialization
	void Start () {

        selector = GameObject.FindGameObjectWithTag("selector");

    }
	
	// Update is called once per frame
	void Update () {
     

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selector.transform.position = new Vector2(-1.892f, -0.888f);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selector.transform.position = new Vector2(-1.892f, -0.888f);
        }

    }
}

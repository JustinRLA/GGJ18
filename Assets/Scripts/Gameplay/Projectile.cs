using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int scoreToAdd = 0;
    public int comboMultiplier = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        //print("I hit something");
        if (collision.gameObject.tag == "Survivor")
        {
            //print("It's a survivor!");
            collision.gameObject.GetComponent<AI_Function>().state = 2;
            collision.gameObject.tag = "Infected";
        }
    }
}

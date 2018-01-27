using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    private float leftMove;
    private float leftRotate;
    private float rightMove;
    private float rightRotate;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Movement
        leftMove = Input.GetAxis("LeftMove") * Time.deltaTime * 5.0f;
        leftRotate = Input.GetAxis("LeftMove") * Time.deltaTime * 150.0f;
        rightMove = Input.GetAxis("RightMove") * Time.deltaTime * 5.0f;
        rightRotate = Input.GetAxis("RightMove") * Time.deltaTime * -150.0f;

        transform.Translate(0, 0, (leftMove + rightMove));
        transform.Rotate(0, (leftRotate + rightRotate)/5,0);
    }
}

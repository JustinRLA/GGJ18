﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public Transform targetCameraPosition;
    public Transform player;
    public float speed;
    public bool resetCameraPos;
    public float distanceFromPlayer;
    public float maxDistanceFromPlayer = 10;
    public float distanceFromTarget;

    public Quaternion originalRotationValue;
    public Rigidbody camRB;


    // Use this for initialization
    void Start ()
    {
        resetCameraPos = false;
        originalRotationValue = transform.rotation;
        camRB = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        distanceFromTarget = Vector3.Distance(targetCameraPosition.position, transform.position);
        
        if (distanceFromPlayer > maxDistanceFromPlayer)
        {
            camRB.isKinematic = true;
            transform.position = targetCameraPosition.position;
            //transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time);
            //resetCameraPos = false;
            camRB.isKinematic = false;
        }
        if (resetCameraPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetCameraPosition.position, step);
        }

        transform.LookAt(player);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        resetCameraPos = false;
        print("collision enter");
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        resetCameraPos = true;
        print("collision exit");
    }
}

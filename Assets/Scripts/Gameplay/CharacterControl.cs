using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    private float leftMove;
    private float leftRotate;
    private float rightMove;
    private float rightRotate;

    public GameObject coughPrefab;
    public Transform coughSpawn;
    public float coughProjectileSpeed = 10;
    public float coughProjectileDuration = 2.0f;

    Terrain_Function myTerrain;

    // Use this for initialization
    void Start () {
        myTerrain = Terrain.Get(gameObject);
    }
	
	// Update is called once per frame
	void Update () {

        //Movement
        leftMove = Input.GetAxis("LeftMove") * Time.deltaTime * 5.0f * myTerrain.MovementPercentPlayer();
        leftRotate = Input.GetAxis("LeftMove") * Time.deltaTime * 150.0f * myTerrain.MovementPercentPlayer();
        rightMove = Input.GetAxis("RightMove") * Time.deltaTime * 5.0f * myTerrain.MovementPercentPlayer();
        rightRotate = Input.GetAxis("RightMove") * Time.deltaTime * -150.0f * myTerrain.MovementPercentPlayer();

        transform.Translate(0, 0, (leftMove + rightMove));
        transform.Rotate(0, (leftRotate + rightRotate)/5,0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cough();
            //Manager_Effect.Manager.Call_Spit();
        }
    }

    void Cough()
    {
        // Create the Cough from the Bullet Prefab
        var cough = (GameObject)Instantiate(
            coughPrefab,
            coughSpawn.position,
            coughSpawn.rotation);

        // Add velocity to the cough
        cough.GetComponent<Rigidbody>().velocity = cough.transform.forward * coughProjectileSpeed;

        // Destroy the cough after 2 seconds
        Destroy(cough, coughProjectileDuration);
    }
}

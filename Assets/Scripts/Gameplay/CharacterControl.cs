using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    private float leftMove;
    private float leftRotate;
    private float rightMove;
    private float rightRotate;
    public SpriteRenderer LeftForward;
    public SpriteRenderer RightForward;
    public SpriteRenderer LeftBackward;
    public SpriteRenderer RightBackward;

    public GameObject coughPrefab;
    public Transform coughSpawn;
    public float coughProjectileSpeed = 10;
    public float coughProjectileDuration = 2.0f;

    public float coughCharge;
    public float coughChargeFull = 100.0f;
    public float coughChargeRate = 1.0f;

    Terrain_Function myTerrain;

    public Animator bodyAnimator;
    public Animator headAnimator;

    // Use this for initialization
    void Start () {
        myTerrain = Terrain.Get(gameObject);
        coughCharge = 0.0f;
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

        //Coughing

        if (Input.GetKeyDown(KeyCode.Space) && coughCharge >= coughChargeFull)
        {
            Cough();
            Manager_Effect.Manager.Call_Spit();
            coughCharge = 0.0f;
        }

        //Cough Recharge
        if(coughCharge < coughChargeFull)
        {
            coughCharge += coughChargeRate;
            if(coughCharge >= coughChargeFull * 0.8)
            {
                //Play readying to shoot
            }
        }
        else
        {
            //Set ready to shoot animation
        }


        //HUD Indicators
        //W Control Indicator
        if (Input.GetAxis("LeftMove") > 0)
            LeftForward.color = Color.green;
        else
            LeftForward.color = Color.white;
        //O Control Indicator
        if (Input.GetAxis("RightMove") > 0)
            RightForward.color = Color.green;
        else
            RightForward.color = Color.white;
        //X Control Indicator
        if (Input.GetAxis("LeftMove") < 0)
            LeftBackward.color = Color.green;
        else
            LeftBackward.color = Color.white;
        //M Control Indicator
        if (Input.GetAxis("RightMove") < 0)
            RightBackward.color = Color.green;
        else
            RightBackward.color = Color.white;



        //Animation Control
        if (Input.GetAxis("LeftMove") != 0 || Input.GetAxis("RightMove") != 0)
        {
            bodyAnimator.SetFloat("Blend", 1);
        }
        else
        {
            bodyAnimator.SetFloat("Blend", 0);
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

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

    public GameObject RandomSound;

    public bool inSand = false;
    public string currentTerrain = "Stone";
    public bool stepNoiseOnCooldown = false;
    public bool gargleNoiseOnCooldown = false;

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
            bodyAnimator.SetTrigger("Spit");
            headAnimator.SetTrigger("Spit");

            StartCoroutine(SpitSound(0.2f));

            Manager_Effect.Manager.Call_Spit();
            coughCharge = 0.0f;
        }

        //Cough Recharge
        if(coughCharge < coughChargeFull)
        {
            coughCharge += coughChargeRate;
            if(coughCharge >= coughChargeFull * 0.8)
            {
                //Readying to shoot
                bodyAnimator.SetBool("Charge", true);
                headAnimator.SetBool("Charge", true);

                if (!gargleNoiseOnCooldown)
                {
                    RandomSound.GetComponent<RandomFX_Gargle>().PlayFootstepStone();
                    StartCoroutine(GargleCooldown(0.5f));
                }
            }
        }
        else
        {
            //Set ready to shoot animation
            bodyAnimator.SetBool("Charge", false);
            headAnimator.SetBool("Charge", false);
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
            //Walking animation
            bodyAnimator.SetFloat("Blend", 1);
            headAnimator.SetFloat("Blend", 1);

            //Play footsteps
            if (!stepNoiseOnCooldown)
            {
                if (inSand)
                {
                    RandomSound.GetComponent<RandomFX_Footstep_Sand>().PlayFootstepStone();
                }
                else if (currentTerrain == "Stone")
                {
                    RandomSound.GetComponent<RandomFX_Footstep_Stone>().PlayFootstepStone();
                }
                else
                {
                    RandomSound.GetComponent<RandomFX_Footstep_Grass>().PlayFootstepStone();
                }
                StartCoroutine(WalkCooldown(0.35f));
            }
        }
        else
        {
            bodyAnimator.SetFloat("Blend", 0);
            headAnimator.SetFloat("Blend", 0);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sand")
        {
            print("I am in sand");
            inSand = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sand")
        {
            print("Sand was too coarse");
            inSand = false;
            if(currentTerrain == "Stone")
            {
                print("I am ROCK HARD");
            }
            else
            {
                print("My ass is grass");
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Stone" && !inSand)
        {
            print("I am ROCK HARD");
            currentTerrain = "Stone";
        }
        else if (collision.gameObject.tag == "Grass" && !inSand)
        {
            print("My ass is grass");
            currentTerrain = "Grass";
        }
    }

    IEnumerator WalkCooldown(float waitTime)
    {
        stepNoiseOnCooldown = true;
        yield return new WaitForSeconds(waitTime);
        stepNoiseOnCooldown = false;
    }

    IEnumerator GargleCooldown(float waitTime)
    {
        gargleNoiseOnCooldown = true;
        yield return new WaitForSeconds(waitTime);
        gargleNoiseOnCooldown = false;
    }

    IEnumerator SpitSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RandomSound.GetComponent<RandomFX_Spit>().PlayFootstepStone();
    }
}

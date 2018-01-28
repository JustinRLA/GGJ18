using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Spit : MonoBehaviour {

	public GameObject spitCube;
	public Transform splatTransform;
	public SpriteRenderer splatSprite;

	public float forceForwardMin = 50f;
	public float forceForwardMax = 300f;
	public float forceUpMin = 50f;
	public float forceUpMax = 500f;
	public float forceDirectionSpread = 30;
	public float spriteSizeMin = 0.3f;
	public float spriteSizeMax = 0.6f;
	public float spriteColorAlphaLevelMin = 1f;
	public float spriteColorAlphaLevelMax = 0.9f;

	public bool instantGround = false;

	public int state;
	Transform myTransform;
	Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("_Effect Manager").transform;
		myTransform = transform;
		myRigidbody = GetComponent<Rigidbody>();
		if(instantGround == true){
			LayerMask detectLayers = 1 << 8;
			RaycastHit hit;
			if(Physics.Raycast(new Vector3(myTransform.position.x, myTransform.position.y + 1f, myTransform.position.z),
			                   Vector3.down, out hit, 100, detectLayers)){
				Debug.Log("WORLK");
				if(hit.collider != null){
					myTransform.position = new Vector3(myTransform.position.x,hit.point.y + 0.01f,myTransform.position.z);
				}
			}
		}
	}

	float TIMER;
	// Update is called once per frame
	void Update () {
		switch(state){
		case 0:
			if(forceDirectionSpread != 0){
				if(forceDirectionSpread == 360){
					myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x,
					                                      Random.Range(0,360),
					                                      myTransform.eulerAngles.z);
				}
				else{
					myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x,
					                                      myTransform.eulerAngles.y + Random.Range(-forceDirectionSpread,forceDirectionSpread),
					                                      myTransform.eulerAngles.z);
				}
			}
			splatSprite.color = new Color(splatSprite.color.r * Random.Range(1,0.9f),splatSprite.color.g * Random.Range(1,0.9f),splatSprite.color.b * Random.Range(1,0.9f),splatSprite.color.a * Random.Range(spriteColorAlphaLevelMin,spriteColorAlphaLevelMax));
			splatSprite.transform.localScale = new Vector3(Random.Range(spriteSizeMin,spriteSizeMax),Random.Range(spriteSizeMin,spriteSizeMax),splatSprite.transform.localScale.z);
			splatSprite.transform.eulerAngles = new Vector3(90,Random.Range(0,360),0);
			myRigidbody.AddForce(myTransform.forward * Random.Range(forceForwardMin,forceForwardMax));
			myRigidbody.AddForce(myTransform.up * Random.Range(forceUpMin,forceUpMax));
			state++;
			break;
		case 1:
			if(myRigidbody.velocity.y > -0.1f && myRigidbody.velocity.y < 0.1f){
				spitCube.SetActive(false);
				TIMER = Time.time + 5f;
				state++;
			}
			break;
		case 2:
			if(Time.time >= TIMER){
				state++;
			}
			if(splatTransform.localScale.x < 1){
				splatTransform.localScale = new Vector3(
					splatTransform.localScale.x + 0.24f,
					splatTransform.localScale.y + 0.24f,
					splatTransform.localScale.z + 0.24f);
				if(splatTransform.localScale.x >= 1){
					splatTransform.localScale = new Vector3(1,1,1);
				}
			}
			if(myRigidbody.velocity.y > 0.1f || myRigidbody.velocity.y < -0.1f){
				splatTransform.localScale = new Vector3(0,0,0);
				spitCube.SetActive(true);
				state--;
			}
			break;
		case 3:
			splatSprite.transform.parent = GameObject.Find("_Effect Manager").transform;
			Destroy(this.gameObject);
			state++;
			break;
		}

	}
}

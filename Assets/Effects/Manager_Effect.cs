using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Effect : MonoBehaviour {
	public static Manager_Effect Manager;
	void Awake(){
		Manager = GetComponent<Manager_Effect>();
	}


	public Transform playerSpitTransform;

	// Use this for initialization
	void Start () {
		
	}

	public void Call_CameraShake(){
		Game_UI.Manager.Call_CameraShake();
	}
	public GameObject prefabSpitPuddle;
	public void Call_SpitPuddle(ref float timer, Transform myTransform){
		if(Time.time < timer){return;}
		Instantiate(prefabSpitPuddle, myTransform.position, myTransform.rotation);
		timer = Time.time + 0.2f;
	}
	public GameObject prefabSpitEffect;
	public void Call_Spit(){
		for(int i = 0; i < 10; i++){
			Instantiate(prefabSpitEffect, playerSpitTransform.position, playerSpitTransform.rotation);
		}
	}
	public GameObject prefabSplatConvertEffect;
	public void Call_AIConvert(Transform myTransform){
		for(int i = 0; i < 10; i++){
			Instantiate(prefabSplatConvertEffect, myTransform.position, myTransform.rotation);
		}
	}
	public GameObject prefabSplatSpreadEffect;
	public void Call_ScatterSpread(){
		for(int i = 0; i < 5; i++){
			Instantiate(prefabSplatSpreadEffect, playerSpitTransform.position, playerSpitTransform.rotation);
		}
	}

}

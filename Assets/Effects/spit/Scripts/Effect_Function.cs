using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Function : MonoBehaviour {

	public bool CallSpitEffect;
	int spitAmount = 10;
	public GameObject prefabSpitEffect;

	Transform myTransform;
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(CallSpitEffect){
			Manager_Effect.Manager.Call_Spit();
			CallSpitEffect = false;
		}
	}
}

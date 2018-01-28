using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFX : MonoBehaviour {
	public static RandomFX Manager;
	void Awake(){
		Manager = GetComponent<RandomFX>();
	}
}

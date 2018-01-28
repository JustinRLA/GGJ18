using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Trigger_Terrain : MonoBehaviour {
	public int movementSpeedPercent = 50;
	void start(){
		if(movementSpeedPercent < 0){movementSpeedPercent = 0;}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Trigger_Terrain : MonoBehaviour {
	public int movementSpeedPercentNPC = 100;
    public int movementSpeedPercentPlayer = 100;

	void start(){
		if(movementSpeedPercentNPC < 0){movementSpeedPercentNPC = 0;}
        if (movementSpeedPercentPlayer < 0) { movementSpeedPercentPlayer = 0; }
    }
}

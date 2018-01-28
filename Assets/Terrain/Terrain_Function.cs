using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Terrain{
	public static Terrain_Function Get(GameObject thisGameObject){
		return thisGameObject.AddComponent<Terrain_Function>() as Terrain_Function;
	}
}

public class Terrain_Function : MonoBehaviour {

	public float MovementPercentNPC(){
		if(movementSpeedPercentNPC == 0){return 1;}
		float _result_f = movementSpeedPercentNPC * 0.01f;
		return _result_f;
	}

    public float MovementPercentPlayer()
    {
        if (movementSpeedPercentPlayer == 0) { return 1; }
        float _result_f = movementSpeedPercentPlayer * 0.01f;
        return _result_f;
    }

    int movementSpeedPercentNPC;
    int movementSpeedPercentPlayer;

    _Trigger_Terrain myTrigger;
	LayerMask detectLayers = 1 << 11;
	Transform myTransform;
	void Start(){
		myTransform = transform;
	}
	float FPS;
	void LateUpdate(){
		if(Time.time < FPS){return;}
		RaycastHit hit;
		if(Physics.Raycast(new Vector3(myTransform.position.x, myTransform.position.y + 1f, myTransform.position.z),
		                   Vector3.down, out hit, 2, detectLayers)){
			if(hit.collider == null){return;}
			if(myTrigger != hit.collider.GetComponent<_Trigger_Terrain>()){
				myTrigger = hit.collider.GetComponent<_Trigger_Terrain>();
			}
			if(movementSpeedPercentNPC != myTrigger.movementSpeedPercentNPC){
				movementSpeedPercentNPC = myTrigger.movementSpeedPercentNPC;
			}
            if (movementSpeedPercentPlayer != myTrigger.movementSpeedPercentPlayer)
            {
                movementSpeedPercentPlayer = myTrigger.movementSpeedPercentPlayer;
            }
        }
		else {
			if(myTrigger != null){myTrigger = null;}
			if(movementSpeedPercentNPC != 0){movementSpeedPercentNPC = 0;}
            if (movementSpeedPercentPlayer != 0) { movementSpeedPercentPlayer = 0; }
        }
		FPS = Time.time + 0.2f;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI{
	public static void Set_Player(GameObject player){
		AI_Function.targetTransform = player.transform;
	}
}

public class AI_Function : MonoBehaviour {
	
	[System.Serializable]
	public class _STAT{
		[System.Serializable]
		public class _NORMAL{
			public float ReactionRadius = 10f;
			public float movementSpeedMin = 0.8f;
			public float movementSpeedMax = 1.2f;
			public float movementDurationMin = 1;
			public float movementDurationMax = 10;
			public float IdleDurationMin = 2;
			public float IdleDurationMax = 4;
		}
		public _NORMAL NORMAL;
		[System.Serializable]
		public class _RUN{
			public float ReactionRadius = 10f;
			public float reactionJumpForce = 200f;
			public float movementSpeedMin = 3f;
			public float movementSpeedMax = 6f;
		}
		public _RUN RUN;
		[System.Serializable]
		public class _PANIC{
			public float ReactionRadius = 10f;
			public float panicDurationMin = 2;
			public float panicDurationMax = 10f;
			public float movementSpeedMin = 3f;
			public float movementSpeedMax = 6f;
			public float movementDurationMin = 0.2f;
			public float movementDurationMax = 1f;
			public float IdleDurationMin = 0.1f;
			public float IdleDurationMax = 0.4f;
		}
		public _PANIC PANIC;
		[System.Serializable]
		public class _FOLLOW_LAMA{
			public float ReactionRadius = 10f;
			public float reactionJumpForce = 200f;
			public float movementSpeedMin = 1f;
			public float movementSpeedMax = 2f;
		}
		public _FOLLOW_LAMA FOLLOW_LAMA;
	}
	public _STAT STAT;
	public Transform _targetTransform;
	public static Transform targetTransform;
	float myTransform_Direction;
	Transform myTransform;
	Rigidbody myRigidbody;
	Terrain_Function myTerrain;

    //Justin's variables
    public Animator bodyAnimator;
    public Animator headAnimator;
	public Animator headLamaAnimator;
    
	float VFX_Convert_TIMER;
	public GameObject[] VFX_Convert = new GameObject[2];

    [HideInInspector] public GameObject PanicTrigger;
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("_Pool AI").transform;
		gameObject.layer = LayerMask.NameToLayer("AI");
		myTransform = transform;
		myRigidbody = GetComponent<Rigidbody>();
		PanicTrigger.SetActive(false);
		myTerrain = Terrain.Get(gameObject);
		headLamaAnimator.gameObject.SetActive(false);
		for(int i = 0; i < VFX_Convert.Length; i++){VFX_Convert[i].SetActive(false);}
	}

	public int state;
	// Update is called once per frame
	void Update () {
		if(state > 0 && targetTransform == null){return;}
		if(_targetTransform != targetTransform){_targetTransform = targetTransform;}
		switch(state){
		case 0://get ready
			Function_WallCheck();
			State_Normal();
			if(Game_Logic.Manager.state > 0){
				state++;
			}
			break;
		case 1://start
			Update_CharacterState_AI();
			break;
		case 2://coverted to lama
			Update_CharacterState_AI_Lama();
			if(Game_Logic.Manager.state > 1){
				state++;
			}
			break;
		case 3://gameover
			Function_WallCheck();
			State_Normal();
			break;
		}
	}
	
	void Update_CharacterState_AI_Lama(){
		Function_WallCheck();
		if(PanicTrigger.activeSelf != true){PanicTrigger.SetActive(true);}
		switch(characterState){
		default:
			Manager_Effect.Manager.Call_AIConvert(myTransform);
			gameObject.layer = LayerMask.NameToLayer("player");
			Game_Logic.Manager.Info.Score++;
			headLamaAnimator.gameObject.SetActive(true);
			headAnimator.gameObject.SetActive(false);
			bodyAnimator.SetBool("detectLama", false);
			headAnimator.SetBool("detectLama", false);
			for(int i = 0; i < VFX_Convert.Length; i++){VFX_Convert[i].SetActive(true);}
			VFX_Convert_TIMER = Time.time + 10f;
			characterState = "chilling";
			break;
		case "chilling":
			State_Chilling_Lama();
			break;
		case "follow":
			State_Follow_Lama();
			break;
		}
		if(VFX_Convert_TIMER != 0){
			if(Time.time >= VFX_Convert_TIMER){
				for(int i = 0; i < VFX_Convert.Length; i++){VFX_Convert[i].SetActive(false);}
				VFX_Convert_TIMER = 0;
			}
		}
	}
	void State_Chilling_Lama(){
		if(characterState_TOGGLE != characterState){
			CharacterState_TIMER = 0;
			State_Normal_STATE = 0;
			bodyAnimator.SetBool("detectLama", false);
			headLamaAnimator.SetBool("detectLama", false);
			characterState_TOGGLE = characterState;
		}
		if(Vector3.Distance(myTransform.position, targetTransform.position) > STAT.FOLLOW_LAMA.ReactionRadius){
			characterState = "follow";
		}
		switch(State_Normal_STATE){
		case 0:
			if(Time.time >= CharacterState_TIMER){
                CharacterState_MOVEMENTSPEED = Random.Range(STAT.NORMAL.movementSpeedMin,STAT.NORMAL.movementSpeedMax);
				CharacterState_TIMER = Time.time + Random.Range(STAT.NORMAL.IdleDurationMin,STAT.NORMAL.IdleDurationMax);
				myTransform_Direction = myTransform.eulerAngles.y + Random.Range(-90,90);
				State_Normal_STATE++;
			}
			bodyAnimator.SetBool("Walk", false);
			headLamaAnimator.SetBool("Walk", false);
			break;
		case 1:
			if(myTransform.eulerAngles.y != myTransform_Direction){myTransform.eulerAngles = new Vector3(0,myTransform_Direction,0);}
			MoveForward(CharacterState_MOVEMENTSPEED);
			if(Time.time >= CharacterState_TIMER){
				CharacterState_TIMER = Time.time + Random.Range(STAT.NORMAL.movementDurationMin,STAT.NORMAL.movementDurationMin);
				State_Normal_STATE++;
			}
			bodyAnimator.SetBool("Walk", true);
			headLamaAnimator.SetBool("Walk", true);
			break;
		case 2:
			State_Normal_STATE = 0;
			break;
		}
	}
	void State_Follow_Lama(){
		if(characterState_TOGGLE != characterState){
			State_Running_STATE = 0;
			CharacterState_TIMER = 0;
			State_Running_TIMER = Time.time + 2f;
			CharacterState_MOVEMENTSPEED = Random.Range(STAT.FOLLOW_LAMA.movementSpeedMin,STAT.FOLLOW_LAMA.movementSpeedMax);
			bodyAnimator.SetBool("detectLama", true);
			headLamaAnimator.SetBool("detectLama", true);
			bodyAnimator.SetBool("Walk", true);
			headLamaAnimator.SetBool("Walk", true);
			characterState_TOGGLE = characterState;
		}
		switch(State_Running_STATE){
		case 0:
			LookAt(targetTransform);
			myRigidbody.AddForce(Vector3.up * STAT.FOLLOW_LAMA.reactionJumpForce);
			State_Running_STATE++;
			break;
		case 1:
			LookAt(targetTransform);
			if(myRigidbody.velocity.y < 0.1f && myRigidbody.velocity.y > -0.1f){
				State_Running_STATE++;
			}
			break;
		case 2:
			LookAt(targetTransform);
			MoveForward(CharacterState_MOVEMENTSPEED);
			if(Time.time < State_Running_TIMER){return;}
			if(Vector3.Distance(myTransform.position, targetTransform.position) <= STAT.FOLLOW_LAMA.ReactionRadius){
				characterState = "chilling";
			}
			break;
		}
	}

	public string characterState;
	string characterState_TOGGLE;
	float CharacterState_TIMER;
	float CharacterState_MOVEMENTSPEED;
	void Update_CharacterState_AI(){
		Function_WallCheck();
		switch(characterState){
		default:
			characterState = "normal";
			break;
		case "normal":
			State_Normal();
			break;
		case "running":
			State_Running();
			break;
		case "panic":
			State_Panic();
			break;
		}
	}

	int State_Normal_STATE;
	void State_Normal(){
		if(characterState_TOGGLE != characterState){
			CharacterState_TIMER = 0;
			State_Normal_STATE = 0;
			PanicTrigger.SetActive(false);
			bodyAnimator.SetBool("detectLama", false);
			headAnimator.SetBool("detectLama", false);
			characterState_TOGGLE = characterState;
		}

		switch(State_Normal_STATE){
		case 0:
			if(Time.time >= CharacterState_TIMER){
				CharacterState_MOVEMENTSPEED = Random.Range(STAT.NORMAL.movementSpeedMin,STAT.NORMAL.movementSpeedMax);
				CharacterState_TIMER = Time.time + Random.Range(STAT.NORMAL.IdleDurationMin,STAT.NORMAL.IdleDurationMax);
				myTransform_Direction = Random.Range(0,360);
				State_Normal_STATE++;
			}
			bodyAnimator.SetBool("Walk", false);
			headAnimator.SetBool("Walk", false);
			break;
		case 1:

			if(myTransform.eulerAngles.y != myTransform_Direction){myTransform.eulerAngles = new Vector3(0,myTransform_Direction,0);}
			MoveForward(CharacterState_MOVEMENTSPEED);
			if(Time.time >= CharacterState_TIMER){
				CharacterState_TIMER = Time.time + Random.Range(STAT.NORMAL.movementDurationMin,STAT.NORMAL.movementDurationMin);
				State_Normal_STATE++;
			}
			bodyAnimator.SetBool("Walk", true);
			headAnimator.SetBool("Walk", true);
			break;
		case 2:
			State_Normal_STATE = 0;
			break;
		}
		if(state == 0){return;}
		if(Vector3.Distance(myTransform.position, targetTransform.position) < STAT.NORMAL.ReactionRadius){
			characterState = "running";
		}
	}

	int State_Running_STATE;
	float State_Running_TIMER;
	void State_Running(){
		if(characterState_TOGGLE != characterState){
			if(characterState_TOGGLE == "panic"){
				State_Running_STATE = 2;
			}
			else{
				State_Running_STATE = 0;
			}
			CharacterState_TIMER = 0;
			State_Running_TIMER = Time.time + 1f;
			CharacterState_MOVEMENTSPEED = Random.Range(STAT.RUN.movementSpeedMin,STAT.RUN.movementSpeedMax);
			PanicTrigger.SetActive(true);
			bodyAnimator.SetBool("detectLama", true);
			headAnimator.SetBool("detectLama", true);
			bodyAnimator.SetBool("Walk", true);
			headAnimator.SetBool("Walk", true);
			characterState_TOGGLE = characterState;
		}
		switch(State_Running_STATE){
		case 0:
			LookAt(targetTransform);
			myRigidbody.AddForce(Vector3.up * STAT.RUN.reactionJumpForce);
			State_Running_STATE++;
			break;
		case 1:
			LookAt(targetTransform);
			if(myRigidbody.velocity.y < 0.1f && myRigidbody.velocity.y > -0.1f){
				State_Running_STATE++;
			}
			break;
		case 2:
			LookAway(targetTransform);
			MoveForward(CharacterState_MOVEMENTSPEED);
			if(Time.time <= State_Running_TIMER){return;}
			if(Vector3.Distance(myTransform.position, targetTransform.position) >= STAT.RUN.ReactionRadius){
				characterState = "panic";
            }
			break;
		}
	}

	int State_Panic_STATE;
	float State_Panic_TIMER;
	void State_Panic(){
		if(characterState_TOGGLE != characterState){
			State_Panic_TIMER = Time.time + Random.Range(STAT.PANIC.panicDurationMin,STAT.PANIC.panicDurationMax);
			CharacterState_TIMER = 0;
			State_Panic_STATE = 0;
			PanicTrigger.SetActive(true);
			bodyAnimator.SetBool("detectLama", false);
			headAnimator.SetBool("detectLama", false);
			characterState_TOGGLE = characterState;
		}
		if(Vector3.Distance(myTransform.position, targetTransform.position) < STAT.PANIC.ReactionRadius){
			characterState = "running";
        }
		if(Time.time >= State_Panic_TIMER){
			characterState = "normal";
        }
		switch(State_Panic_STATE){
		case 0:
			if(Time.time >= CharacterState_TIMER){
				CharacterState_MOVEMENTSPEED = Random.Range(STAT.PANIC.movementSpeedMin,STAT.PANIC.movementSpeedMax);
				CharacterState_TIMER = Time.time + Random.Range(STAT.PANIC.IdleDurationMin,STAT.PANIC.IdleDurationMax);
				myTransform.eulerAngles = new Vector3(0, myTransform.eulerAngles.y + Random.Range(-90,90),0);
				State_Panic_STATE++;
			}
			bodyAnimator.SetBool("Walk", false);
			headAnimator.SetBool("Walk", false);
			break;
		case 1:
			MoveForward(CharacterState_MOVEMENTSPEED);
			if(Time.time >= CharacterState_TIMER){
				CharacterState_TIMER = Time.time + Random.Range(STAT.PANIC.movementDurationMin,STAT.PANIC.movementDurationMax);
				State_Panic_STATE++;
			}
			bodyAnimator.SetBool("Walk", true);
			headAnimator.SetBool("Walk", true);
			break;
		case 2:
			State_Panic_STATE = 0;
			break;
		}
	}

	[System.Serializable]
	public class _RayCheck{
		public Transform start;
	}
	public _RayCheck[] RayCheck = new _RayCheck[2];
	LayerMask detectLayers = 1 << 8;
	int Function_WallCheck_DIRECTION; int Function_WallCheck_DIRECTION_CURRENT;
	float Function_WallCheck_TIMER;
	void Function_WallCheck(){
		Function_WallCheck_DIRECTION = 0;
		for(int i = 0; i < RayCheck.Length; i++){
			Debug.DrawRay(RayCheck[i].start.position,RayCheck[i].start.TransformDirection(Vector3.forward) * 0.5f,Color.green);
			RaycastHit hit;
			if (Physics.Raycast(RayCheck[i].start.position, RayCheck[i].start.TransformDirection(Vector3.forward), out hit, 0.5f, detectLayers)){
				if(Function_WallCheck_DIRECTION != 0){
					if(Random.Range(-100,100) <= 0){
						Function_WallCheck_DIRECTION = -1;
					}
					else{
						Function_WallCheck_DIRECTION = 1;
					}
				}
				switch(i){
				case 0:
					Function_WallCheck_DIRECTION = -1;
					break;
				case 1:
					Function_WallCheck_DIRECTION = 1;
					break;
				}
			}
		}
		switch(Function_WallCheck_DIRECTION){
		case -1:
			myTransform.eulerAngles = new Vector3(0,myTransform.eulerAngles.y - 65f, 0);
			break;
		case 1:
			myTransform.eulerAngles = new Vector3(0,myTransform.eulerAngles.y + 65f, 0);
			break;
		}
		if(Function_WallCheck_DIRECTION != 0){
			Function_WallCheck_TIMER = Time.time + 0.4f;
		}
	}

	void MoveForward(float speed){
		myTransform.Translate(Vector3.forward * (speed * myTerrain.MovementPercentNPC()) * Time.deltaTime);
    }
	void LookAt(Transform target) {
		if(Time.time < Function_WallCheck_TIMER){return;}
		myTransform.LookAt(new Vector3(target.position.x,0,target.position.z));
		myTransform.eulerAngles = new Vector3(0,myTransform.eulerAngles.y,0);
	}
	void LookAway(Transform target) {
		if(Time.time < Function_WallCheck_TIMER){return;}
		Vector3 _mypos_v3 = new Vector3(myTransform.position.x,0,myTransform.position.z);
		Vector3 _targetpos_v3 = new Vector3(target.position.x,0,target.position.z);
		myTransform.LookAt(2 * _mypos_v3 - _targetpos_v3);
		myTransform.eulerAngles = new Vector3(0,myTransform.eulerAngles.y,0);
	}

	void OnTriggerEnter(Collider other){
		if(state == 1){
			if(other.tag == "PANIC"){
				characterState = "running";
			}
		}
	}
//	void Targeting (float x, float z) {
//		float AngleRad = Mathf.Atan2(x - myTransform.position.x, z - myTransform.position.z);
//		float AngleDeg = (180 / Mathf.PI) * AngleRad;
//		myTransform.rotation = Quaternion.Euler(0, AngleDeg, 0);
//	}
}

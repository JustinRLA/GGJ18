using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_UI : MonoBehaviour {
	public static Game_UI Manager;
	void Awake(){
		Manager = GetComponent<Game_UI>();
	}

	public TextMesh scoreText;
	public Transform scoreTransform; int scoreTransform_STATE;

	public TextMesh timeText;
	public Transform timeTransform; int timeTransform_STATE;

	// Use this for initialization
	void Start () {
		mainCameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Game_Logic.Manager.state == 0){return;}
		if(scoreTransform_STATE != 2){
			if(Pop(ref scoreTransform_STATE,scoreTransform) == false){}
		}
		if(timeTransform_STATE != 2){
			if(Pop(ref timeTransform_STATE,timeTransform) == false){}
		}
		if(scoreText.text != ""+Game_Logic.Manager.Info.Score){
			scoreTransform_STATE = 0;
			scoreText.text = ""+Game_Logic.Manager.Info.Score;
		}
		if(timeText.text != ""+Game_Logic.Manager.Info.Timer){
			if(Game_Logic.Manager.Info.Timer != 0){
				if(Game_Logic.Manager.Info.Timer < 20){
					if(timeText.color != Color.red){timeText.color = Color.red;}
					if(timeText.transform.localScale.x != 4){timeText.transform.localScale = new Vector3(4,4,4);}
					timeTransform_STATE = 0;
				}
				else{
					if(timeText.color != Color.white){timeText.color = Color.white;}
					if(timeText.transform.localScale.x != 2){timeText.transform.localScale = new Vector3(2,2,2);}
				}
			}
			timeText.text = ""+Game_Logic.Manager.Info.Timer;
		}
		CameraShakeFunction();
	}

	public void Call_CameraShake(){
		CameraShakeActivate = true;
	}
	public bool CameraShakeActivate = false;
	private bool shake = false;
	float shakeDecay = 0.001f;
	float shakeIntensity = 0.025f;
	float shake_decay;
	float shake_intensity;	
	Transform mainCameraTransform;
	Quaternion originShakeRotation;
	Vector3 originShakePosition;
	Quaternion originalPosition;
	void CameraShakeFunction(){
		if(Time.timeScale != 0){
			if(CameraShakeActivate == true){
				if(shake == false){
					originShakePosition = mainCameraTransform.position;
					originShakeRotation = mainCameraTransform.rotation;
					shake_intensity = shakeIntensity;
					shake_decay = shakeDecay;
					shake = true;
				}
				CameraShakeActivate = false;
			}
			if(shake == true){
				if(shake_intensity > 0){
					//mainCameraTransform.position = originShakePosition + Random.insideUnitSphere * shake_intensity
					mainCameraTransform.position = new Vector3(mainCameraTransform.position.x, originShakePosition.y + Random.insideUnitSphere.y * shake_intensity, mainCameraTransform.position.z);
					mainCameraTransform.rotation = new Quaternion(
						originShakeRotation.x + Random.Range(-shake_intensity,shake_intensity)*0.2f,
						originShakeRotation.y + Random.Range(-shake_intensity,shake_intensity)*0.2f,
						originShakeRotation.z + Random.Range(-shake_intensity,shake_intensity)*0.2f,
						originShakeRotation.w + Random.Range(-shake_intensity,shake_intensity)*0.2f);
					shake_intensity -= shake_decay;
				}
				else if(shake_intensity <= 0){
					shake = false;
				}
			}
		}
	}
	
	bool Pop(ref int state, Transform thisTransform){
		switch(state){
		case 0:
			if(thisTransform.localScale.x < 1.4f){
				thisTransform.localScale = new Vector3(
					thisTransform.localScale.x + 0.1f,
					thisTransform.localScale.y + 0.1f,
					thisTransform.localScale.z + 0.1f);
			}
			else{
				thisTransform.localScale = new Vector3(1.2f,1.2f,1.2f);
				state++;
			}
			break;
		case 1:
			if(thisTransform.localScale.x > 1f){
				thisTransform.localScale = new Vector3(
					thisTransform.localScale.x - 0.1f,
					thisTransform.localScale.y - 0.1f,
					thisTransform.localScale.z - 0.1f);
			}
			else{
				thisTransform.localScale = new Vector3(1.0f,1.0f,1.0f);
				state++;
			}
			break;
		case 2:return true;
		}
		return false;

	}
}

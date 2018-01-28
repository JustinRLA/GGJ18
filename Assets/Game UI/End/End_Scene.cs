using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Scene : MonoBehaviour {

	public SpriteRenderer spriteBackground;
	public SpriteRenderer spriteShine;
	public TextMesh[] textGrade;
	public TextMesh textScore;
	public GameObject[] lamaObject = new GameObject[3];

	int state;
	public int score;
	public int scoreLevelAverage;
	public int scoreLevelVictory;
	public string result;
	// Use this for initialization
	void Start () {
		score = ApplicationModel.playerScore;
		Call_ScreenTranition(true, -1);
		textScore.text = ""+score;
		for(int i = 0; i < lamaObject.Length; i++){
			lamaObject[i].SetActive(false);
		}
		if(score >= scoreLevelVictory){
			result = "victory";
		}
		else if(score >= scoreLevelAverage && score < scoreLevelVictory){
			result = "average";
		}
		else if(score < scoreLevelAverage){
			result = "defeat";
		}
	}

	public GameObject prefab_screenTransition;
	public void Call_ScreenTranition(bool toggle, int sceneNum){
		GameObject _inst = Instantiate(prefab_screenTransition, prefab_screenTransition.transform.position, prefab_screenTransition.transform.rotation) as GameObject;
		_inst.GetComponent<ScreenTransition>().transitionIn = toggle;
		_inst.GetComponent<ScreenTransition>().newSceneNumber = sceneNum;
		_inst.SetActive(true);
	}
	
	void Textthing(string t, Color c, float s){
		textGrade[0].transform.localScale = new Vector3(s,s,1);
		for(int i = 0; i < textGrade.Length; i++){
			textGrade[i].text = t;
		}
		textGrade[0].color = c;
		textGrade[1].color = Color.black;
	}

	float TIMER;
	// Update is called once per frame
	void Update () {
		if(state > 0){
			spriteShine.transform.Rotate(0,0,0.2f);
		}
		switch(state){
		case 0:
			switch(result){
			case "victory":
				Textthing("GREAT", Color.white, 3);
				spriteBackground.color = new Color32(255,255,0,255);
				spriteShine.color = new Color32(0,255,255,50);
				lamaObject[2].SetActive(true);
				break;
			case "average":
				Textthing("AVERAGE", Color.white, 2);
				spriteBackground.color = new Color32(255,255,255,255);
				spriteShine.color = new Color32(255,255,255,50);
				lamaObject[1].SetActive(true);
				break;
			case "defeat":
				Textthing("BAD", Color.white, 2);
				spriteBackground.color = new Color32(0,0,0,255);
				spriteShine.color = new Color32(0,0,0,50);
				lamaObject[0].SetActive(true);
				break;
			}
			TIMER = Time.time + 2f;
			state++;
			break;
		case 1:
			if(Time.time >= TIMER){
				TIMER = Time.time + 2f;
				state++;
			}
			break;
		case 2:
			if(Input.anyKeyDown){
				Call_ScreenTranition(false, 0);
				state++;
			}
			break;
		}
	}
}

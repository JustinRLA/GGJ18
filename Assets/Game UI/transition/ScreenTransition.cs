using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour {

	public GameObject[] spriteImages;

	public bool transitionIn;
	public int newSceneNumber;
	float TIMER;
	int num;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < spriteImages.Length; i++){
			spriteImages[i].SetActive(transitionIn);
		}
		switch(transitionIn){
		case true:
			num = spriteImages.Length-1;
			break;
		case false:
			num = 0;
			break;
		}
		TIMER = Time.time + 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(TIMER != 0){
			if(Time.time >= TIMER){
				switch(transitionIn){
				case true:
					spriteImages[num].SetActive(false);
					num--;
					if(num < 0){
						TIMER = 0;
						return;
					}
					break;
				case false:
					spriteImages[num].SetActive(true);
					num++;
					if(num >= spriteImages.Length){
						TIMER = 0;
						return;
					}
					break;
				}
				TIMER = Time.time + 0.1f;
			}
		}
		else{
			switch(transitionIn){
			case true:
				Destroy(gameObject);
				break;
			case false:
				if(newSceneNumber != -1){
					SceneManager.LoadScene(newSceneNumber);
				}
				break;
			}

		}
	}
}

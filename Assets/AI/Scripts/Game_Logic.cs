﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Logic : MonoBehaviour {
	public static Game_Logic Manager;
	void Awake(){
		Manager = GetComponent<Game_Logic>();
	}

	public bool startGame;
	public int state;
	public string gameState;
	public GameObject player;

	[System.Serializable]
	public class _STAT{
		public int TimerDuration = 30;
	}
	public _STAT STAT;
	[System.Serializable]
	public class _Info{
		public int Timer;
		public int Score;
	}
	public _Info Info;
	
	// Use this for initialization
	void Start () {
		if( player != null){
			AI.Set_Player(player);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		switch(state){
		case 0://waiting
			switch(gameState){
			default:
				gameState = "waiting";
				break;
			case "waiting":
				if(startGame){
					if(player == null){return;}
					gameState = "start";
				}
				break;
			case "start":
				AI.Set_Player(player);
				state++;
				break;
			}
			break;
		case 1://gameplay
			switch(gameState){
			default:
				gameState = "gameplay";
				break;
			case "gameplay":
				Function_GameTimer();
				if(Info.Timer <= 0){
					gameState = "score";
				}
				break;
			case "score":
				state++;
				break;
			}
			break;
		case 2://end
			switch(gameState){
			default:
				gameState = "gameover";
				break;
			case "gameover":
	
				break;
			}
			break;
		}
	}

	int Function_GameTimer_STATE;
	float Function_GameTimer_TIMER;
	void Function_GameTimer(){
		if(Function_GameTimer_STATE != 0){
			if(Info.Timer > 0){
				if(Time.time >= Function_GameTimer_TIMER){
					Info.Timer--;
					Function_GameTimer_TIMER = Time.time + 1;
				}
			}
		}
		switch(Function_GameTimer_STATE){
		case 0:
			Info.Timer = STAT.TimerDuration;
			Function_GameTimer_TIMER = Time.time + 1;
			Function_GameTimer_STATE++;
			break;
		case 1:

			break;
		}

	}
}
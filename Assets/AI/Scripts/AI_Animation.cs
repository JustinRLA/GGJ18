using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Animation : MonoBehaviour {

	public Animator headAnimator;

	[System.Serializable]
	public class _AnimatorPart{
		public Animator animator;
		[System.Serializable]
		public class _thisAnimation{
			public string animationType;
			public string animatorID;
		}
		public _thisAnimation[] thisAnimation;
	}
	public _AnimatorPart[] AnimatorPart;

	public int test;
	// Update is called once per frame
	public void Call_Animation(int partID, int animationID) {

	}
}

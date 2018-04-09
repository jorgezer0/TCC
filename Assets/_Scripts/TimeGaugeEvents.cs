using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGaugeEvents : MonoBehaviour {

	private Animator anim;
	private Image gaze;

	void Start(){
		anim = GetComponent<Animator> ();
		gaze = GetComponent<Image> ();
	}

	void Update(){
		if ((gaze.fillAmount == 1) && (!anim.GetBool ("full")) && (!anim.GetBool ("slow"))) {
			anim.SetBool ("full", true);
		}
	}

	public void Revert(){
		anim.SetFloat ("speed", -2f);
		anim.SetBool ("slow", false);
		TimeManager.instance.NormalTime ();
	}

	public void NotFull(){
		anim.SetBool ("full", false);
	}
}

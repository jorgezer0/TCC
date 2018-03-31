using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGaugeEvents : MonoBehaviour {

	private Animator anim;
	private Image gaze;
	private PlayerController player;

	void Start(){
		anim = GetComponent<Animator> ();
		gaze = GetComponent<Image> ();
		player = GetComponentInParent<PlayerController> ();
	}

	void Update(){
		if ((gaze.fillAmount < 0.05f) && (!anim.GetBool ("idle")) && (!anim.GetBool ("charge"))) {
			anim.SetBool ("idle", true);
		}
	}

	public void Revert(){
		anim.SetFloat ("speed", -2f / Time.timeScale);
		anim.SetBool ("idle", true);
		anim.SetBool ("charge", false);
		StartTeleport ();
	}

	public void StartTeleport(){
		player.StartTeleport();
	}

}

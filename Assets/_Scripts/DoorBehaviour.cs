using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DoorBehaviour : MonoBehaviour {

	Animator anim;
	public bool _open = false;
	public bool _broken = false;
	public bool autoClose = false;
	float distance;
	bool isOpen = false;

	public int pulses;
	public bool aditivePulses;
	private int receivedPulses;

	public bool isBetweenLevels = false;
	public Object nextLevel;
	Scene loadedScene;
	AsyncOperation loading;
	bool loaded = false;

	public float openPos = 0.89f;
	public float dur = 0.5f;
	public Transform[] doors;
	public Transform checkPoint;

	private bool locked = false;

	void Awake(){
		doors = GetComponentsInChildren<Transform> ();
	}

	void Start () {
		anim = GetComponent<Animator> ();
		if (_open) {
			if (_broken) {
				BreakeDoor ();
			} else {
				ChangeDoorState ();
				isOpen = true;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.C)) {
			ChangeDoorState ();
		}
	}

	public void ChangeDoorState(){
		if (!locked) {
			anim.SetTrigger ("change");
		}
	}

	public void BreakeDoor(){
		anim.SetBool ("broken", true);
	}

	public void FixDoor(){
		anim.SetBool ("broken", false);
	}

	public void ButtonBehaviour(int p){
		Debug.Log ("Pulse Received");
		if (aditivePulses) {
			receivedPulses += p;
		} else {
			receivedPulses = p;
		}

		if (receivedPulses >= pulses) {
			Debug.Log ("Open");
			if (_broken) {
				BreakeDoor ();
			} else {
				ChangeDoorState ();
				isOpen = true;
			}
		} else {
			if (isOpen) {
				isOpen = false;
				ChangeDoorState ();
			}
		}
	}

	public void SwitchOff(){
		ChangeDoorState ();
	}

	public void AutoClose(PlayerController player){
		Debug.Log ("Passed");

		if (_broken) {
			FixDoor ();
			locked = true;
		} else if (autoClose) {
			if (isOpen) {
				ChangeDoorState ();
				isOpen = false;
				player.checkPoint = checkPoint;
//				locked = true;
				Debug.Log ("Locked!");
			}
		}
	}

}


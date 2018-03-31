using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public static TimeManager instance;
	public float timeInSlow;

	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SlowTime(){
		Time.timeScale = 0.01f;
	}

	public void NormalTime(){
		Time.timeScale = 1;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformBehaviour : MonoBehaviour {

	public Transform platform;
	public Transform start;
	public Transform end;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((platform.localPosition - start.localPosition).magnitude < 0.05f) {
			platform.DOMove (end.position, speed);
		} else if ((platform.localPosition - end.localPosition).magnitude < 0.05f){
			platform.DOMove (start.position, speed);
		}
	}
}

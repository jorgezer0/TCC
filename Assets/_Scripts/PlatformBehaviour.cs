using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformBehaviour : MonoBehaviour {

	public Transform platform;
	public Transform start;
	public Transform end;
	public float speed;
	public float delay;

	// Use this for initialization
	void Start () {
		Go ();
	}

	void Go(){
		platform.DOMove (end.position, speed).OnComplete(() => {
			Back();
		});
	}

	void Back(){
		platform.DOMove (start.position, speed).SetDelay(delay).OnComplete(() => {
			Go();
		});
	}
}

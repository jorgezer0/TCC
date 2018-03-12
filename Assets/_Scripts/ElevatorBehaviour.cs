using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ElevatorBehaviour : MonoBehaviour {

	public Transform[] floors;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			GoTo (1);
		}
	}

	void GoTo(int floor){
		transform.DOMove (floors [floor].position, 3f);
	}
}

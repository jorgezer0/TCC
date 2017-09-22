using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {

	public NavMeshAgent nav;
	public GameObject player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		nav.destination = player.transform.position;
		Debug.Log (nav.hasPath);
	}
}

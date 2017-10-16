using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveAgent : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	// Use this for initialization
	void Start () {

		agent.destination = target.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

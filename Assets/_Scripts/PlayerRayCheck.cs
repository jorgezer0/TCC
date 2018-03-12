using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour {

	public AudioSource audioSource;
	public Transform player;
	public string hitName;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Physics.Raycast (transform.position, (player.position - transform.position), out hit, audioSource.maxDistance);
		Debug.DrawLine (transform.position, hit.point);
		hitName = hit.collider.tag;
		if (hit.collider.tag != player.tag) {
			audioSource.mute = true;
		} else {
			audioSource.mute = false;
		}
	}
}

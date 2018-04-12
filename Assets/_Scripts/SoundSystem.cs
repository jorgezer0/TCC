using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {

	public AudioPool audioPool;

	public AudioSource[] speakers;
	public GameObject roomsRoot;
	public BoxCollider[] room;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[ContextMenu ("Get Audio Sources")]
	void GetSources ()
	{
		speakers = GetComponentsInChildren<AudioSource> ();
	}

	[ContextMenu ("Get Rooms")]
	void GetRooms ()
	{
		room = roomsRoot.GetComponentsInChildren<BoxCollider> ();
	}

}

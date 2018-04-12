using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAudioBehaviour : MonoBehaviour {

	public AudioPool audioPool;

	public int roomId;
	public AudioSource[] speakers;


	public bool onEnter;
	public bool onStay;
	public bool openDoorOnFinish;
	public DoorBehaviour door;

	public void PlayInSpeakers(int clipId){
		foreach (AudioSource _speaker in speakers) {
			_speaker.clip = audioPool.GetAudio (roomId, clipId);
			_speaker.Play ();
		}
	}
		
	void OnTriggerEnter(Collider col){
		if (onEnter) {
			PlayInSpeakers (0);
			if (openDoorOnFinish)
				StartCoroutine ("OpenOnFinish");
			if (onStay)
				StartCoroutine ("CountTime");
		}
	}

	IEnumerator CountTime(){
		yield return new WaitForSeconds (Random.Range (30, 60));
		PlayInSpeakers (1);
	}

	IEnumerator OpenOnFinish(){
		yield return new WaitForSeconds (speakers [0].clip.length);
		door.ChangeDoorState ();
	}

	void OnTriggerExit(Collider col){
		foreach (AudioSource _speaker in speakers) {
			_speaker.Stop ();
		}
		this.gameObject.SetActive (false);
	}
}

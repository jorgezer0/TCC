using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchTimed : MonoBehaviour {

	public GameObject attachedTo;
	public int pulse;
	public float timer;
	public bool on = false;
	public float onPos;
	public Transform switchTransform;
	private float dur = 0.5f;
	private MeshRenderer switchRenderer;
	public Color onColor;
	public Color offColor;
	public MeshRenderer line;
	public MeshRenderer display;
	public bool playAudio;
	public RoomAudioBehaviour room;
	public int audioId;

	// Use this for initialization
	void Start () {
		switchRenderer = switchTransform.GetComponent<MeshRenderer> ();
		if (on) {
			switchTransform.localPosition = new Vector3 (0, onPos, onPos);
			switchRenderer.material.color = onColor;
			line.material.color = onColor;
			display.material.color = onColor;
		} else {
			switchTransform.localPosition = new Vector3 (0, -onPos, onPos);
			switchRenderer.material.color = offColor;
			line.material.color = offColor;
			display.material.color = offColor;
		}
	}

	public void InteracBehaviour(){
		on = !on;
		if (on) {
			switchTransform.DOLocalMoveY (onPos, dur);
			switchRenderer.material.DOColor (onColor, dur);
			line.material.DOColor (onColor, dur);
			display.material.DOColor (onColor, dur);
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
			StartCoroutine ("Timer");
		}
	}

	private void Revert(){
		switchTransform.DOLocalMoveY (-onPos, dur);
		switchRenderer.material.DOColor (offColor, dur);
		line.material.DOColor (offColor, dur);
		display.material.DOColor (offColor, dur);
		attachedTo.BroadcastMessage ("ButtonBehaviour", -pulse);
	}

	public void ButtonBehaviour(int p){
		if (on) {
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse + p);
		}
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds (timer);
		on = !on;
		Revert ();
	}

	private void PlayAudio(){
		if (playAudio) {
			room.PlayInSpeakers (audioId);
			playAudio = false;
		}
	}
}

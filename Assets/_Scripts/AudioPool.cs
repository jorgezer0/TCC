using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "_Inventory/AudioList", order = 1)]
public class AudioPool : ScriptableObject {

	[System.Serializable]
	public struct Audio
	{
		public AudioClip[] audio;
	}

	[SerializeField]
	public List<Audio> room;

	public AudioClip GetAudio(int _room, int _audio){
		return room [_room].audio [_audio];
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

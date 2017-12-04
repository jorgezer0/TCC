using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public GameObject attachedTo;

	public void Pressed(){
		Debug.Log ("Pressed Button!");
		attachedTo.BroadcastMessage ("ButtonBehaviour");
	}
}

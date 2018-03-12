using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumPad : MonoBehaviour {

	public GameObject attachedTo;
	public string password;
	public int pulse;
	public string entry;
	public TextMesh display;

	// Update is called once per frame
	void Update () {
		if (entry.Length > 0) {
			display.text = entry;
		}

		if ((entry.Length >= 4) && (entry == password)) {
			StartCoroutine ("RightPassword");
		} else if ((entry.Length >= 4) && (entry != password)) {
			StartCoroutine ("WrongPassword");
		}
	}

	IEnumerator RightPassword(){
		yield return new WaitForSeconds (0.5f);
		entry = "";
		display.text = "PASS";
		Debug.Log ("Right Password!");
		attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
		yield return new WaitForSeconds (1f);
		display.text = "XXXX";
	}

	IEnumerator WrongPassword(){
		yield return new WaitForSeconds (0.5f);
		entry = "";
		display.text = "WRONG";
		yield return new WaitForSeconds (1f);
		display.text = "XXXX";
	}
}

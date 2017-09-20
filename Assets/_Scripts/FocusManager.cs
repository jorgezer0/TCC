using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour {

	public Transform player;

	public enum focusType{
		Area,
		Path
	};
	
	public focusType type = focusType.Area;

	public List<Transform> focusPoints = new List<Transform>();
	Vector3 tmpFocus;
	Vector3 focus = Vector3.zero;
	int count = 0;
//	public GameObject debug;


	// Use this for initialization
	void Awake () {
		tmpFocus = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
//		debug.transform.position = GetFocus ();
	}

	public Vector3 GetFocus(){
		if (type == focusType.Area) {
			for (int i = 0; i < focusPoints.Count; i++) {
				tmpFocus += focusPoints [i].position;
				count++;
			}
			focus = tmpFocus / count;
			count = 0;
			tmpFocus = Vector3.zero;
			return focus;
		} else if (type == focusType.Path) {
			focus = focusPoints [0].transform.position;
			if ((player.position - focus).magnitude < 2) {
				focusPoints [0].gameObject.SetActive (false);
				focusPoints.Remove(focusPoints[0]);
			}
			return focus;
		}
		return Vector3.zero;
	}
}

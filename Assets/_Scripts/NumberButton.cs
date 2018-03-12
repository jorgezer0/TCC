using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton : MonoBehaviour {

	public NumPad numPad;
	public string value;

	public void InteracBehaviour(){
		if (numPad.entry.Length < 4)
			numPad.entry += value;
	}
}

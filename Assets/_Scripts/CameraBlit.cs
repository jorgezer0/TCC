using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBlit : MonoBehaviour {
	
	public Material mat;
	public float amount = 0;

	void Update(){
		mat.SetFloat ("_SmoothSize", amount);
		mat.SetFloat ("_Bright", (amount*2)+1);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, mat);
	}
}

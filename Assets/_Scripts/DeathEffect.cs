using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeathEffect : MonoBehaviour {
	
	public Material mat;
	[Range(0,2)]
	public float amount = 0;

	private PlayerController player;

	void Start(){
		player = GetComponentInParent<PlayerController> ();
	}

	void Update(){
		mat.SetFloat ("_SmoothSize", amount);
		mat.SetFloat ("_Bright", (amount*2)+1);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit (src, dest, mat);
	}

	public void GoToCheckPoint(){
		player.GoToCheckPoint ();
	}
}

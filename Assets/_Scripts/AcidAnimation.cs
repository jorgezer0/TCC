using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidAnimation : MonoBehaviour {

    [SerializeField]
    private Material mat;
    [SerializeField]
    private float speed;
    private Vector2 currentStep;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentStep.x += Time.deltaTime*speed;
        mat.SetTextureOffset("_NoiseTexture", currentStep);
	}
}

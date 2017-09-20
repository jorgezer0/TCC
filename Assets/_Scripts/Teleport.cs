using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
//using UnityEngine.VR;
using Kino;

public class Teleport : MonoBehaviour {

	public Camera cam;
	public float rotateSpeed = 2;
	RaycastHit hit;

	public GameObject controller;

	public GameObject tCursor;
	private Vector3 tCursor_velocity = Vector3.zero;
	private Vector3 tDestiny = Vector3.zero;
	float deltaTime;
	public float distance;
	Vector3 vel = Vector3.zero;
	bool canWarp = false;
	public float warpTime;
	AnalogGlitch glitch;
	Vignette vignette;
	Bloom bloom;
	public PostProcessingProfile pProces;
	PostProcessingProfile tempPProces;
	BloomModel.Settings ppBloom;
	ChromaticAberrationModel.Settings ppChromatic;
	VignetteModel.Settings ppVignette;

	public FocusManager focusManager;

	// Use this for initialization
	void Start () {
		glitch = cam.GetComponent<AnalogGlitch> ();
		vignette = cam.GetComponent<Vignette> ();
		bloom = cam.GetComponent<Bloom> ();
//		pProces = cam.GetComponent<PostProcessingProfile> ();
		ppBloom = pProces.bloom.settings;
		ppChromatic = pProces.chromaticAberration.settings;
		ppVignette = pProces.vignette.settings;
	}

	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		transform.Rotate(0, Input.GetAxis ("Mouse X") * rotateSpeed, 0);
		cam.transform.Rotate(-Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);


		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, distance)) {
//			Debug.DrawLine (cam.transform.position, hit.point, Color.red, 2f);
			if (hit.transform.gameObject.layer == 8) {
//				tCursor.transform.position = hit.point;
				if(!tCursor.activeSelf){
					tCursor.SetActive (true);
				}
				tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, hit.point, ref tCursor_velocity, 0.25f);
				if (Input.GetMouseButtonDown (0)) {
					//				transform.position = hit.point;
					tDestiny = tCursor.transform.position;
					StartCoroutine ("TeleportTo");
				}
			} else {
				if(tCursor.activeSelf){
					tCursor.SetActive (false);
				}
			}
		}
		if (canWarp) {
			pProces.motionBlur.enabled = true;
			transform.position = Vector3.SmoothDamp (transform.position, tDestiny, ref vel, warpTime);
			transform.LookAt (focusManager.GetFocus ());
			Vector3 normalize = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = Quaternion.Euler (normalize);
		}
	}

	IEnumerator TeleportTo(){
		float step = 0.01f;
		bool grow = true;
		int speed = 5;
		while (step > 0) {
			if (grow) {
				step += Time.deltaTime * speed;
			} else {					 
				step -= Time.deltaTime * speed*2;
			}
			glitch.enabled = true;
			glitch.scanLineJitter = step/2;
			glitch.colorDrift = step;
//			bloom.intensity = step*10;
//			vignette.intensity = step;
			ppBloom.bloom.intensity = step;
			ppChromatic.intensity = step;
			ppVignette.intensity = step;
			pProces.bloom.settings = ppBloom;
			pProces.chromaticAberration.settings = ppChromatic;
			pProces.vignette.settings = ppVignette;
			if (step >= 1) {
				grow = false;
//				transform.position = tCursor.transform.position;
			}
			if (step >= 0.5f) {
				canWarp = true;
			}

			yield return new WaitForSeconds (1/(deltaTime * 1000f));
		}
		pProces.motionBlur.enabled = false;
		glitch.scanLineJitter = 0;
		glitch.horizontalShake = 0;
		glitch.colorDrift = 0;
		glitch.enabled = false;
//		bloom.intensity = 0;
//		vignette.intensity = 0;
		ppBloom.bloom.intensity = 0;
		ppChromatic.intensity = 0;
		ppVignette.intensity = 0;
		canWarp = false;
		pProces.bloom.settings = ppBloom;
		pProces.chromaticAberration.settings = ppChromatic;
		pProces.vignette.settings = ppVignette;
		yield return null;
	}
}

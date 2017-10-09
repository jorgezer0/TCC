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

	public GameObject cursorCanvas;
	public GameObject controller;
	public LineRenderer line;
	public Transform particleLine;
	public Transform rayOrigin;

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

//		Time.timeScale = 0.5f;

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

		if (!OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {
			transform.Rotate (0, Input.GetAxis ("Mouse X") * rotateSpeed, 0);
			cam.transform.Rotate (-Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
		}

		if (OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {
//			Debug.Log ("Controller Conected!");
			cursorCanvas.SetActive (false);
			controller.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
			Debug.DrawRay (rayOrigin.transform.position, rayOrigin.forward);
			if (Physics.Raycast (rayOrigin.position, rayOrigin.forward, out hit, distance)) {
//				line.SetPosition (1, hit.point);

				line.SetPosition (0, rayOrigin.position);
				//				tCursor.transform.position = hit.point;
				if (!tCursor.activeSelf) {
					tCursor.SetActive (true);
				}
//					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, hit.point, ref tCursor_velocity, 0.1f);
				tCursor.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
				Vector3 dir = new Vector3 (focusManager.GetFocus ().x, 0, focusManager.GetFocus ().z);
				tCursor.transform.LookAt (dir);
//				line.SetPosition (1, tCursor.transform.position);
				line.SetPosition (1, hit.point);

				if (OVRInput.GetDown (OVRInput.Button.PrimaryTouchpad)) {
					StartCoroutine ("SlowTime");
				}

				if (OVRInput.GetUp (OVRInput.Button.PrimaryTouchpad)) {
					StartCoroutine ("NormalTime");
					tDestiny = tCursor.transform.position;
					StartCoroutine ("TeleportTo");
				}

			}

		} else {
//			Debug.Log ("Controller Disconected!");
			cursorCanvas.SetActive (true);
//			if (controller.activeSelf) {
//				controller.SetActive (false);
//			}
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, distance)) {
//			Debug.DrawLine (cam.transform.position, hit.point, Color.red, 2f);

				line.SetPosition (0, rayOrigin.position);
				//				tCursor.transform.position = hit.point;
				if (!tCursor.activeSelf) {
					tCursor.SetActive (true);
				}
				//					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, hit.point, ref tCursor_velocity, 0.1f);
				tCursor.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
				Vector3 dir = new Vector3 (focusManager.GetFocus ().x, 0, focusManager.GetFocus ().z);
				tCursor.transform.LookAt (dir);
				particleLine.LookAt (hit.point);
				//				line.SetPosition (1, tCursor.transform.position);
				line.SetPosition (1, hit.point);
				if (Input.GetMouseButtonDown (0)) {
					Debug.Log ("Slow...");
					StartCoroutine ("SlowTime");
				}

				if (Input.GetMouseButtonUp (0)) {
					Debug.Log ("normal...");
					tDestiny = tCursor.transform.position;
					StartCoroutine ("TeleportTo");
					StartCoroutine ("NormalTime");
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
				step += (deltaTime*Time.timeScale) * speed;
			} else {					 
				step -= (deltaTime*Time.timeScale) * speed*2;
			}
			glitch.enabled = true;
			glitch.scanLineJitter = step/2;
			glitch.colorDrift = step;
//			bloom.intensity = step*10;
//			vignette.intensity = step;
			ppBloom.bloom.intensity = step/2;
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

//			yield return new WaitForSeconds (1/((deltaTime/Time.timeScale) * 1000f));
			yield return new WaitForSeconds (Time.deltaTime/Time.timeScale);
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

	IEnumerator SlowTime(){
		while (Time.timeScale > 0.05f) {
			Time.timeScale -= 0.05f;
		}
		yield return null;
	}

	IEnumerator NormalTime(){
//		yield return new WaitForSeconds (2f);
		while (Time.timeScale < 1f) {
			Time.timeScale += 0.1f;
		}
		Time.timeScale = 1;
		yield return null;
	}
}


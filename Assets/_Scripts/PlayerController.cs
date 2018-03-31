using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
//using UnityEngine.VR;
using Kino;

public class PlayerController : MonoBehaviour {

	public Camera cam;
	public float rotateSpeed = 2;
	RaycastHit hit;
	int layerMask = 1 << 0;

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
	public float wallOffset;
	Vector3 vel = Vector3.zero;
	bool canWarp = false;
	public float warpTime;
	public float teleCharge;
	private float tempCharge = 0;
	public Image chargeGauge;
	public Animator chargeGaugeAnim;
	private bool canCharge = true;

	AnalogGlitch glitch;
	Vignette vignette;
	Bloom bloom;
	public PostProcessingProfile pProces;
	PostProcessingProfile tempPProces;
	BloomModel.Settings ppBloom;
	ChromaticAberrationModel.Settings ppChromatic;
	VignetteModel.Settings ppVignette;
	RaycastHit visualHit;

	Animator camAnim;
	CameraBlit camBlit;

	public Image timeGauge;
	private Animator timeGaugeAnim;
	public float timeInSlow = 5f;
	public float refilTime = 5f;

    public Text log;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		tCursor.transform.parent = null;

		timeGaugeAnim = timeGauge.GetComponent<Animator> ();
		chargeGaugeAnim = chargeGauge.GetComponent<Animator> ();
//		focusManager = GameObject.Find ("FocusManager").GetComponent<FocusManager>();
//		Time.timeScale = 0.5f;

		camAnim = cam.GetComponent<Animator> ();
		camBlit = cam.GetComponent<CameraBlit> ();
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

        log.text = OVRInput.GetConnectedControllers().ToString();

        if (!OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {
			transform.Rotate (0, Input.GetAxis ("Mouse X") * rotateSpeed, 0);
			cam.transform.Rotate (-Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
		}

		if (OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {
//			Debug.Log ("Controller Conected!");
			cursorCanvas.SetActive (false);
			controller.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
            
            Debug.DrawRay (rayOrigin.transform.position, rayOrigin.forward);

			if (Physics.Raycast (rayOrigin.position, rayOrigin.forward, out hit, distance, layerMask, QueryTriggerInteraction.Ignore))
            {
                line.SetPosition(1, hit.point);

                line.SetPosition (0, rayOrigin.position);

                if (!tCursor.activeSelf) {
					tCursor.SetActive (true);
				}

				if (hit.collider.tag == "Wall") {
					Vector3 wallHit = hit.point - ((hit.point - rayOrigin.transform.position).normalized * wallOffset);
					tCursor.transform.localPosition = Vector3.SmoothDamp (tCursor.transform.position, new Vector3(wallHit.x, 0, wallHit.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				} else {
					tCursor.transform.localPosition = Vector3.SmoothDamp (tCursor.transform.position, new Vector3 (hit.point.x, 0, hit.point.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				}

                //Interact with interactables.
                if (hit.collider.tag == "Interact")
                {
                    tCursor.SetActive(false);
                    if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) && ((hit.collider.transform.position - transform.position).magnitude < 3))
                    {
                        hit.collider.BroadcastMessage("InteracBehaviour");
                    }
                }
                else
                {
                    if (!tCursor.activeSelf)
                    {
                        tCursor.SetActive(true);
                    }
                }

                //				line.SetPosition (1, tCursor.transform.position);
                line.SetPosition (1, hit.point);
                
				if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)){
					SlowCountdown ();
                }

				if ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && (hit.collider.tag != "Interact"))
                {
					tempCharge += Time.deltaTime;
					chargeGauge.fillAmount = tempCharge / teleCharge;
					if (tempCharge >= teleCharge) {
						canCharge = false;
						tDestiny = tCursor.transform.position;
						//StartCoroutine ("TeleportTo");
						camAnim.Play("TeleportTunel");
						tempCharge = 0;
						chargeGauge.fillAmount = 0;
					}
				} else if (tempCharge > 0) {
					canCharge = true;
					tempCharge = 0;
					chargeGauge.fillAmount = 0;
				} else {
					canCharge = true;
				}

            }

		} else {
//			Debug.Log ("Controller Disconected!");
			cursorCanvas.SetActive (true);
//			if (controller.activeSelf) {
//				controller.SetActive (false);
//			}
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, distance, layerMask, QueryTriggerInteraction.Ignore)) {
//			Debug.DrawLine (cam.transform.position, hit.point, Color.red, 2f);

				line.SetPosition (0, rayOrigin.position);
				//				tCursor.transform.position = hit.point;
				if (!tCursor.activeSelf) {
					tCursor.SetActive (true);
				}

				if (hit.collider.tag == "Wall") {
					Vector3 wallHit = hit.point - ((hit.point - rayOrigin.transform.position).normalized * wallOffset);
					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, new Vector3(wallHit.x, 0, wallHit.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				} else {
					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, new Vector3 (hit.point.x, 0, hit.point.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				}

				//Interact with interactables.
				if (hit.collider.tag == "Interact") {
					tCursor.SetActive (false);
					if ((Input.GetMouseButtonDown (0)) && (hit.collider.transform.position - transform.position).magnitude < 3){
						hit.collider.BroadcastMessage ("InteracBehaviour");
					}
				} else {
					if (!tCursor.activeSelf) {
						tCursor.SetActive (true);
					}
				}

//				tCursor.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
//				Vector3 dir = new Vector3 (focusManager.GetFocus ().x, 0, focusManager.GetFocus ().z);
//				tCursor.transform.LookAt (dir);
				particleLine.LookAt (hit.point);
				//				line.SetPosition (1, tCursor.transform.position);
				line.SetPosition (1, hit.point);

                if (Input.GetMouseButtonDown(1))
                {
					SlowCountdown ();
                }

				if (Input.GetMouseButtonDown (0)) {
					if (!chargeGaugeAnim.GetBool ("charge")) {
						chargeGaugeAnim.SetBool ("charge", true);
						chargeGaugeAnim.SetBool ("idle", false);
						chargeGaugeAnim.SetFloat ("speed", chargeGaugeAnim.speed / Time.timeScale);
					} 
				} else if (Input.GetMouseButtonUp (0)) {
					if (chargeGaugeAnim.GetBool ("charge")) {
						chargeGaugeAnim.SetBool ("charge", false);
						chargeGaugeAnim.SetFloat ("speed", -chargeGaugeAnim.speed / Time.timeScale);
					}
				}
            }
		}
		if (canWarp) {
			pProces.motionBlur.enabled = true;
			transform.position = Vector3.SmoothDamp (transform.position, tDestiny, ref vel, warpTime * Time.timeScale);
//			transform.LookAt (focusManager.GetFocus ());
			Vector3 normalize = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = Quaternion.Euler (normalize);
			Debug.Log (Vector3.Distance (transform.position, tDestiny));
			if (Vector3.Distance (transform.position, tDestiny) < 0.15f) {
				canWarp = false;
				camAnim.SetBool ("play", false);
				camAnim.SetBool ("arrived", true);
			}
		}
	}

	public void StartTeleport(){
		tDestiny = tCursor.transform.position;
		camAnim.SetFloat("speed", camAnim.speed / Time.timeScale);
		camAnim.SetBool ("play", true);
		camAnim.SetBool ("arrived", false);
	}

	public void Teleport(){
		canWarp = true;
	}

	void SlowCountdown(){
		if (!timeGaugeAnim.GetBool ("slow")) {
			TimeManager.instance.SlowTime ();
			if (timeGaugeAnim.GetCurrentAnimatorStateInfo (0).IsName ("TimeGauge")) {
				float tempTime = timeGaugeAnim.GetCurrentAnimatorStateInfo (0).normalizedTime;
				float tempNorm = tempTime - Mathf.FloorToInt (tempTime);
				timeGaugeAnim.Play ("TimeGauge", 0, tempNorm);
			}
			timeGaugeAnim.SetFloat ("speed", (timeGaugeAnim.speed / Time.timeScale));
			timeGaugeAnim.SetBool ("full", false);
			timeGaugeAnim.SetBool ("slow", true);
		} else {
			timeGaugeAnim.SetFloat ("speed", -0.5f);
			timeGaugeAnim.SetBool ("slow", false);
			TimeManager.instance.NormalTime ();
		}
		
	}

//	IEnumerator TeleportTo(){
//		float step = 0.01f;
//		bool grow = true;
//		int speed = 3;
//		while (step > 0) {
//			if (grow) {
//				step += (Time.deltaTime/Time.timeScale) * speed;
//			} else {					 
//				step -= (Time.deltaTime/Time.timeScale) * speed*2;
//			}
//			camBlit.amount = step * 2;
//			glitch.enabled = true;
//			glitch.scanLineJitter = step/2;
//			glitch.colorDrift = step/4;
////			bloom.intensity = step*10;
////			vignette.intensity = step;
//			ppBloom.bloom.intensity = step;
//			ppChromatic.intensity = step;
//			ppVignette.intensity = step;
//			pProces.bloom.settings = ppBloom;
//			pProces.chromaticAberration.settings = ppChromatic;
//			pProces.vignette.settings = ppVignette;
//			if (step >= 1) {
//				grow = false;
////				transform.position = tCursor.transform.position;
//			}
//			if (step >= 0.5f) {
//				canWarp = true;
//			}
//
//			yield return new WaitForSeconds (Time.deltaTime);
//		}
//		pProces.motionBlur.enabled = false;
//		camBlit.amount = 0;
//		glitch.scanLineJitter = 0;
//		glitch.horizontalShake = 0;
//		glitch.colorDrift = 0;
//		glitch.enabled = false;
////		bloom.intensity = 0;
////		vignette.intensity = 0;
//		ppBloom.bloom.intensity = 0;
//		ppChromatic.intensity = 0;
//		ppVignette.intensity = 0;
//		canWarp = false;
//		pProces.bloom.settings = ppBloom;
//		pProces.chromaticAberration.settings = ppChromatic;
//		pProces.vignette.settings = ppVignette;
//		yield return null;
//	}

    
}


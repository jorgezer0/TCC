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
	int layerMask;

	public GameObject cursorCanvas;
	public Transform mousePivot;
	public Transform vrPivot;
	public GameObject controller;
	public Transform canon;
	public Transform fore_arm;
	public Transform arm;
	public LineRenderer line;
	public Transform particleLine;
	public Transform rayOrigin;
	public Transform lineOrigin;

	public TCursorBehaviour tCursor;
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

	private Vector3 firstPos;
	private Vector3 lastPos;
	private float cursorDelta;
	public float cursorThreshold;
	private bool breakCharge = false;

	public  DoorBehaviour detectedDoor;
	public Transform checkPoint;
	private bool isDead = false;

    public Text log;

	// Use this for initialization
	void Start () {
		layerMask = LayerMask.GetMask("Default", "Door");
		Debug.Log (layerMask);
		Cursor.lockState = CursorLockMode.Locked;
		tCursor.transform.parent = null;

		timeGaugeAnim = timeGauge.GetComponent<Animator> ();
		chargeGaugeAnim = chargeGauge.GetComponent<Animator> ();
//		focusManager = GameObject.Find ("FocusManager").GetComponent<FocusManager>();
//		Time.timeScale = 0.5f;

		camAnim = cam.GetComponent<Animator> ();
		camBlit = cam.GetComponent<CameraBlit> ();
	}

	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

//        log.text = OVRInput.GetConnectedControllers().ToString();

        if (!OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {
			transform.Rotate (0, Input.GetAxis ("Mouse X") * rotateSpeed, 0);
			cam.transform.Rotate (-Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
		}

		if (OVRInput.IsControllerConnected (OVRInput.Controller.RTrackedRemote)) {

//			Debug.Log ("Controller Conected!");
			cursorCanvas.transform.parent = vrPivot;
			cursorCanvas.transform.position = vrPivot.position;
			cursorCanvas.transform.rotation = vrPivot.rotation;

			controller.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

			Quaternion controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

			canon.localRotation = Quaternion.Euler (0, 0, controllerRot.eulerAngles.z);
			fore_arm.localRotation = Quaternion.Euler (controllerRot.eulerAngles.x, controllerRot.eulerAngles.y, 0);
			arm.localRotation = Quaternion.Euler (arm.localEulerAngles.x, arm.localEulerAngles.y, fore_arm.localEulerAngles.y);

			if (Physics.Raycast (rayOrigin.position, rayOrigin.forward, out hit, distance, layerMask, QueryTriggerInteraction.Ignore))
            {
				line.SetPosition (1, hit.point);
				line.SetPosition (0, lineOrigin.position);

				if (hit.collider.tag == "Wall") {
					ToggleLine (true);
					Vector3 wallHit = hit.point - ((hit.point - rayOrigin.transform.position).normalized * wallOffset);
					tCursor.transform.localPosition = Vector3.SmoothDamp (tCursor.transform.position, new Vector3 (wallHit.x, 0, wallHit.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				} else if (hit.collider.tag == "Floor") {
					ToggleLine (true);
					tCursor.transform.localPosition = Vector3.SmoothDamp (tCursor.transform.position, new Vector3 (hit.point.x, 0, hit.point.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				} else {
					ToggleLine (false);
				}

                //Interact with interactables.
                if (hit.collider.tag == "Interact")
                {
					tCursor.gameObject.SetActive(false);
                    if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) && ((hit.collider.transform.position - transform.position).magnitude < 3))
                    {
                        hit.collider.BroadcastMessage("InteracBehaviour");
                    }
                }

                //				line.SetPosition (1, tCursor.transform.position);
                line.SetPosition (1, hit.point);
                
				if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)){
					SlowCountdown ();
                }

				if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
					if ((!chargeGaugeAnim.GetBool ("charge")) && (canCharge)) {
						firstPos = tCursor.transform.position;
						chargeGaugeAnim.SetBool ("charge", true);
						chargeGaugeAnim.SetBool ("idle", false);
						chargeGaugeAnim.SetFloat ("speed", chargeGaugeAnim.speed / Time.timeScale);
					}

					lastPos = tCursor.transform.position;
					cursorDelta = Vector3.Distance(firstPos, lastPos);
					if (cursorDelta > cursorThreshold){
						breakCharge = true;
					}
				}
					
					if ((OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) || (breakCharge)){
					if (chargeGaugeAnim.GetBool ("charge")) {
						chargeGaugeAnim.SetBool ("charge", false);
						chargeGaugeAnim.SetBool ("idle", true);
						chargeGaugeAnim.SetFloat ("speed", -chargeGaugeAnim.speed/2 / Time.timeScale);
						breakCharge = false;
					}
				}

            }

		} else {
//			Debug.Log ("Controller Disconected!");
			cursorCanvas.transform.SetParent(mousePivot);
			cursorCanvas.transform.position = mousePivot.position;
			cursorCanvas.transform.rotation = mousePivot.rotation;
//			if (controller.activeSelf) {
//				controller.SetActive (false);
//			}
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, distance, layerMask, QueryTriggerInteraction.Ignore)) {
				line.SetPosition (0, lineOrigin.position);

				if (hit.collider.tag == "Wall") {
					ToggleLine (true);
					Vector3 wallHit = hit.point - ((hit.point - rayOrigin.transform.position).normalized * wallOffset);
					tCursor.TCursorAdjustedPos (wallHit);
				} else if (hit.collider.tag == "Floor") {
					ToggleLine (true);
					tCursor.TCursorAdjustedPos (hit.point);
				} else {
					ToggleLine (false);
				}

				//Interact with interactables.
				if (hit.collider.tag == "Interact") {
					tCursor.gameObject.SetActive (false);
					if ((Input.GetMouseButtonDown (0)) && (hit.collider.transform.position - transform.position).magnitude < 3){
						hit.collider.BroadcastMessage ("InteracBehaviour");
					}
				} else {
//					if (!tCursor.activeSelf) {
//						tCursor.SetActive (true);
//					}
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

				if (Input.GetMouseButton (0)) {
					if ((!chargeGaugeAnim.GetBool ("charge")) && (canCharge)) {
						firstPos = tCursor.transform.position;
						chargeGaugeAnim.SetBool ("charge", true);
						chargeGaugeAnim.SetBool ("idle", false);
						chargeGaugeAnim.SetFloat ("speed", chargeGaugeAnim.speed / Time.timeScale);
					}

					lastPos = tCursor.transform.position;
					cursorDelta = Vector3.Distance(firstPos, lastPos);
					if (cursorDelta > cursorThreshold){
						breakCharge = true;
					}
				}
				if ((Input.GetMouseButtonUp (0)) || (breakCharge)){
					if (chargeGaugeAnim.GetBool ("charge")) {
						chargeGaugeAnim.SetBool ("charge", false);
						chargeGaugeAnim.SetBool ("idle", true);
						chargeGaugeAnim.SetFloat ("speed", -chargeGaugeAnim.speed/2 / Time.timeScale);
						breakCharge = false;
					}
				}

            }
		}

		if (canWarp) {
			transform.position = Vector3.SmoothDamp (transform.position, tDestiny, ref vel, warpTime * Time.timeScale);
			Vector3 normalize = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = Quaternion.Euler (normalize);
			if (Vector3.Distance (transform.position, tDestiny) < 0.15f) {
				canWarp = false;
//				camAnim.SetBool ("play", false);
				camAnim.SetTrigger ("arrived_");
				if (detectedDoor != null) {
					detectedDoor.AutoClose (this);
					detectedDoor = null;
				}
			}
		} else {
			CheckFloor ();
		}

	}

	private void ToggleLine(bool toggle){
		if (canCharge != toggle) {
			tCursor.gameObject.SetActive (toggle);
			canCharge = toggle;
		}
		line.endColor = toggle ? Color.green : Color.red;
	}

	public void StartTeleport(){
		Debug.Log ("Start Teleport!");
		tDestiny = tCursor.transform.position;
		detectedDoor = CheckDoor ();
		camAnim.SetFloat("speed", camAnim.speed / Time.timeScale);
		camAnim.SetTrigger ("play_");
//		camAnim.SetBool ("arrived", false);
	}

	public void Teleport(){
		Debug.Log ("Can Warp!");
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
			timeGaugeAnim.SetFloat ("speed", -2f);
			timeGaugeAnim.SetBool ("slow", false);
			TimeManager.instance.NormalTime ();
		}
	}

	public void GoToCheckPoint(){
		transform.position = checkPoint.position;
		transform.rotation = checkPoint.rotation;

		camAnim.SetTrigger ("arrived_");
		isDead = false;
	}

	void CheckFloor(){
		if (!isDead) {
			RaycastHit _floorHit;
			Physics.Raycast (cam.transform.position, transform.up * -1, out _floorHit, 10f);
			if ((_floorHit.collider.tag == "Water")) {
				isDead = true;
				canWarp = false;
				tDestiny = checkPoint.position;
				camAnim.SetTrigger ("dead_");
			}
		}
	}

	private DoorBehaviour CheckDoor(){
		DoorBehaviour _detectedDoor = null;
		RaycastHit _doorHit;
		int layer_mask = LayerMask.GetMask ("Door");
		if (Physics.Raycast (cam.transform.position, (hit.point - cam.transform.position), out _doorHit, 100f, layer_mask)) {
			Debug.Log (_doorHit.collider.name);
			if (_doorHit.collider.isTrigger)
				_detectedDoor = _doorHit.collider.GetComponent<DoorBehaviour> ();
		}
		return _detectedDoor;
	}
		    
}


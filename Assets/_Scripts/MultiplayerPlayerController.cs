using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Networking;
using Kino;

public class MultiplayerPlayerController : NetworkBehaviour {

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
	public float wallOffset;
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
	DepthOfFieldModel.Settings ppDepth;
	RaycastHit visualHit;
	float tempFocusDist;

	ParticleSystem _particles;

	public float force = 1000;
	public GameObject bullet;

	public Player _player;
	public NetworkManager _netMan;

	// Use this for initialization
	void Start () {
		_player = GetComponent <Player> ();
		_particles = GetComponent <ParticleSystem> ();
		_netMan = GameObject.Find ("_NetworkManager").GetComponent <NetworkManager> ();
//		Time.timeScale = 0.5f;

		glitch = cam.GetComponent<AnalogGlitch> ();
		vignette = cam.GetComponent<Vignette> ();
		bloom = cam.GetComponent<Bloom> ();
//		pProces = cam.GetComponent<PostProcessingProfile> ();
		ppBloom = pProces.bloom.settings;
		ppChromatic = pProces.chromaticAberration.settings;
		ppVignette = pProces.vignette.settings;
		ppDepth = pProces.depthOfField.settings;
	}

	// Update is called once per frame
	void Update () {
		AdaptDepthOfField ();
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

			if (isLocalPlayer) {
				if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger)) {
					CmdShoot (rayOrigin.transform.position + rayOrigin.transform.forward, rayOrigin.transform.forward);
				}
			}

			if (Physics.Raycast (rayOrigin.position, rayOrigin.forward, out hit, distance)) {
//				line.SetPosition (1, hit.point);

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
//				tCursor.transform.position = new Vector3(hit.point.x, 0, hit.point.z);


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

			if (isLocalPlayer) {
				if (Input.GetMouseButtonDown (0)) {
					CmdShoot (rayOrigin.transform.position + rayOrigin.transform.forward, rayOrigin.transform.forward);
				}
			}

			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, distance)) {
//			Debug.DrawLine (cam.transform.position, hit.point, Color.red, 2f);

				line.SetPosition (0, rayOrigin.position);
				//				tCursor.transform.position = hit.point;
				if (!tCursor.activeSelf) {
					tCursor.SetActive (true);
				}
				if (hit.collider.tag == "Wall") {
					Vector3 wallHit = hit.point - ((hit.point - cam.transform.position).normalized * wallOffset);
					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, new Vector3(wallHit.x, 0, wallHit.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				} else {
					tCursor.transform.position = Vector3.SmoothDamp (tCursor.transform.position, new Vector3 (hit.point.x, 0, hit.point.z), ref tCursor_velocity, 0.1f * Time.timeScale);
				}
//				tCursor.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

				particleLine.LookAt (hit.point);
				//				line.SetPosition (1, tCursor.transform.position);
				line.SetPosition (1, hit.point);
				if (Input.GetMouseButtonDown (1)) {
					Debug.Log ("Slow...");
					StartCoroutine ("SlowTime");
				}

				if (Input.GetMouseButtonUp (1)) {
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

//			transform.position = tDestiny;
			CmdTeleportRemote(tDestiny);
			Vector3 normalize = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = Quaternion.Euler (normalize);
		}

		if (_player.health == 0){
			Transform randomSpawn = _netMan.startPositions [Random.Range (0, _netMan.startPositions.Count)].transform;
			CmdTeleportRemote (randomSpawn.position);
			transform.rotation = randomSpawn.rotation;
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
			glitch.colorDrift = step/4;
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
//				_particles.Emit (5);
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
			Time.timeScale += 0.01f;
		}
		Time.timeScale = 1;
		yield return null;
	}

	void AdaptDepthOfField(){
		
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out visualHit, distance)) {
//			ppDepth.focusDistance = (cam.transform.position - hit.point).magnitude;
			ppDepth.focusDistance = Mathf.Lerp (ppDepth.focusDistance, (cam.transform.position - visualHit.point).magnitude, ((Time.deltaTime/Time.timeScale)*4));
			pProces.depthOfField.settings = ppDepth;
//			Debug.Log (ppDepth.focusDistance);
		}
	}

	[Command]
	void CmdTeleportRemote(Vector3 dest){
		RpcTeleportRemote(transform.name, dest);
	}

	[ClientRpc]
	void RpcTeleportRemote(string _playerID, Vector3 tDest){
		Player _player = GameManager.GetPlayer (_playerID);

		_player.RpcTeleportTo (tDest);
	}

	[Command]
	public void CmdShoot(Vector3 position, Vector3 dir){
		RpcInstantiateBullet (position, dir);
	}

	[ClientRpc]
	public void RpcInstantiateBullet(Vector3 position, Vector3 dir){

		GameObject obj = (GameObject)Instantiate (bullet, 
			position,Quaternion.identity);
		obj.GetComponent <Bullet>().shooter = GetComponent <Player>();
		obj.GetComponent<Rigidbody> ().AddForce (dir * force);
	}


}


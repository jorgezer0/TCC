using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

	bool canTeleport = false;
	Vector3 tDestiny;

	[SyncVar]
	public float health = 10;
	public TextMesh life;
	public TextMesh lifeHUD;

	public Image lifeBar;
	Color emptyBar = Color.red;
	Color fullBar = Color.green;

	public int score = 0;
	int playersCount = 0;
	public Text versusScore;

	ParticleSystem _particles;


	// Use this for initialization
	void Start () {
		life.text = health.ToString ();
		_particles = GetComponent <ParticleSystem> ();
		versusScore.text = "";
		foreach(KeyValuePair<string, Player> _players in GameManager.players){
			versusScore.text += (_players.Key + " - " + _players.Value.score + "\n");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canTeleport) {
			transform.position = Vector3.Lerp (transform.position, tDestiny, Time.deltaTime * 5);
			if (transform.position == tDestiny) {
				canTeleport = false;
				Debug.Log (transform.name + " moving.");
			}
		}

		life.transform.LookAt (2 * life.transform.position - Camera.allCameras [0].transform.position);

		if (health == 0){
			health = 10;
			life.text = health.ToString ();
		}

		float amount = health / 10;
		Debug.Log (amount);
		lifeBar.color = Color.Lerp (emptyBar, fullBar, amount);
		lifeBar.fillAmount = amount;


		versusScore.text = "";
		foreach(KeyValuePair<string, Player> _players in GameManager.players){
			versusScore.text += (_players.Key + " - " + _players.Value.score + "\n");
		}


	}
		
	public void RpcTeleportTo(Vector3 tDest){
		tDestiny = tDest;
		canTeleport = true;
		_particles.Emit (5);
	}

	public void SetTimeScale(float t){
		Time.timeScale = t;
	}

	[ClientRpc]
	public void RpcDamagePlayer(string shooter){
		health--;
		life.text = health.ToString ();
		Player _shooter = GameManager.GetPlayer (shooter);
		if (health == 0){
			_shooter.score++;
		}
		versusScore.text = "";
		foreach(KeyValuePair<string, Player> _players in GameManager.players){
			versusScore.text += (_players.Key + " - " + _players.Value.score + "\n");
		}
	}
}

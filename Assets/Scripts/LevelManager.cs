using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private bool isCheckpointed;
	private Vector3 initialPlayerPos;
	public float timeWhileDeath;
	public GameObject mainCamera;
	public GameObject  onlyPlayerCamera;
	Enemy[] enemies;
	Trap[] traps;
	Box[] boxes;
	ButtonArrow[] buttonsArrow;
	private Player player;
	ChangeAmbient[] changesForAmbient;
	Mirror[] mirrors;
	TrapDoor[] trapDoors;

	void Awake()
	{
		isCheckpointed = false;

		if(TransitionManager.Instance == null){
			var transitionManager= new GameObject().AddComponent<TransitionManager>();
			transitionManager.name = "TransitionManager";
		}
		if(GameManager.Instance == null){
			var gameManager= new GameObject().AddComponent<GameManager>();
			gameManager.name = "GameManager";
			gameManager.gameObject.AddComponent<Lang>();
		}
		if(SoundManager.Instance == null){
			var soundManager= new GameObject().AddComponent<SoundManager>();
			soundManager.name = "SoundManager";
		}
	}

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
		enemies = FindObjectsOfType<Enemy>();
		traps = FindObjectsOfType<Trap>();
		boxes = FindObjectsOfType<Box>();
		buttonsArrow = FindObjectsOfType<ButtonArrow>();
		changesForAmbient = FindObjectsOfType<ChangeAmbient>();
		mirrors = FindObjectsOfType<Mirror>();
		trapDoors = FindObjectsOfType<TrapDoor>();
		TimeInNormal();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public Vector3 GetPlayerInitialPos(){
		return initialPlayerPos;
	}
	public void SetPlayerInitialPos(Vector3 initPos){
		initialPlayerPos = initPos;
	}

	public void TimeInDeath(){
		if(!isCheckpointed){
			var invent = FindObjectOfType<Inventory>();
			if(invent.HasKey()){
				invent.RemoveKey();
			}
			foreach(Enemy e in enemies){
				e.Respawn();
			}
			foreach(Trap t in traps){
				t.Enable();
			}
			foreach(Box b in boxes){
				b.Respawn();
			}
			foreach(ButtonArrow b in buttonsArrow){
				b.DisableButton();
			}
			foreach(TrapDoor t in trapDoors){
				t.Enable();
			}
		}
		Time.timeScale = timeWhileDeath;
		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionRamp>().enabled = true;
		onlyPlayerCamera.SetActive(true);
	}

	public void TimeInNormal(){
		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionRamp>().enabled = false;
		player.ImBack();
		onlyPlayerCamera.SetActive(false);
		Time.timeScale = 1f;
		if(!isCheckpointed){
			foreach(Mirror m in mirrors){
				m.ResetMirror();
			}
			foreach(ChangeAmbient c in changesForAmbient){
				c.ResetObject();
			}
		}
	}

	public void ActiveCheckPoint(){
		isCheckpointed = true;
		SetPlayerInitialPos(player.gameObject.transform.position);
	}

	public bool IsThereAFuckingCheckpoint(){
		return isCheckpointed;
	}
}
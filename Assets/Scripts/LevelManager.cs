using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private Vector3 initialPlayerPos;
	public float timeWhileDeath;
	public GameObject mainCamera;
	public GameObject  onlyPlayerCamera;

	// Use this for initialization
	void Start () {
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
		Time.timeScale = timeWhileDeath;
		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionRamp>().enabled = true;
		onlyPlayerCamera.SetActive(true);
	}

	public void TimeInNormal(){
		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionRamp>().enabled = false;
		onlyPlayerCamera.SetActive(false);
		Time.timeScale = 1f;
	}
}
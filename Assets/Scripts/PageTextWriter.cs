using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PageTextWriter : MonoBehaviour {

	public float delay = 0.05f;
	public string fullText;
	private string currentText = "";
	public Font ocidentalFont;
	public Font japaneseFont;

	// Use this for initialization
	void Start () {
		StartCoroutine(ShowText());
		if(FindObjectOfType<GameManager>().Language == "Japanese"){
			GetComponent<Text>().font = japaneseFont;
		}
		else{
			GetComponent<Text>().font = ocidentalFont;
		}
		
	}
	
	IEnumerator ShowText(){
		for(int i = 0; i <= fullText.Length; i++){
			currentText = fullText.Substring(0,i);
			this.GetComponent<Text>().text = currentText;
			yield return new WaitForSecondsRealtime(delay);
		}
	}
}

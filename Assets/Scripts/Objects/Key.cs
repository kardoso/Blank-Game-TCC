using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	public AudioClip keyFX;

	void Start()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -1);
	}

	void Update()
	{
		if(Time.timeScale < 1 && Time.timeScale > 0)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			SoundManager.PlaySFX(keyFX);
			//input
			other.gameObject.GetComponent<Inventory>().AddKey();
			Destroy(this.gameObject);
		}
	}
}

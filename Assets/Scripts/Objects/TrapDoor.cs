using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapDoor : MonoBehaviour {
	private bool keyPressed = false;

	private bool isInside;

	public GameObject enemyToSpawn;
    private GameObject ObjectForEnemiesSpawn;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

	public AudioClip trapFX;

	void Start()
	{
		foreach(Transform t in transform){
			t.gameObject.SetActive(false);
		}

		p1 = point1.position;
        p2 = point2.position;
        p3 = point3.position;
	}

	void Update()
	{
		if(isInside){
			transform.Find("ButtonKeyboard").gameObject.SetActive(keyPressed?false:true);
			if((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("B")) && !keyPressed){
				FindObjectOfType<Inventory>().RemoveKey();
				FindObjectOfType<Player>().StopMovement();
				keyPressed = true;
				GetComponent<Animator>().SetTrigger("AbrirPorta");
			}
		}
		else{
			foreach(Transform t in transform){
				t.gameObject.SetActive(false);
			}
		}
	}

	void AbrirPorta(){
		int _value = Random.Range(0,2);
		switch (_value){
			case 0:
				GetComponent<Animator>().SetTrigger("Porta1");
				break;
			case 1:
				GetComponent<Animator>().SetTrigger("Porta2");
				break;
			default:
				break;
		}
	}

	void LoadLevel(){
		FindObjectOfType<Inventory>().AddKey();
		FindObjectOfType<Player>().ImBack();
		SoundManager.PlaySFX(trapFX);
		SpawnEnemies();
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			if(other.gameObject.GetComponent<Inventory>().HasKey()){
				isInside = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			isInside = false;
		}
	}

	public void Enable(){
        if(ObjectForEnemiesSpawn != null){
            for(int i = ObjectForEnemiesSpawn.transform.childCount-1; i >= 0; i--){
                ObjectForEnemiesSpawn.transform.GetChild(i).GetComponent<EnemySurprise>().MakeDamage();
                ObjectForEnemiesSpawn.transform.GetChild(i).parent = null;
            }
            Destroy(ObjectForEnemiesSpawn);
        }
    }

	void SpawnEnemies(){
		ObjectForEnemiesSpawn = new GameObject();
		ObjectForEnemiesSpawn.transform.position = this.transform.position;
		ObjectForEnemiesSpawn.name = "ObjectForEnemiesSpawn";
		
		GameObject enemy1 = GameObject.Instantiate(enemyToSpawn, p1, Quaternion.identity);
		enemy1.transform.parent = ObjectForEnemiesSpawn.transform;
		GameObject enemy2 = GameObject.Instantiate(enemyToSpawn, p2, Quaternion.identity);
		enemy2.transform.parent = ObjectForEnemiesSpawn.transform;
		GameObject enemy3 = GameObject.Instantiate(enemyToSpawn, p3, Quaternion.identity);
		enemy3.transform.parent = ObjectForEnemiesSpawn.transform;
	}
}

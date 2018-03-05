using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public bool isInteractable;
    public GameObject enemyToSpawn;
    private GameObject ObjectForEnemiesSpawn;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    void Start()
    {
        p1 = point1.position;
        p2 = point2.position;
        p3 = point3.position;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
            this.gameObject.SetActive(false);
            ObjectForEnemiesSpawn = new GameObject();
            ObjectForEnemiesSpawn.transform.position = this.transform.position;
            ObjectForEnemiesSpawn.name = "ObjectForEnemiesSpawn";
            
            if(isInteractable){
                //input - ainda vou fazer o input
                //isInteractable geralmente vai ser uma porta
                GameObject enemy1 = GameObject.Instantiate(enemyToSpawn, p1, Quaternion.identity);
                enemy1.transform.parent = ObjectForEnemiesSpawn.transform;
                GameObject enemy2 = GameObject.Instantiate(enemyToSpawn, p2, Quaternion.identity);
                enemy2.transform.parent = ObjectForEnemiesSpawn.transform;
                GameObject enemy3 = GameObject.Instantiate(enemyToSpawn, p3, Quaternion.identity);
                enemy3.transform.parent = ObjectForEnemiesSpawn.transform;
            }
            else{
                GameObject enemy1 = GameObject.Instantiate(enemyToSpawn, p1, Quaternion.identity);
                enemy1.transform.parent = ObjectForEnemiesSpawn.transform;
                GameObject enemy2 = GameObject.Instantiate(enemyToSpawn, p2, Quaternion.identity);
                enemy2.transform.parent = ObjectForEnemiesSpawn.transform;
                GameObject enemy3 = GameObject.Instantiate(enemyToSpawn, p3, Quaternion.identity);
                enemy3.transform.parent = ObjectForEnemiesSpawn.transform;
            }
		}
	}

    public void Enable(){
        this.gameObject.SetActive(true);
        /*foreach(Transform t in ObjectForEnemiesSpawn.transform){
            t.gameObject.GetComponent<EnemySurprise>().MakeDamage();
            t.parent = null;
        }*/
        /*for(int i = ObjectForEnemiesSpawn.transform.childCount; i >= 0; i--){
            ObjectForEnemiesSpawn.transform.GetChild(i).GetComponent<EnemySurprise>().MakeDamage();
            ObjectForEnemiesSpawn.transform.GetChild(i).parent = null;
        }*/
        ObjectForEnemiesSpawn.transform.GetChild(2).GetComponent<EnemySurprise>().MakeDamage();
        ObjectForEnemiesSpawn.transform.GetChild(2).parent = null;
        ObjectForEnemiesSpawn.transform.GetChild(1).GetComponent<EnemySurprise>().MakeDamage();
        ObjectForEnemiesSpawn.transform.GetChild(1).parent = null;
        ObjectForEnemiesSpawn.transform.GetChild(0).GetComponent<EnemySurprise>().MakeDamage();
        ObjectForEnemiesSpawn.transform.GetChild(0).parent = null;
        Destroy(ObjectForEnemiesSpawn);
    }
}

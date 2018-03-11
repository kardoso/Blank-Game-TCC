using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Player player;
    RaycastHit2D hitInArrow;
    RaycastHit2D hitInBox;
    RaycastHit2D hitInEnemy;
    RaycastHit2D hitNormalWall;
    RaycastHit2D hitWallForArrow;
    RaycastHit2D hitInButton;
    float velocity = 1000;

    Vector3 direction;
    bool move;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();
        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            hitInArrow = Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 4, transform.position.y), 1 << LayerMask.NameToLayer("Arrow"));
            hitInBox = Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 4, transform.position.y), 1 << LayerMask.NameToLayer("Box"));
            hitNormalWall = Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 4, transform.position.y), 1 << LayerMask.NameToLayer("Wall"));
            hitWallForArrow = Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 4, transform.position.y), 1 << LayerMask.NameToLayer("WallForArrow"));
            hitInButton = Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 4, transform.position.y), 1 << LayerMask.NameToLayer("Button"));
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            hitInArrow = Physics2D.Linecast(new Vector2(transform.position.x - 1, transform.position.y), new Vector2(transform.position.x - 4, transform.position.y), 1 << LayerMask.NameToLayer("Arrow"));
            hitInBox = Physics2D.Linecast(new Vector2(transform.position.x - 1, transform.position.y), new Vector2(transform.position.x - 4, transform.position.y), 1 << LayerMask.NameToLayer("Box"));
            hitNormalWall = Physics2D.Linecast(new Vector2(transform.position.x - 1, transform.position.y), new Vector2(transform.position.x - 4, transform.position.y), 1 << LayerMask.NameToLayer("Wall"));
            hitWallForArrow = Physics2D.Linecast(new Vector2(transform.position.x - 1, transform.position.y), new Vector2(transform.position.x - 4, transform.position.y), 1 << LayerMask.NameToLayer("WallForArrow"));
            hitInButton = Physics2D.Linecast(new Vector2(transform.position.x - 1, transform.position.y), new Vector2(transform.position.x - 4, transform.position.y), 1 << LayerMask.NameToLayer("Button"));
        }

        if (hitInArrow)
        {
            hitInArrow.collider.gameObject.GetComponent<Animator>().SetBool("Break", true);
            gameObject.transform.position = hitInArrow.collider.gameObject.transform.position;
        }
        else if (hitInBox)
        {
            gameObject.GetComponent<Animator>().SetBool("Break", true);
        }
        else if (hitNormalWall)
        {
            gameObject.GetComponent<Animator>().SetBool("Break", true);
        }
        else if (hitWallForArrow)
        {

        }
        else if(hitInButton){
            
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("Break", true);
        }
    }

    void Update()
    {
        if (move)
        {
            transform.position += direction * Time.deltaTime * velocity;
            //transform.Translate(direction * Time.deltaTime * velocity);
            //GetComponent<Rigidbody2D>().AddForce(direction * velocity);
        }
		
		if(Time.timeScale < 1)
		{
			Destroy(this.gameObject);
		}
    }

    public void SetInitial(bool _move, Vector2 dir)
    {
        move = _move;
        direction = dir;
    }

    public void DestroyArrow()
    {
        Destroy(this.gameObject);
    }
}

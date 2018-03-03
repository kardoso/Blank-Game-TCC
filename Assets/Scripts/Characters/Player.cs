using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    protected Rigidbody2D rb;
    protected Animator anim;

    protected float input_x;
    protected float input_y;
    protected bool invertDirectionalControls;
	protected float walkSpeed = 70;          //Velocidade que o player anda
    protected float maxSpeed = 70;
    protected bool movingRight = true;      //Se está virado para a direita ou não. Usado para inverter o sprite do personagem
    protected bool isGrounded;              //Se o player está no chão ou não
    protected bool canMove;
    
    /***********************************************************/
    //Variáveis para altura do pulo
    [Range(100, 200)]
    public float jumpVelocity = 150;        //velocidade do pulo
    public float fallMultiplier = 20;       //multiplicador de queda
    public float lowJumpMultiplier = 50;    //multiplicador de pulo baixo
    
    /***************************************************/
    
    public bool wallSliding;
    public Transform wallCheckRightPoint;
    public Transform wallCheckLeftPoint;
    public bool wallCheckLeft;
    public bool wallCheckRight;
    public LayerMask wallLayerMask;

    public GameObject arrowIndicatorSphere;
    public Transform bowInitialPointRight;
    public Transform bowInitialPointLeft;
    RaycastHit2D hitForBow;
	RaycastHit2D[] allHits;
	List<RaycastHit2D> objectsInRayHit = new List<RaycastHit2D>();
    bool canUseBow;

    public LayerMask boxLayerMask;
    public Transform boxCheckRightPoint;
    public Transform boxCheckLeftPoint;
    public bool boxCheckRight;
    public bool boxCheckLeft;

	// Use this for initialization
	void Awake () {
        invertDirectionalControls = false;
        canUseBow = true;
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
        if(invertDirectionalControls){
            input_x = -Input.GetAxisRaw("Horizontal");
            input_y = -Input.GetAxisRaw("Vertical");
        }
        else{
            input_x = Input.GetAxisRaw("Horizontal");
            input_y = Input.GetAxisRaw("Vertical");
        }

        isGrounded = Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16.5f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16.5f), 1 << LayerMask.NameToLayer("Floor"))
                    ||
                    Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16.5f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16.5f), 1 << LayerMask.NameToLayer("Arrow"))
                    ||
                    Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16.5f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16.5f), 1 << LayerMask.NameToLayer("Box"))
                    ;

        Debug.DrawLine(new Vector2(transform.position.x - 3.8f, transform.position.y - 16.5f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16.5f));

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", wallSliding);

        if(canMove){
            FlipSprite();
            Jump();
            WallSlide();
            BowAttack();
        }
	}

    void FixedUpdate()
    {
        if(canMove){
            Move();
        }
    }

    void BowAttack(){
        
        float initialX = 0;
        float initialY = 0;
        float initialZ = 0;
        float distance = 0;

        if(movingRight){
            initialX = bowInitialPointRight.position.x;
            initialY = bowInitialPointRight.position.y;
            initialZ = bowInitialPointRight.position.z;
            distance = 500;
        }
        else{
            initialX = bowInitialPointLeft.position.x;
            initialY = bowInitialPointLeft.position.y;
            initialZ = bowInitialPointLeft.position.z;
            distance = -500;
        }

		
		allHits = Physics2D.LinecastAll(new Vector2(initialX, initialY), new Vector2(initialX + distance, initialY));
		Debug.DrawLine(new Vector2(initialX, initialY), new Vector2(initialX + distance, initialY));
		

        //if(hitForBow.collider != null){
            if(((Input.GetButtonDown("Attack") || Input.GetButtonDown("X")) && canUseBow)){
				if(allHits != null){
					foreach (RaycastHit2D rh in allHits)
					{
						objectsInRayHit.Add(rh);
						if(!rh.collider.gameObject.layer.Equals(12)) //12 = Enemy layer
							break;
					}

					objectsInRayHit.Sort(delegate(RaycastHit2D rh1, RaycastHit2D rh2){
						return (rh1.point.x).CompareTo(rh2.point.x);
						});

					/*for(int i = 0; i < objectsInRayHit.Count; i++){
						Debug.Log(objectsInRayHit[i].distance);
					}*/
				}
				//objectsInRayHit.Clear();
				//Se o ultimo ponto da reta toca em algo
				bool hasFinalPoint = objectsInRayHit.Count != 0;
				//Se o ultimo ponto da reta toca em algo e esse algo é um inimigo
				bool hasFinal = hasFinalPoint?hasFinal = !objectsInRayHit[objectsInRayHit.Count-1].collider.gameObject.layer.Equals(12) : hasFinal = false;
				
				//Debug.Log(hasFinal);
				StartCoroutine(ThrowArrow(
                                        objectsInRayHit,
										distance,
										hasFinal?false:true,
										hasFinal?objectsInRayHit.Last().point:new Vector2(initialX+distance/2, initialY), 
										hasFinal?objectsInRayHit.Last().point.x:initialX+distance/2, 
										hasFinal?objectsInRayHit.Last().point.y:initialY,
										initialX, 
										initialY, 
										initialZ));
				
            }
        //}
    }

    IEnumerator ThrowArrow(List<RaycastHit2D> hits,float _distance, bool arrowShouldMove, Vector2 hitForArrow, float spawnX, float spawnY, float initialX, float initialY, float initialZ){
		if(!wallSliding){
            anim.SetTrigger("Attack");
            //Desativar tiro de arco
            canUseBow = false;
            //Desativar movimento
            canMove = false;
            //congelar constraints
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            var ArrowPrototype = Resources.Load("Objects/Arrow", typeof(GameObject)) as GameObject;
            //Definir posição para spawnar a flecha
            Vector3 spawnPos = new Vector3(spawnX, spawnY, initialZ);

            yield return new WaitForSeconds(0.25f);
            //Desenhar a linha
            GetComponent<LineRenderer>().SetPosition(0, new Vector3(initialX, initialY, initialZ));
            GetComponent<LineRenderer>().SetPosition(1, new Vector3(hitForArrow.x, hitForArrow.y, initialZ));
            //Dano nos inimigos
            if(allHits != null){
                foreach (RaycastHit2D rh in hits)
                {
                    if(rh.collider.gameObject.layer.Equals(12)) //12 = Enemy layer
                        rh.collider.gameObject.GetComponent<Enemy>().MakeDamage();
                }

                objectsInRayHit.Sort(delegate(RaycastHit2D rh1, RaycastHit2D rh2){
                    return (rh1.point.x).CompareTo(rh2.point.x);
                    });

                /*for(int i = 0; i < objectsInRayHit.Count; i++){
                    Debug.Log(objectsInRayHit[i].distance);
                }*/
            }
            
            yield return new WaitForSeconds(0.025f);
            //Instanciar flecha
            //GameObject.Instantiate(ArrowPrototype, spawnPos, Quaternion.identity);
            ArrowPrototype.transform.position = spawnPos;
            GameObject.Instantiate(ArrowPrototype).GetComponent<Arrow>().SetInitial(arrowShouldMove, movingRight?Vector2.right:Vector2.left);

            yield return new WaitForSeconds(0.025f);
            //Limpar a linha
            GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            //Desativar a esfera indicadora
            FindObjectOfType<Fade>().FadeGameObject(arrowIndicatorSphere, false, 0.5f);
            //Ativar movimento novamente
            canMove = true;
            
            yield return new WaitForSeconds(0.5f);
            arrowIndicatorSphere.SetActive(false);

            //yield return new WaitForSeconds(0.5f);
            arrowIndicatorSphere.SetActive(true);
            FindObjectOfType<Fade>().FadeGameObject(arrowIndicatorSphere, true, 0.5f);
            //Ativar flecha
            canUseBow = true;
        }
        //Limpar lista dos objetos na linha de colisão da flecha
        objectsInRayHit.Clear();
    }

	public void Move()
    {
        float _actualSpeedX = 0;
        float _maxSpeed = 0;

        boxCheckRight = Physics2D.OverlapCircle(boxCheckRightPoint.position, 0.1f, boxLayerMask);
        boxCheckLeft = Physics2D.OverlapCircle(boxCheckLeftPoint.position, -0.1f, boxLayerMask);

        //Andar
        if (input_x != 0)
        {
            if(boxCheckLeft || boxCheckRight){
                anim.SetBool("Pushing", true);
                _maxSpeed = maxSpeed/8;
            }
            else{
                _maxSpeed = maxSpeed;
                anim.SetBool("Pushing", false);
            }
            _actualSpeedX = walkSpeed;
        }
        else{
            _actualSpeedX = 0;
            anim.SetBool("Pushing", false);
        }

        //Mover
        //transform.position += new Vector3(input_x, 0, 0).normalized * Time.deltaTime * _actualSpeedX;
        rb.AddForce((Vector2.right * _actualSpeedX * 500) * input_x);
        if(rb.velocity.x > _maxSpeed){
            rb.velocity = new Vector2(_maxSpeed, rb.velocity.y);
        }
        if(rb.velocity.x < -_maxSpeed){
            rb.velocity = new Vector2(-_maxSpeed, rb.velocity.y);
        }
        //rb.velocity = new Vector2(input_x * _actualSpeedX, rb.velocity.y);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        anim.SetFloat("xVelocity", _actualSpeedX);
    }

    public void InvetControls(){invertDirectionalControls = !invertDirectionalControls; }

	void Jump()
    {
        if((Input.GetButtonDown("Jump") || Input.GetButtonDown("A")) && !wallSliding){
            if(isGrounded){
                rb.velocity = Vector2.up * jumpVelocity;
            }
        }
        
        //Controlar altura do pulo
        if(rb.velocity.y < 0){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && (!Input.GetButton("Jump") && !Input.GetButton("A"))){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    void WallSlide(){
        if(!isGrounded){
            wallCheckRight = Physics2D.OverlapCircle(wallCheckRightPoint.position, 0.1f, wallLayerMask);
            wallCheckLeft = Physics2D.OverlapCircle(wallCheckLeftPoint.position, -0.1f, wallLayerMask);

            if((wallCheckLeft && input_x < 0) || (wallCheckRight && input_x > 0)){
                HandleWallSliding();
            }
            else{
                wallSliding = false;
            }
        }
    }

    void HandleWallSliding(){
        rb.velocity = new Vector2(rb.velocity.x, -10);
        wallSliding = true;

        if(Input.GetButtonDown("Jump") || Input.GetButtonDown("A")){
            if(wallCheckLeft){
                rb.AddForce(new Vector2(5000, 2500) * jumpVelocity);
            }
            else{
                rb.AddForce(new Vector2(-5000, 2500) * jumpVelocity);
            }
        }
    }

    void FlipSprite(){
        if (input_x > 0)
        {
            if (!movingRight)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                movingRight = !movingRight;
            }
        }
        if (input_x < 0)
        {
            if (movingRight)
            {
                //inverter sprite na horizontal
                GetComponent<SpriteRenderer>().flipX = true;
                movingRight = !movingRight;
            }
        }
    }

    public void MakeDamage(){

    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Desenhar linha de checagem do chão
        Gizmos.DrawLine(new Vector2(transform.position.x - 4f, transform.position.y - 16.5f), new Vector2(transform.position.x + 4f, transform.position.y - 16.5f));
        
        foreach (RaycastHit2D rh in allHits)
		{
			Gizmos.DrawSphere(rh.point, 2);
		}
	}*/
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{

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
    public float fallMultiplier = 30;       //multiplicador de queda
    public float lowJumpMultiplier = 50;    //multiplicador de pulo baixo

    /***************************************************/

    public bool wallSliding;
    public Transform wallCheckRightPoint;
    public Transform wallCheckLeftPoint;
    public bool wallCheckLeft;
    public bool wallCheckRight;
    public LayerMask wallLayerMask;

    //public GameObject arrowIndicatorSphere;
    public Transform bowInitialPointRight;
    public Transform bowInitialPointLeft;
    RaycastHit2D hitForBow;
    RaycastHit2D[] allHits;
    List<RaycastHit2D> objectsInRayHit = new List<RaycastHit2D>();
    bool canUseBow;
    public LayerMask layersForArrow;
    public LineRenderer ArrowLine;

    public LayerMask boxLayerMask;
    public Transform boxCheckRightPoint;
    public Transform boxCheckLeftPoint;
    public bool boxCheckRight;
    public bool boxCheckLeft;

    //Position player to go when "dead"
    public Vector3 posToGo;

    public GameObject bolha;

    // Use this for initialization
    void Awake()
    {
        gameObject.layer = 11;//layer Player
        invertDirectionalControls = false;
        canUseBow = true;
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        canMove = true;
        //"Salvar" a posição inicial do jogador na fase
        FindObjectOfType<LevelManager>().SetPlayerInitialPos(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (Time.timeScale == 1)
            {
                if (invertDirectionalControls)
                {
                    input_x = -Input.GetAxisRaw("Horizontal");
                    input_y = -Input.GetAxisRaw("Vertical");
                }
                else
                {
                    input_x = Input.GetAxisRaw("Horizontal");
                    input_y = Input.GetAxisRaw("Vertical");
                }

                isGrounded = Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16f), 1 << LayerMask.NameToLayer("Floor"))
                            ||
                            Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16f), 1 << LayerMask.NameToLayer("Arrow"))
                            ||
                            Physics2D.Linecast(new Vector2(transform.position.x - 3.8f, transform.position.y - 16f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16f), 1 << LayerMask.NameToLayer("Box"))
                            ;

                Debug.DrawLine(new Vector2(transform.position.x - 3.8f, transform.position.y - 16f), new Vector2(transform.position.x + 3.9f, transform.position.y - 16f));

                anim.SetBool("isGrounded", isGrounded);
                anim.SetBool("isSliding", wallSliding);

                FlipSprite();
                Jump();
                WallSlide();
                BowAttack();
            }
        }
        else
        {
            /********* CHANGE ANIMATOR TO IDLE *******/
            anim.SetFloat("xVelocity", 0);
            anim.SetFloat("yVelocity", -1);
            anim.SetBool("isGrounded", false);
            anim.SetBool("isSliding", false);
            /********************************************/
            /*
            //Voltar para a posicao inicial sem tempo definido, em uma velocidade constante
            if ((Time.timeScale < 1 && Time.timeScale > 0) && !canMove)
            {
                //Faz a animação usar UnscaledTime, fazendo com que não dependa do Time.deltaTime
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;

                float step = (walkSpeed * 2f) * Time.unscaledDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, posToGo, step);

                if (transform.position == posToGo)
                {
                    anim.updateMode = AnimatorUpdateMode.Normal;
                    FindObjectOfType<LevelManager>().TimeInNormal();
                    ImBack();
                }
            }*/
        }
    }

    void FixedUpdate()
    {
        if (canMove && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (Time.timeScale == 1)
            {
                Move();
            }
        }
    }

    void BowAttack()
    {

        float initialX = 0;
        float initialY = 0;
        float initialZ = 0;
        float distance = 0;

        if (movingRight)
        {
            initialX = bowInitialPointRight.position.x;
            initialY = bowInitialPointRight.position.y;
            initialZ = bowInitialPointRight.position.z;
            distance = 500;
        }
        else
        {
            initialX = bowInitialPointLeft.position.x;
            initialY = bowInitialPointLeft.position.y;
            initialZ = bowInitialPointLeft.position.z;
            distance = -500;
        }


        allHits = Physics2D.LinecastAll(new Vector2(initialX, initialY), new Vector2(initialX + distance, initialY), layersForArrow);
        Debug.DrawLine(new Vector2(initialX, initialY), new Vector2(initialX + distance, initialY));


        //if(hitForBow.collider != null){
        if (((Input.GetButtonDown("Attack") || Input.GetButtonDown("X")) && canUseBow))
        {
            if (allHits != null)
            {
                foreach (RaycastHit2D rh in allHits)
                {
                    objectsInRayHit.Add(rh);
                    if(rh.collider.gameObject.layer.Equals(8) || rh.collider.gameObject.layer.Equals(13)){
                        break;
                    }
                }

                objectsInRayHit.Sort(delegate (RaycastHit2D rh1, RaycastHit2D rh2)
                {
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
            bool hasFinal = hasFinalPoint ? hasFinal = !objectsInRayHit[objectsInRayHit.Count - 1].collider.gameObject.layer.Equals(12) : hasFinal = false;

            //Debug.Log(hasFinal);
            StartCoroutine(ThrowArrow(
                                    objectsInRayHit,
                                    distance,
                                    hasFinal ? false : true,
                                    hasFinal ? objectsInRayHit.Last().point : new Vector2(initialX + distance, initialY),
                                    hasFinal ? objectsInRayHit.Last().point.x : initialX + distance,
                                    hasFinal ? objectsInRayHit.Last().point.y : initialY,
                                    initialX,
                                    initialY,
                                    initialZ));
        }
    }

    IEnumerator ThrowArrow(List<RaycastHit2D> hits, float _distance, bool arrowShouldMove, Vector2 hitForArrow, float spawnX, float spawnY, float initialX, float initialY, float initialZ)
    {
        if (!wallSliding)
        {
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
            //criar linha
            if (Time.timeScale >= 1)
            {
                ArrowLine.SetPosition(0, new Vector3(initialX, initialY, initialZ));
                ArrowLine.SetPosition(1, new Vector3(hitForArrow.x, hitForArrow.y, initialZ));
            }
            //Dano nos inimigos
            if ((allHits != null) && Time.timeScale >= 1)
            {
                foreach (RaycastHit2D rh in hits)
                {
                    if (rh.collider.gameObject.layer.Equals(12)) //12 = Enemy layer
                        rh.collider.gameObject.GetComponent<Enemy>().MakeDamage();
                    if(rh.collider.gameObject.layer.Equals(20))
                        rh.collider.gameObject.GetComponent<ButtonArrow>().EnableButton();
                }

                objectsInRayHit.Sort(delegate (RaycastHit2D rh1, RaycastHit2D rh2)
                {
                    return (rh1.point.x).CompareTo(rh2.point.x);
                });

                /*for(int i = 0; i < objectsInRayHit.Count; i++){
                    Debug.Log(objectsInRayHit[i].distance);
                }*/
            }

            yield return new WaitForSeconds(0.025f);
            if (Time.timeScale >= 1)
            {
                //Instanciar flecha
                //GameObject.Instantiate(ArrowPrototype, spawnPos, Quaternion.identity);
                ArrowPrototype.transform.position = spawnPos;
                GameObject.Instantiate(ArrowPrototype).GetComponent<Arrow>().SetInitial(arrowShouldMove, movingRight ? Vector2.right : Vector2.left);
            }

            yield return new WaitForSeconds(0.025f);
            //Limpar a linha
            ArrowLine.SetPosition(0, Vector3.zero);
            ArrowLine.SetPosition(1, Vector3.zero);
            //Desativar a esfera indicadora
            //FindObjectOfType<Fade>().FadeGameObject(arrowIndicatorSphere, 0.5f, 1, 0);
            //Ativar movimento novamente
            if(Time.timeScale >= 1){canMove = true;}

            yield return new WaitForSeconds(0.5f);
            //arrowIndicatorSphere.SetActive(false);

            //yield return new WaitForSeconds(0.5f);
            //arrowIndicatorSphere.SetActive(true);
            //FindObjectOfType<Fade>().FadeGameObject(arrowIndicatorSphere, 0.5f, 0, 1);
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
        float _velocityMultiplier = 0;

        boxCheckRight = Physics2D.OverlapCircle(boxCheckRightPoint.position, 0.1f, boxLayerMask);
        boxCheckLeft = Physics2D.OverlapCircle(boxCheckLeftPoint.position, -0.1f, boxLayerMask);

        var inLeft = Physics2D.Linecast(new Vector2(transform.position.x - 4f, transform.position.y - 15f), new Vector2(transform.position.x - 4f, transform.position.y + 1), 1 << LayerMask.NameToLayer("Floor"));
        var inRight = Physics2D.Linecast(new Vector2(transform.position.x + 4.1f, transform.position.y - 15f), new Vector2(transform.position.x + 4.1f, transform.position.y + 1), 1 << LayerMask.NameToLayer("Floor"));
        Debug.DrawLine(new Vector2(transform.position.x - 4f, transform.position.y - 15f), new Vector2(transform.position.x - 4f, transform.position.y + 1));
        Debug.DrawLine(new Vector2(transform.position.x + 4.1f, transform.position.y - 15f), new Vector2(transform.position.x + 4.1f, transform.position.y + 1));

        //Andar
        if (input_x != 0)
        {
            if (boxCheckLeft || boxCheckRight)
            {
                anim.SetBool("Pushing", true);
                _maxSpeed = maxSpeed / 8;
                _velocityMultiplier = 1;
            }
            else if((inRight && input_x > 0) && !isGrounded){
                _actualSpeedX = 0;
                _velocityMultiplier = 0;
            }
            else if((inLeft && input_x < 0) && !isGrounded){
                _actualSpeedX = 0;
                _velocityMultiplier = 0;
            }
            else
            {
                _maxSpeed = maxSpeed;
                anim.SetBool("Pushing", false);
                _velocityMultiplier = 1;
            }
            _actualSpeedX = walkSpeed;
        }
        else
        {
            _actualSpeedX = 0;
            _velocityMultiplier = 0;
            anim.SetBool("Pushing", false);
        }

        //Mover
        //transform.position += new Vector3(input_x, 0, 0).normalized * Time.deltaTime * _actualSpeedX;
        rb.AddForce((Vector2.right * _actualSpeedX * 500) * input_x * _velocityMultiplier);
        if (rb.velocity.x > _maxSpeed)
        {
            rb.velocity = new Vector2(_maxSpeed, rb.velocity.y);
        }
        if (rb.velocity.x < -_maxSpeed)
        {
            rb.velocity = new Vector2(-_maxSpeed, rb.velocity.y);
        }
        //rb.velocity = new Vector2(input_x * _actualSpeedX, rb.velocity.y);

        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        anim.SetFloat("xVelocity", _actualSpeedX);
    }

    public void InvetControls() { invertDirectionalControls = !invertDirectionalControls; }
    public void NormalizeControls(){ invertDirectionalControls = false; }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("A")) && !wallSliding)
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.up * jumpVelocity;
            }
        }

        //Controlar altura do pulo
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && (!Input.GetButton("Jump") && !Input.GetButton("A")))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    void WallSlide()
    {
        if (!isGrounded)
        {
            wallCheckRight = Physics2D.OverlapCircle(wallCheckRightPoint.position, 0.1f, wallLayerMask);
            wallCheckLeft = Physics2D.OverlapCircle(wallCheckLeftPoint.position, -0.1f, wallLayerMask);

            if ((wallCheckLeft && input_x < 0) || (wallCheckRight && input_x > 0))
            {
                HandleWallSliding();
            }
            else
            {
                wallSliding = false;
            }
        }
    }

    void HandleWallSliding()
    {
        rb.velocity = new Vector2(rb.velocity.x, -10);
        wallSliding = true;

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("A"))
        {
            if (wallCheckLeft)
            {
                rb.AddForce(new Vector2(5000, 2500) * jumpVelocity);
            }
            else
            {
                rb.AddForce(new Vector2(-5000, 2500) * jumpVelocity);
            }
        }
    }

    void FlipSprite()
    {
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

    public void MakeDamage()
    {
        canMove = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        FindObjectOfType<LevelManager>().TimeInDeath();
        posToGo = FindObjectOfType<LevelManager>().GetPlayerInitialPos();
        bolha.GetComponent<Bubble>().BubbleOn();
        //transform.position = FindObjectOfType<LevelManager>().GetPlayerInitialPos();
    }

    public void ImBack(){
        NormalizeControls();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.isKinematic = false;
        GetComponent<BoxCollider2D>().enabled = true;
        canMove = true;
    }

    public void StopMovement(){
        canMove = false;
    }

    //Voltar para a posicao inicial em tempo x, com velocidade variavel
    public IEnumerator MoveToInitialPosition(float timeToMove)
    {
        //Faz a animação usar UnscaledTime, fazendo com que não dependa do Time.deltaTime
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;

        var currentPos = transform.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, posToGo, t);
            yield return null;
        }
        bolha.GetComponent<Bubble>().BubbleOff();
    }

    public void InitialPositionDone(){
        anim.updateMode = AnimatorUpdateMode.Normal;
        FindObjectOfType<LevelManager>().TimeInNormal();
        ImBack();
    }

    //Check side collider
    //And change the velocity.x to 0 when colliding with something in side on Move() function

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
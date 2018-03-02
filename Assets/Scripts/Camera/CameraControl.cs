 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    //VARIÁVEIS PARA DEFINIR OS LIMITES DA CÂMERA DENTRO DE CADA AMBIENTE
    //Variáveis para limitar pontos máximos e mínimos da câmera
    public float minX;          //Localização mínima em X
    public float maxX;          //Localização máxima em X
    public float minY;          //Localização mínima em Y
    public float maxY;          //Localização máxima em Y

    //VARIÁVEIS PARA FAZER A CÂMERA SEGUIR O PLAYER
    private float dampTime = 0f;    //Tempo de atraso que a câmera seguirá o jogador
    private Transform target;       //O alvo que a câmera seguirá
    private Vector3 velocidade;     //A velocidade da câmera

    void Start()
    {
        //Pegar o transform do player procurando o objeto com a tag "Player"
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //Definir a variável "velocidade" com a velocidade da câmera
        velocidade = transform.position;
    }

    void Update()
    {
        FollowPlayer();

        #if UNITY_EDITOR
        DebugDrawLimits();
        #endif
    }

    private void LateUpdate()
    {
        LimitCameraBounds();
    }

    //SEGUIR O PLAYER
    private void FollowPlayer()
    {
        //Seguir o player
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destino = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velocidade, dampTime);
    }

    //DEFINIR LIMITES DA CAMERA
    private void LimitCameraBounds()
    {
        //Limitar o movimento da câmera entre os limites
        //Pegar a posição da câmera
        //Variável Vector3 terá 3 valores: X, Y e Z
        Vector3 coord = transform.position;
        //Mathf.Clamp define um valor mínimo e máximo
        //Limitar no eixo X
        coord.x = Mathf.Clamp(coord.x, minX, maxX);
        //Limitar no eixo Y
        coord.y = Mathf.Clamp(coord.y, minY, maxY);
        //Definir a posição em Z da câmera. Padrão: -10
        coord.z = -10;
        //Setar a posição da câmera de acordo com a variável "coord"
        transform.position = coord;
    }

    //Função para definir os limites da câmera. (Função para ser chamada em outro script)
    public void SetBounds(float _minX, float _maxX, float _minY,float _maxY)
    {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }

    public float GetMinX(){return minX;}
    public float GetMaxX(){return maxX;}
    public float GetMinY(){return minY;}
    public float GetMaxY(){return maxY;}

    //Retorna o objeto que a câmera está focando
    public GameObject GetTarget(){
        return target.gameObject;
    }

    //Define um objeto para a camera focar
    public void SetTarget(Transform newTarget){
        target = newTarget;
    }

    //Faz a câmera focar no player depois de 0.5 segundos
    public IEnumerator TargetPlayer(){
        yield return new WaitForSeconds(.5f);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void DebugDrawLimits()
    {
        //Limite superior
        Debug.DrawLine(new Vector3(minX - (Camera.main.orthographicSize * 1.78f), maxY + (Camera.main.orthographicSize), 0), new Vector3(maxX + (Camera.main.orthographicSize * 1.78f), maxY + (Camera.main.orthographicSize), 0), Color.blue);
        //Limite Inferior
        Debug.DrawLine(new Vector3(minX - (Camera.main.orthographicSize * 1.78f), minY - (Camera.main.orthographicSize), 0), new Vector3(maxX + (Camera.main.orthographicSize * 1.78f), minY - (Camera.main.orthographicSize), 0), Color.blue);
        //Limite Esquerdo
        Debug.DrawLine(new Vector3(minX - (Camera.main.orthographicSize * 1.78f), minY - (Camera.main.orthographicSize), 0), new Vector3(minX - (Camera.main.orthographicSize * 1.78f), maxY + (Camera.main.orthographicSize), 0), Color.blue);
        //Limite Direito
        Debug.DrawLine(new Vector3(maxX + (Camera.main.orthographicSize * 1.78f), minY - (Camera.main.orthographicSize), 0), new Vector3(maxX + (Camera.main.orthographicSize * 1.78f), maxY + (Camera.main.orthographicSize), 0), Color.blue);
    }
}

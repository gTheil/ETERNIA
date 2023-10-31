using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform lookAt; // o objeto em cena no qual a câmera estará centralizada

    //private const float boundX = 0.6f; // o limite de distância (no eixo X) que o objeto pode se afastar do centro da câmera antes dela começar a seguí-lo
    //private const float boundY = 0.3f; // o limite de distância (no eixo Y) que o objeto pode se afastar do centro da câmera antes dela começar a seguí-lo

    //variaveis com nivel public para teste
    public float boundX;
    public float boundY;

    // inicializa a câmera centralizada no jogador
    private void Start() {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        SetCameraTarget(player.transform);
    }

    private void LateUpdate() {
        Vector3 cameraDelta = Vector3.zero;

        // verifica a distância (no eixo X) entre o centro da câmera e o objeto no qual ela está centrlizada
        // caso o valor seja maior que o limite de distância no eixo X, a posição da câmera sejá ajustada nesse eixo
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX) {
            if (transform.position.x < lookAt.position.x)
                cameraDelta.x = deltaX - boundX;
            else
                cameraDelta.x = deltaX + boundX;
        }

        // verifica a distância (no eixo Y) entre o centro da câmera e o objeto no qual ela está centrlizada
        // caso o valor seja maior que o limite de distância no eixo Y, a posição da câmera sejá ajustada nesse eixo
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY) {
            if (transform.position.y < lookAt.position.y)
                cameraDelta.y = deltaY - boundY;
            else
                cameraDelta.y = deltaY + boundY;
        }

        transform.position += new Vector3(cameraDelta.x, cameraDelta.y, 0); // atualiza a posição da câmera para que possa seguir o objeto
    }

    // função utilizada para alterar o objeto no qual a câmera está centralizada
    public void SetCameraTarget(Transform cameraTarget) {
        lookAt = cameraTarget;
    }
}

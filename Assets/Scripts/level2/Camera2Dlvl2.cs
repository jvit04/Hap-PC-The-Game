using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2Dlvl2 : MonoBehaviour
{
    public Transform targetPlayer;
    
    [Header("Limites de Camara")]
    public bool camaraEstatica = true;
    public float minX = -5f;
    public float maxX = 5f;

    void Update()
    {
        if (targetPlayer == null) return;

        if (camaraEstatica)
        {
            // La cámara se queda anclada en el centro para ver toda la arena
            transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            // El jugador se mueve, pero la cámara no pasa de los límites establecidos
            float clampedX = Mathf.Clamp(targetPlayer.position.x + 6f, minX, maxX);
            transform.position = new Vector3(clampedX, 0, -10);
        }
    }
}
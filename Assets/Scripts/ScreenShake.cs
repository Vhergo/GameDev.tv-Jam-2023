using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;
    private bool isShaking;
    private Vector3 initialPosition;

    void Start() {
        initialPosition = transform.position;
    }

    void Update() {
        isShaking = (player.IsBeamActive() && !isShaking) ? true : false;
        if (isShaking) {
            ShakeScreen();
        }else {
            isShaking = false;
            transform.position = initialPosition;
        }
    }

    void ShakeScreen() {
        float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
        float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;
        
        transform.position = initialPosition + new Vector3(xOffset, yOffset, 0);
    }
}

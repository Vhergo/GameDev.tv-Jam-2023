using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPCrystal : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private PlayerController playerScript;

    [Range(0, 1)] [SerializeField] private float moveSpeedBonus; // up to double if set to 1
    [SerializeField] private float collectionDelay;
    private float collectionDelayCounter = 0;
    private GameObject player;
    private Transform targetPos;
    private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        targetPos = player.transform;
        playerScript = player.GetComponent<PlayerController>();

        moveSpeed = playerScript.GetPlayerSpeed() * (1 + moveSpeedBonus);
    }

    void Update()
    {
        if (collectionDelayCounter < collectionDelay) collectionDelayCounter += Time.deltaTime;
        if (collectionDelayCounter > collectionDelay) FollowPlayer();
    }

    void FollowPlayer() {
        Vector2 direction = targetPos.position - transform.position;
        rb.velocity = direction.normalized * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D col) {
        GameObject colObject = col.gameObject;
        if (colObject.tag == "Player") {
            playerScript.GainXP(gameObject);
        }
    }
}

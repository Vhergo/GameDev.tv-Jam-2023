using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 lookDirection;

    [SerializeField] private GameObject XPCrystal;
    [SerializeField] private GameObject explosion;

    private Transform targetPos;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        targetPos = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        FollowTarget();
        LookRotation();
    }

    void FollowTarget() {
        Vector2 direction = targetPos.position - transform.position;
        rb.velocity = direction.normalized * moveSpeed;
    }

    void DropXP() {
        Instantiate(XPCrystal, transform.position, Quaternion.identity);
        BlowUp();
    }

    void BlowUp() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f); // slight delay so cyrstals have time to spawn
    }

    void OnCollisionEnter2D(Collision2D col) {
        GameObject colObject = col.gameObject;
        if (colObject.tag == "Player") {
            BlowUp();
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        GameObject colObject = col.gameObject;
        if (colObject.tag == "Attack") {
            print("DROP XP");
            DropXP();
        }
    }

    void LookRotation() {
        lookDirection = targetPos.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}

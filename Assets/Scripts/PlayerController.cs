using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Components and Objects")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject attackBase;
    [SerializeField] private GameObject attackBeam;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private UIManager UIManager;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private Vector2 mousePos;

    [Header("XP")]
    [SerializeField] private int playerLevel;
    [SerializeField] private float currentXP;
    [SerializeField] private float maxXP;
    [SerializeField] private float XPGainPerCrystal;
    [SerializeField] private TMP_Text playerLevelDisplay;

    // Upgrade Multipliers
    //Player
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float attackDurationMultiplier;
    [SerializeField] private float beamScaleMultiplier;

    //Enemy
    [SerializeField] private float enemySpeedMultiplier;
    [SerializeField] private float enemySpawnMultiplier;

    [Range(0, 1)] [SerializeField] private float maxXPUpgradeMultiplier;
    [SerializeField] private float playerUpgradeMultiplier;
    [SerializeField] private float enemyUpgradeMultiplier;
    [SerializeField] private float spawnIntervalIncrease;

    [Header("Attack")]
    [SerializeField] private float beamSpeed;
    [SerializeField] private float beamLength;
    [SerializeField] private float attackDuration;
    [Range(0, 1)] [SerializeField] private float beamMoveSpeedDebuff;
    [Range(0, 1)] [SerializeField] private float beamRotateDebuff;

    [SerializeField] private float baseAttackScale;
    [SerializeField] private float scaleDuration;
    private Vector3 beamScale;
    private Vector3 currentScale;
    private float scaleTimer = 0;
    private float amplifiedBaseAttackScale;
    private float attackDurationCounter;
    private bool isAttacking = false;
    private bool attackSpawned = false;
    private bool beamSpawned = false;
    private bool rotationDebuffActive = false;

    private float currentBeamLength = 0;
    
    void Start() {
        beamScale = attackBase.transform.localScale;
        attackDurationCounter = attackDuration;
        amplifiedBaseAttackScale = baseAttackScale;

        playerLevelDisplay.text = playerLevel.ToString();
    }

    void Update() {
        ProccessInput();
        LookRotation();

    }

    void FixedUpdate() {
        Move();
        if (isAttacking) {
            Attack();
        }else {
            StopAttack();
        }
    }

    void ProccessInput() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;

        isAttacking = (Input.GetMouseButton(0)) ? true : false;
    }

    void Move() {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void Attack() {
        if (!attackSpawned)  {
            attackSpawned = true;

            attackBase.SetActive(true);

        }else {
            scaleTimer += Time.deltaTime;
            float t = Mathf.Clamp01(scaleTimer / scaleDuration);
            if (t < 1) {
                currentScale = Vector3.Lerp(beamScale, beamScale * amplifiedBaseAttackScale, t);
                attackBase.transform.localScale = currentScale;
            }else {
                if (attackDurationCounter > 0) {
                    ActivateDebuff();
                    if (currentBeamLength <= beamLength) currentBeamLength += beamSpeed * 50 * Time.deltaTime;
                    attackBeam.transform.localScale = new Vector3(currentBeamLength, 1f, 1f);
                    attackBeam.transform.localPosition = new Vector3(currentBeamLength / 2, 0, 0);

                    attackDurationCounter -= Time.deltaTime;
                }else {
                    StopAttack();
                }
            }
        }
    }

    void StopAttack() {
        attackSpawned = false;
        beamSpawned = false;

        scaleTimer = 0;
        currentBeamLength = 0;
        attackDurationCounter = attackDuration;
        attackBeam.transform.localScale = new Vector3(currentBeamLength, 1f, 1f);
        if (rotationDebuffActive) {
            moveSpeed /= beamMoveSpeedDebuff;
            rotationSpeed /= beamRotateDebuff;
            rotationDebuffActive = false;
        }
        attackBase.SetActive(false);
    }

    void ActivateDebuff() {
        beamSpawned = true;
        if (!rotationDebuffActive) {
            rotationDebuffActive = true;
            moveSpeed *= beamMoveSpeedDebuff;
            rotationSpeed *= beamRotateDebuff;
        }
    }

    public void GainXP(GameObject colObject) {
        currentXP += XPGainPerCrystal;
        UIManager.UpdateXPBar(maxXP, currentXP);
        if (currentXP >= maxXP) LevelUpManager();

        Destroy(colObject);
    }

    void LevelUpManager() {
        playerLevel++;
        playerLevelDisplay.text = playerLevel.ToString();
        float extraXP = currentXP - maxXP;
        currentXP = extraXP;
        maxXP *= 1 + maxXPUpgradeMultiplier;

        PlayerUpgrades();
        EnemyUpgrades();
    }

    void PlayerUpgrades() {
        moveSpeed *= 1 + playerUpgradeMultiplier;
        attackDuration *= 1 + playerUpgradeMultiplier;
        amplifiedBaseAttackScale *= 1 + playerUpgradeMultiplier;
    }

    void EnemyUpgrades() {
        moveSpeed *= 1 + enemyUpgradeMultiplier;
        enemySpawner.SetSpawnInterval(spawnIntervalIncrease);
    }

    public float GetPlayerSpeed() {
        return moveSpeed;
    }

    public bool IsBeamActive() {
        return beamSpawned;
    }

    void LookRotation() {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}

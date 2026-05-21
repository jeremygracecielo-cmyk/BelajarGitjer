using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHP = 100;
    public float currentHP;
    public float speed = 5f;

    [Header("Combat")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float damageCooldown = 0.5f; 
    
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private float attackInput;
    private float previousAttackInput;
    private float nextDamageTime;

    void Awake()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Playing) return;
        
        if (playerInput == null) return;

        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime);

        attackInput = playerInput.actions["Attack"].ReadValue<float>();
        if (previousAttackInput == 0 && attackInput > 0)
        {
            Shoot();
        }
        previousAttackInput = attackInput;
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position;
        
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;
        
        Vector3 shootDirection = (mouseWorldPos - spawnPos).normalized;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
 
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (Time.time >= nextDamageTime)
            {
                TakeDamage(10f); 
                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            GameManager.Instance.GameOver();
        }
    }
}
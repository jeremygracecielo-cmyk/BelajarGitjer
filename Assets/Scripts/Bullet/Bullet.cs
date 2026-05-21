using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    private Rigidbody2D rb;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (rb != null)
            rb.linearVelocity = direction * speed;
        else
            transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDir) => direction = newDir.normalized;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) return;

        Destroy(gameObject);
    }
}
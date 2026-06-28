using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float lazerSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;

    public float maxLength;
    public float increaseRate;

    void Start()
    {
        Destroy(gameObject, 8f);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * lazerSpeed * Time.deltaTime);

        Vector3 scale = transform.localScale;
        if (scale.x < maxLength)
        {
            scale.x = Mathf.Min(scale.x + increaseRate * Time.deltaTime, maxLength);
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (string.IsNullOrEmpty(fireTag)) return;
        if (collision.CompareTag(fireTag)) return;
        if (collision.isTrigger) return;  // Don't hit detection range colliders

        Debug.Log("Lazer hit: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            PlayerStats ps = collision.GetComponent<PlayerStats>();
            if (ps != null) ps.TakeDamage(damageDelt);
        }
        else if (collision.CompareTag("Enemy"))
        {
            spawnEnemy enemy = collision.GetComponent<spawnEnemy>();
            if (enemy != null) enemy.HP -= damageDelt;
        }

        Destroy(gameObject);
    }
}

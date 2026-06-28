using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;
    AudioManager audioManager;


    private void Start()
    {
        Destroy(gameObject, 5f);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore the shooter's own tag AND ignore other bullets/projectiles
        if (string.IsNullOrEmpty(fireTag)) return;
        if (collision.CompareTag(fireTag)) return;
        if (collision.isTrigger) return;  // Don't hit other trigger colliders (detection ranges etc)

        Debug.Log("Bullet hit: " + collision.name + " tag: " + collision.tag);

        if (collision.CompareTag("Player"))
        {
            PlayerStats ps = collision.GetComponent<PlayerStats>();
            if (ps != null) ps.TakeDamage(damageDelt);

            audioManager.PlaySFX(audioManager.characterdamaged);
        }
        else if (collision.CompareTag("Enemy"))
        {
            spawnEnemy enemy = collision.GetComponent<spawnEnemy>();
            if (enemy != null) enemy.HP -= damageDelt;
        }

        Destroy(gameObject);
    }
}

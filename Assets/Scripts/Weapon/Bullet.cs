using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;
    private AudioManager audioManager;

    private void Start()
    {
<<<<<<< HEAD
        Destroy(gameObject, 5);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
=======
        Destroy(gameObject, 5f);
>>>>>>> 068a08e0afaee0f2f66cde3070512ac5cb1e89c9
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< HEAD
        // Check to ensure it is not friendly fire
        if (fireTag != collision.tag)
        {
            // Destroy bullet on hit
            Destroy(gameObject);

            if (collision.tag == "Player")
            {
                // Deal damage
                collision.gameObject.GetComponent<PlayerStats>().baseHP -= damageDelt;

                // Play alienshoot sound
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.alienshoot);
                }
            }
            else if (collision.tag == "Enemy")
            {
                collision.gameObject.GetComponent<spawnEnemy>().HP -= damageDelt;
            }
        }
=======
        // Ignore the shooter's own tag AND ignore other bullets/projectiles
        if (string.IsNullOrEmpty(fireTag)) return;
        if (collision.CompareTag(fireTag)) return;
        if (collision.isTrigger) return;  // Don't hit other trigger colliders (detection ranges etc)

        Debug.Log("Bullet hit: " + collision.name + " tag: " + collision.tag);

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
>>>>>>> 068a08e0afaee0f2f66cde3070512ac5cb1e89c9
    }
}
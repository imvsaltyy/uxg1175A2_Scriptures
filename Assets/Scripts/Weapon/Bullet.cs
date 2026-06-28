using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;
    AudioManager audioManager;


    private void Start()
    {
<<<<<<< Updated upstream
<<<<<<< HEAD
        Destroy(gameObject, 5);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
=======
        Destroy(gameObject, 5f);
>>>>>>> 068a08e0afaee0f2f66cde3070512ac5cb1e89c9
=======
        Destroy(gameObject, 5f);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

>>>>>>> Stashed changes
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< Updated upstream
<<<<<<< HEAD
        // Check to ensure it is not friendly fire
        if (fireTag != collision.tag)
=======
        // If bullet hits wall, destroy immediately
        if (collision.CompareTag("Wall"))
>>>>>>> Stashed changes
        {
            Destroy(gameObject);
            return;
        }
<<<<<<< Updated upstream
=======
=======


>>>>>>> Stashed changes
        // Ignore the shooter's own tag AND ignore other bullets/projectiles
        if (string.IsNullOrEmpty(fireTag)) return;
        if (collision.CompareTag(fireTag)) return;
        if (collision.isTrigger) return;  // Don't hit other trigger colliders (detection ranges etc)

        Debug.Log("Bullet hit: " + collision.name + " tag: " + collision.tag);

        if (collision.CompareTag("Player"))
        {
            PlayerStats ps = collision.GetComponent<PlayerStats>();
            if (ps != null) ps.TakeDamage(damageDelt);
<<<<<<< Updated upstream
=======

            audioManager.PlaySFX(audioManager.characterdamaged);
>>>>>>> Stashed changes
        }
        else if (collision.CompareTag("Enemy"))
        {
            spawnEnemy enemy = collision.GetComponent<spawnEnemy>();
            if (enemy != null) enemy.HP -= damageDelt;
        }

        Destroy(gameObject);
<<<<<<< Updated upstream
>>>>>>> 068a08e0afaee0f2f66cde3070512ac5cb1e89c9
=======
>>>>>>> Stashed changes
    }
}

using System.Collections;
using UnityEngine;

// Attach this to the walker's attack child object (the one with BoxCollider2D).
// It reads the parent enemy's damage value and applies it to the player.
public class Damage : MonoBehaviour
{
    // How much damage this collider deals — set automatically from parent spawnEnemy
    private float damageAmount = 0f;

    private void Start()
    {
        // Pull the damage value from the parent enemy
        spawnEnemy parentEnemy = GetComponentInParent<spawnEnemy>();
        if (parentEnemy != null)
            damageAmount = parentEnemy.damage;
        else
            Debug.LogWarning("Damage.cs: no spawnEnemy found in parent.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerStats ps = collision.GetComponent<PlayerStats>();
        if (ps == null) return;

        ps.TakeDamage(damageAmount);
        StartCoroutine(DamageFlash(collision));

        Debug.Log("Walker dealt " + damageAmount + " damage to player.");
    }

    IEnumerator DamageFlash(Collider2D collision)
    {
        if (collision == null) yield break;

        SpriteRenderer sr = collision.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) yield break;

        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }
}

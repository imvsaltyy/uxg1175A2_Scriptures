using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    [HideInInspector] public string enemyTypeName;

    #region Enemy Stats
    [HideInInspector] public string ID;
    [HideInInspector] public string variantID;
    [HideInInspector] public string lootTableID;
    [HideInInspector] public float HP;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float detectionRange;

    public List<GameObject> enemyDropPrefabs;
    #endregion

    [HideInInspector] public Collider2D attackCollider;

    #region Enemy State
    public bool idleState = true;
    public bool chasedState = false;
    public bool attackState = false;
    public bool isAlive = true;
    public bool isDead = false;

    public float distance;
    [HideInInspector] public GameObject player;
    public Coroutine Atk;
    #endregion

    protected virtual void Start()
    {
        statsAssignment();
        player = GameObject.FindWithTag("Player");

        if (LevelManager.Instance != null)
        {
            HP *= LevelManager.Instance.enemyHPMultiplier;
            damage *= LevelManager.Instance.enemyDmgMultiplier;
            speed *= LevelManager.Instance.enemySpeedMultiplier;
        }

        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null) circle.radius = detectionRange;
    }

    protected virtual void Update()
    {
        if (player == null || !isAlive) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackRange && idleState && !attackState)
        {
            idleState = false;
            chasedState = false;
            attackState = true;
        }

        if (distance > detectionRange && !idleState)
        {
            idleState = true;
            chasedState = false;
            attackState = false;
        }

        if (HP <= 0 && isAlive)
        {
            isAlive = false;
            isDead = true;
            StopAllCoroutines();
            enemyDead();
            return;
        }

        if (chasedState && !idleState && !attackState) playerChase();
        if (attackState && !chasedState && Atk == null) enemyAttack();
    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.enemyType.Length; i++)
        {
            string[] cols = GameManager.enemyType[i].Split(',');
            if (cols.Length < 9) continue;

            if (cols[0].Trim() == enemyTypeName)
            {
                ID = cols[0].Trim();
                variantID = cols[1].Trim();
                HP = float.Parse(cols[2].Trim());
                damage = float.Parse(cols[3].Trim());
                speed = float.Parse(cols[4].Trim());
                attackRange = float.Parse(cols[5].Trim());
                detectionRange = float.Parse(cols[7].Trim());
                lootTableID = cols[8].Trim();
                assigned = true;
                Debug.Log(ID + " stats assigned. HP=" + HP + " dmg=" + damage);
                break;
            }
        }

        if (!assigned)
        {
            Debug.LogError("Enemy type not found: " + enemyTypeName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !chasedState && idleState)
        {
            idleState = false;
            chasedState = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !chasedState && idleState)
        {
            idleState = false;
            chasedState = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && chasedState && !idleState)
        {
            idleState = true;
            chasedState = false;
        }
    }

    public void playerChase()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (distance <= attackRange && !attackState)
        {
            chasedState = false;
            attackState = true;
        }
    }

    public virtual void enemyAttack() { }

    public void enemyDead()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.EnemyDied();

        enemyDrop();
        Debug.Log("Enemy Dead: " + ID);
        Destroy(gameObject);
    }

    public void enemyDrop()
    {
        if (GameManager.enemyDrop == null || GameManager.enemyDrop.Length == 0)
        {
            Debug.LogWarning("enemyDrop CSV is empty or not loaded.");
            return;
        }

        if (enemyDropPrefabs == null || enemyDropPrefabs.Count == 0)
        {
            Debug.LogWarning(ID + ": no enemyDropPrefabs assigned in Inspector.");
            return;
        }

        // dropRate in EnemyLootDropTrial is a fraction 0.0-1.0 (e.g. Heart=0.2, Star=1.0)
        // Roll a random float and check each item independently (items are not mutually exclusive)
        bool anyDropped = false;

        for (int i = 0; i < GameManager.enemyDrop.Length; i++)
        {
            string[] cols = GameManager.enemyDrop[i].Split(',');
            if (cols.Length < 4) continue;

            string dropID = cols[0].Trim();

            float dropRate;
            if (!float.TryParse(cols[3].Trim(), out dropRate)) continue;

            float roll = Random.value; // 0.0 to 1.0
            Debug.Log(ID + " drop roll for " + dropID + ": " + roll + " vs rate " + dropRate);

            if (roll <= dropRate)
            {
                // Find the matching prefab
                for (int j = 0; j < enemyDropPrefabs.Count; j++)
                {
                    if (enemyDropPrefabs[j] == null) continue;

                    EnemyDrop dropComp = enemyDropPrefabs[j].GetComponent<EnemyDrop>();
                    if (dropComp != null && dropComp.toID.Trim() == dropID)
                    {
                        Instantiate(enemyDropPrefabs[j], transform.position, Quaternion.identity);
                        Debug.Log(ID + " dropped: " + dropID);
                        anyDropped = true;
                        break;
                    }
                }
            }
        }

        if (!anyDropped)
            Debug.Log(ID + ": no loot dropped this time.");
    }
}

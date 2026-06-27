using System.Collections;
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

        // The CircleCollider2D is the detection trigger — set its radius
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null)
            circle.radius = detectionRange;
    }

    protected virtual void Update()
    {
        if (player == null || !isAlive) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        // Enter attack range
        if (distance <= attackRange && idleState && !attackState)
        {
            idleState = false;
            chasedState = false;
            attackState = true;
        }

        // Lost player — back to idle
        if (distance > detectionRange && !idleState)
        {
            idleState = true;
            chasedState = false;
            attackState = false;
        }

        // Death check
        if (HP <= 0 && isAlive)
        {
            isAlive = false;
            isDead = true;
            StopAllCoroutines();
            enemyDead();
            return;
        }

        if (chasedState && !idleState && !attackState)
            playerChase();

        if (attackState && !chasedState && Atk == null)
            enemyAttack();
    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.enemyType.Length; i++)
        {
            string[] columns = GameManager.enemyType[i].Split(',');
            if (columns.Length < 9) continue;

            if (columns[0].Trim() == enemyTypeName)
            {
                ID = columns[0].Trim();
                variantID = columns[1].Trim();
                HP = float.Parse(columns[2].Trim());
                damage = float.Parse(columns[3].Trim());
                speed = float.Parse(columns[4].Trim());
                attackRange = float.Parse(columns[5].Trim());
                detectionRange = float.Parse(columns[7].Trim());
                lootTableID = columns[8].Trim();

                assigned = true;
                Debug.Log(ID + " enemy stats assigned. HP=" + HP + " dmg=" + damage + " spd=" + speed);
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
        if (GameManager.enemyDrop == null || enemyDropPrefabs == null || enemyDropPrefabs.Count == 0)
            return;

        // dropRate in CSV is a fraction (0.0–1.0), so roll a 0–1 float
        float roll = Random.value;

        for (int i = 0; i < GameManager.enemyDrop.Length; i++)
        {
            string[] columns = GameManager.enemyDrop[i].Split(',');
            if (columns.Length < 4) continue;

            float dropRate;
            if (!float.TryParse(columns[3].Trim(), out dropRate)) continue;

            if (roll <= dropRate)
            {
                string dropID = columns[0].Trim();
                Debug.Log("Dropped: " + dropID + " (roll=" + roll + " rate=" + dropRate + ")");

                for (int j = 0; j < enemyDropPrefabs.Count; j++)
                {
                    EnemyDrop dropComp = enemyDropPrefabs[j].GetComponent<EnemyDrop>();
                    if (dropComp != null && dropComp.toID == dropID)
                    {
                        Instantiate(enemyDropPrefabs[j], transform.position, Quaternion.identity);
                        break;
                    }
                }
                return;
            }
        }

        Debug.Log("No loot dropped (roll=" + roll + ")");
    }
}

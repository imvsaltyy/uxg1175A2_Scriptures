using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

     public float attackRange;
    //public float attackCooldown;

    [HideInInspector]
    public float detectionRange;
    #endregion

    [HideInInspector]
    public Collider2D attackCollider;

    #region Enemy
    public bool idleState = true;
    public bool chasedState = false;
    public bool attackState = false;
    public bool isAlive = true;
    public bool isDead = false;

    public float distance;
    [HideInInspector] public GameObject player;

    public Coroutine DoT;
    public Coroutine Atk;

    #endregion

    private void Awake()
    {
        //LoadExcelData();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        statsAssignment();
        player = GameObject.FindWithTag("Player");

        gameObject.GetComponent<CircleCollider2D>().radius = detectionRange;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (isAlive && DoT == null)
        {
            //Damage Over Time to emulate death
            //DoT = StartCoroutine(damageOT());
        }

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

        if (HP <= 0)
        {
            isAlive = !isAlive;
            isDead = !isDead;

            //Debug.Log("Enemy Dead");
            //Destroy(gameObject);
            //enemyDead();

            StopAllCoroutines();
        }

        if (chasedState && !idleState && !attackState)
        {
            //Debug.Log("Enter Chased State");
            playerChase();
        }

        if (attackState && !chasedState && Atk == null)
        {
            //Debug.Log("Enter Attack State");
            enemyAttack();
        }

    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.enemyType.Length; i++)
        {
            string[] columns = GameManager.enemyType[i].Split(',');

            if (columns[0] == enemyTypeName)
            {
                ID = columns[0];
                variantID = columns[1];

                HP = float.Parse(columns[2]);
                damage = float.Parse(columns[3]);
                speed = float.Parse(columns[4]);
                attackRange = float.Parse(columns[5]);  
 
                //newEnemy.attackCooldown = float.Parse(columns[6]);
                
                detectionRange = float.Parse(columns[7]);

                lootTableID = columns[8];

                assigned = true;
                Debug.Log(ID + " Enemy stats assigned");

                //Debug.Log("HP: " + HP);
                //Debug.Log("Damage: " + damage);
                //Debug.Log("Speed: " + speed);
            }

        }

        //Failsafe if enemy type is not found
        if (!assigned)
        {
            Debug.Log("Enemy type not found!");
            Destroy(gameObject);
        }
    }

    //LoadExcelData()

    //Getting Enemy Stats from Data File (Unused)
    //void LoadExcelData()
    //{
    //    bool assigned = false;

    //    //Load CSV file from the Resources folder
    //    TextAsset enemyCSV = Resources.Load<TextAsset>("EnemyStatsTrial");

    //    if (enemyCSV == null)
    //    {
    //        //Debug.LogError("CSV file not found.");
    //        return;
    //    }

    //    string[] rows = enemyCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

    //    for (int i = 1; i < rows.Length; i++)
    //    {
    //        //Skip empty rows
    //        if (string.IsNullOrWhiteSpace(rows[i])) continue;

    //        //Split columns by comma delimiter
    //        string[] columns = rows[i].Split(',');

    //        if (columns[0] != enemyTypeName) continue;

    //        //Assign vales to variables in the order that is inside the CSV File
    //        else
    //        {
    //            ID = columns[0];

    //            this.HP = float.Parse(columns[1]);
    //            damage = float.Parse(columns[2]);
    //            speed = float.Parse(columns[3]);

    //            //newEnemy.attackRange = float.Parse(columns[4]);
    //            //newEnemy.attackCooldown = float.Parse(columns[5]);
    //            //newEnemy.detectionRange = float.Parse(columns[6]);

    //            lootTableID = columns[7];

    //            assigned = !assigned;
    //        }

    //    }

    //    //Failsafe if enemy type is not found
    //    if (!assigned)
    //    {
    //        Debug.Log("Enemy type not found!");
    //        Destroy(gameObject);
    //    }
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !chasedState && idleState)
        {
            //Begin player chase here
            idleState = false;
            chasedState = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !chasedState && idleState)
        {
            //Begin player chase here
            idleState = false;
            chasedState = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && chasedState && !idleState)
        {
            idleState = true;
            chasedState = false;
        }
    }

    public void playerChase()
    {

        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        if (distance <= attackRange && !attackState)
        {
            chasedState = false;
            attackState = true;
        }

    }

    public IEnumerator damageOT()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(1f);
            HP -= 10f;

            Debug.Log(HP);
        }

    }

    public virtual void enemyAttack()
    {
        //Debug.Log("Basic Enemy Attack");
        
    }
}

using System.Collections;
using UnityEngine;

public class enemyWalker : spawnEnemy
{

    private void Awake()
    {
        enemyTypeName = "enemy_walker";

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        attackCollider = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

    }

    IEnumerator attack()
    {
        while (attackState)
        {
            yield return new WaitForSeconds(0.2f);
            //Trigger Start Attack Animation here

            //Insert Attack Code here
            attackCollider.enabled = true;

            //Trigger End Ataack Animation here
            yield return new WaitForSeconds(1f);

            //Debug.Log("Enemy Walker End Attack");

            idleState = true;
            attackState = false;
            attackCollider.enabled = false;

            Atk = null;

        }

    }

    public override void enemyAttack()
    {
        //base.enemyAttack();
        //Debug.Log("EnemyWalker Attacks");

        Atk = StartCoroutine(attack());

        
    }
}

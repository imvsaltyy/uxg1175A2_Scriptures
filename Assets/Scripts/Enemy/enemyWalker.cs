using UnityEngine;

public class enemyWalker : spawnEnemy
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
    }

    public override void enemyAttack()
    {
        //base.enemyAttack();
        Debug.Log("EnemyWalker Attacks");
        attackCollider.enabled = true;
    }
}

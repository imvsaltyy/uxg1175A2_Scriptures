using UnityEngine;

public class enemyShooter : spawnEnemy
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
        Debug.Log("EnemyShooter Attacks");
    }
}

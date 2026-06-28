using System.Collections;
using UnityEngine;

public class enemyWalker : spawnEnemy
{
    private void Awake()
    {
        enemyTypeName = "enemy_walker";
    }

    protected override void Start()
    {
        base.Start();

        // Attack collider is on first child
        if (transform.childCount > 0)
            attackCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void enemyAttack()
    {
        if (Atk == null)
            Atk = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);

        if (attackCollider != null)
            attackCollider.enabled = true;

        yield return new WaitForSeconds(1f);

        if (attackCollider != null)
            attackCollider.enabled = false;

        idleState = true;
        attackState = false;
        Atk = null;
    }
}

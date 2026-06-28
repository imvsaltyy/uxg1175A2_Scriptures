using System.Collections;
using UnityEngine;

public class enemyWalker : spawnEnemy
{
<<<<<<< Updated upstream
=======
    AudioManager audioManager;

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
>>>>>>> Stashed changes
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void enemyAttack()
    {
        if (Atk == null)
            Atk = StartCoroutine(Attack());
<<<<<<< Updated upstream
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);

=======
        audioManager.PlaySFX(audioManager.duckshoot);

    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);

>>>>>>> Stashed changes
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

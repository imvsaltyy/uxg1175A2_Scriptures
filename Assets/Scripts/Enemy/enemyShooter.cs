using UnityEngine;

public class enemyShooter : spawnEnemy
{
    private float nextTimeToFire = 0f;

    private void Awake()
    {
        enemyTypeName = "enemy_shooter";
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        // Enter chase state when in detection range but outside attack range
        if (attackRange < distance && distance < detectionRange && !chasedState)
        {
            idleState = false;
            chasedState = true;
            attackState = false;
        }

        // Face the player whenever not idle.
        // Atan2 gives the angle for the RIGHT axis (+X), but bullets fire along UP (+Y),
        // so we subtract 90 degrees to align the facing with the fire direction.
        if (!idleState && player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -270f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public override void enemyAttack()
    {
        nerfGun nerf = GetComponentInChildren<nerfGun>();
        lazerGun lazer = GetComponentInChildren<lazerGun>();

        if (nerf != null && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + nerf.fireRate;
            nerf.SetOwnerTag(gameObject.tag);
            nerf.Fire(damage);
        }
        else if (lazer != null && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + lazer.fireRate;
            lazer.SetOwnerTag(gameObject.tag);
            lazer.Fire(damage);
        }
    }
}

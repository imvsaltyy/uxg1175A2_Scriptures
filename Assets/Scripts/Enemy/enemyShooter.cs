using System.Collections;
using UnityEngine;

public class enemyShooter : spawnEnemy
{
    //private GameObject shootingPoint;
    //public GameObject bullet;

    private float nextTimeToFire = 0f;

    private void Awake()
    {
        enemyTypeName = "enemy_shooter";

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();


        if (attackRange < distance && distance < detectionRange && !chasedState)
        {
            idleState = false;
            chasedState = true;
            attackState = false;
        }


        if (!idleState)
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

    }

    public override void enemyAttack()
    {
        //base.enemyAttack();

        //Debug.Log("shooterEnemyAttack");

        if (Time.time >= nextTimeToFire && gameObject.GetComponentInChildren<nerfGun>() != null)
        {
            nextTimeToFire = Time.time + gameObject.GetComponentInChildren<nerfGun>().fireRate;
            gameObject.GetComponentInChildren<nerfGun>().Fire(damage);
        }

        else if (Time.time >= nextTimeToFire && gameObject.GetComponentInChildren<lazerGun>() != null)
        {
            nextTimeToFire = Time.time + gameObject.GetComponentInChildren<lazerGun>().fireRate;
            gameObject.GetComponentInChildren<lazerGun>().Fire(damage);
        }


    }


}

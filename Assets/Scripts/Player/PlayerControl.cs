using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("Movement")]
    private float speed;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction reloadAction;

    [Header("Rotation")]
    private Transform playerSprite;
    private float rotationSpeed;

    [Header("FOV")]
    private Transform visionLight;
    private float viewDistance;
    private float viewAngle;

    public bool canShootEnemy;

    private Camera mainCam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.statsAssignment();

        speed = playerStats.baseMoveSpeed;
        viewDistance = playerStats.vision;
        viewAngle = playerStats.fovAngle;
        rotationSpeed = playerStats.rotationSpeed;

        Debug.Log("Speed from CSV: " + speed);


        mainCam = Camera.main;

        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        reloadAction = InputSystem.actions.FindAction("Reload");
        //key down
        attackAction.started += (x) =>
        {
            Debug.Log("started");
        };
        //called once
        attackAction.performed += (x) =>
        {
            Debug.Log("perform");
        };
        //key up
        attackAction.canceled += (x) =>
        {
            Debug.Log("canceled");
        };

        //reloadAction.started += (x) =>
        //{
        //    Debug.Log("reload");
        //};
    }
    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        RotateToMouse();
        CheckEnemiesInFOV();
    }

    private void MovePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        transform.position += (Vector3)(moveInput * speed * Time.deltaTime);
    }

    private void RotateToMouse()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = transform.position.z;

        Vector2 lookDir = mouseWorldPos - transform.position;

        if (lookDir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

            playerSprite.rotation = Quaternion.RotateTowards(
                playerSprite.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            visionLight.rotation = Quaternion.RotateTowards(
                visionLight.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void CheckEnemiesInFOV()
    {
        canShootEnemy = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector2 playerPos = transform.position;
            Vector2 enemyPos = enemy.transform.position;

            Vector2 directionToEnemy = enemyPos - playerPos;
            float distanceToEnemy = directionToEnemy.magnitude;

            if (distanceToEnemy > viewDistance)
                continue;

            directionToEnemy.Normalize();

            Vector2 playerForward = playerSprite.up;

            float dot = Vector2.Dot(playerForward, directionToEnemy);

            float angleLimit = Mathf.Cos((viewAngle / 2f) * Mathf.Deg2Rad);

            if (dot >= angleLimit)
            {
                canShootEnemy = true;
                Debug.Log("Enemy in FOV. Can shoot.");
                return;
            }
        }
    }
}
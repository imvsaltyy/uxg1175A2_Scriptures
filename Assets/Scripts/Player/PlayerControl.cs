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
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Transform weaponHolder;
    private float rotationSpeed;

    [Header("FOV")]
    [SerializeField] private Transform visionLight;
    private float viewDistance;
    private float viewAngle;

    [Header("Weapon")]
    private iWeapon currentWeapon;
    private float nextTimeToFire;

    public bool canShootEnemy;

    private Camera mainCam;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        // PlayerStats already does statsAssignment in its own Start,
        // but this keeps your current setup safe.
        playerStats.statsAssignment();

        speed = playerStats.FinalMoveSpeed;
        viewDistance = playerStats.vision;
        viewAngle = playerStats.fovAngle;
        rotationSpeed = playerStats.rotationSpeed;

        Debug.Log("Speed from CSV: " + speed);

        mainCam = Camera.main;

        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        reloadAction = InputSystem.actions.FindAction("Reload");

        if (moveAction != null) moveAction.Enable();
        if (attackAction != null) attackAction.Enable();
        if (reloadAction != null) reloadAction.Enable();

        SetupSelectedWeapon();

        attackAction.started += OnAttackStarted;
        //attackAction.performed += OnAttackPerformed;
        //attackAction.canceled += OnAttackCanceled;
    }

    private void OnDestroy()
    {
        if (attackAction != null)
        {
            attackAction.started -= OnAttackStarted;
            //attackAction.performed -= OnAttackPerformed;
            //attackAction.canceled -= OnAttackCanceled;
        }
    }

    private void Update()
    {
        //Check if its paused
        if (PauseController.isPaused)
            return;

        MovePlayer();
        RotateToMouse();
        CheckEnemiesInFOV();
    }

    private void SetupSelectedWeapon()
    {
        if (weaponHolder == null)
        {
            Debug.LogError("Weapon holder is not assigned in PlayerControl.");
            return;
        }

        string selectedWeaponID = "nerf_gun";

        if (PlayerManager.Instance != null)
        {
            selectedWeaponID = PlayerManager.Instance.selectedWeaponID;
        }

        Debug.Log("Trying to equip weapon: " + selectedWeaponID);

        iWeapon[] weapons = weaponHolder.GetComponentsInChildren<iWeapon>(true);

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            bool isSelectedWeapon = false;

            if (selectedWeaponID == "nerf_gun" && weapons[i] is nerfGun)
            {
                isSelectedWeapon = true;
            }
            else if (selectedWeaponID == "lazer_gun" && weapons[i] is lazerGun)
            {
                isSelectedWeapon = true;
            }

            if (isSelectedWeapon)
            {
                weapons[i].gameObject.SetActive(true);
                currentWeapon = weapons[i];

                currentWeapon.SetOwnerTag(gameObject.tag);

                Debug.Log("Equipped weapon: " + selectedWeaponID);
                return;
            }
        }

        Debug.LogWarning("Selected weapon not found under weapon holder: " + selectedWeaponID);
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Attack started");
        Attack();
    }

    //private void OnAttackPerformed(InputAction.CallbackContext context)
    //{
    //    Attack();
    //}

    //private void OnAttackCanceled(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Attack canceled");
    //}

    private void Attack()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        if (Time.time < nextTimeToFire)
        {
            return;
        }

        // If you only want player to shoot when enemy is inside FOV, uncomment this.
        //if (!canShootEnemy)
        //{
        //    Debug.Log("No enemy in FOV. Cannot shoot.");
        //    return;
        //}

        nextTimeToFire = Time.time + currentWeapon.fireRate;

        currentWeapon.Fire(playerStats.FinalDamage);

        Debug.Log("Player attacked with: " + currentWeapon.ID);
    }

    private void MovePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        transform.position += (Vector3)(moveInput * speed * Time.unscaledDeltaTime);
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
                rotationSpeed * Time.unscaledDeltaTime
            );

            visionLight.rotation = Quaternion.RotateTowards(
                visionLight.rotation,
                targetRotation,
                rotationSpeed * Time.unscaledDeltaTime
            );

            if (weaponHolder != null && weaponHolder.parent != playerSprite)
            {
                weaponHolder.rotation = Quaternion.RotateTowards(
                    weaponHolder.rotation,
                    targetRotation,
<<<<<<< Updated upstream
                    rotationSpeed * Time.deltaTime
=======
                    rotationSpeed * Time.unscaledDeltaTime
>>>>>>> Stashed changes
                );
            }
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
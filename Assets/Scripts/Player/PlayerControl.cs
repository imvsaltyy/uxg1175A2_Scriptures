using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction attackAction;
    [SerializeField] private InputAction reloadAction;

    [Header("Rotation")]
    [SerializeField] private Transform playerSprite;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("FOV")]
    [SerializeField] private Transform fovTriangle;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private float viewAngle = 60f;
    //to test
    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
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

        reloadAction.started += (x) =>
        {
            Debug.Log("reload");
        };
    }
    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        RotateSprite();
        CheckFOV();
    }

    private void MovePlayer()
    {
        Vector2 dir = moveAction.ReadValue<Vector2>();

        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

            playerSprite.rotation = Quaternion.RotateTowards(
                playerSprite.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (fovTriangle != null)
            {
                fovTriangle.rotation = playerSprite.rotation;
            }
        }
    }

    private void RotateSprite()
    {
        //empty for now
        //if want mouse rotation
    }

    private void CheckFOV()
    {
        if (target == null)
            return;

        Vector2 playerPos = transform.position;
        Vector2 targetPos = target.position;

        Vector2 directionToTarget = targetPos - playerPos;

        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > viewDistance)
        {
            //Debug.Log("Target too far");
            return;
        }

        directionToTarget.Normalize();

        Vector2 playerForward = playerSprite.up;

        float dot = Vector2.Dot(playerForward, directionToTarget);

        float angleLimit = Mathf.Cos((viewAngle / 2f) * Mathf.Deg2Rad);

        if (dot >= angleLimit)
        {
            //Debug.Log("Target is inside FOV");
        }
        else
        {
            //Debug.Log("Target is outside FOV");
        }
    }
}
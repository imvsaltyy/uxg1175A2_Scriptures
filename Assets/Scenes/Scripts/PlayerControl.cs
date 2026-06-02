using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction attackAction;
    [SerializeField] private InputAction reloadAction;

    private Camera mainCam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        reloadAction.started += (x) =>
        {
            Debug.Log("reload");
        };
    }
    [SerializeField] private float rotationSpeed = 360f;
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = moveAction.ReadValue<Vector2>();
        transform.position = transform.position + (dir * speed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}


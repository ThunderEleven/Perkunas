using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    private new Rigidbody rigidbody;

    public bool canLook = true;
    private bool isPaused = false;

    public Action inventory;

    public int attackDamage = 5;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UIManager.Instance.GetUI<UICrafting>().OpenUI();
            ToggleCursor();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isPaused)
            {
                UIManager.Instance.OpenUI<UIPause>();
                isPaused = true;
                ToggleCursor();
            }
            else
            {
                UIManager.Instance.CloseUI<UIPause>();
                isPaused = false;
                ToggleCursor();
            }
        }
    }
    
    public void changeIsPaused()
    {
        UIManager.Instance.CloseUI<UIPause>();
        isPaused = false;
        ToggleCursor();
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 2.5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    public void AttackWithRaycastHit(RaycastHit hit)
    {
        Monster enemy = hit.collider.GetComponent<Monster>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
            Debug.Log($"Hit {hit.collider.name} for {attackDamage} damage!");
        }
    }

    // [SerializeField]GameObject buildingPrefab; // �߰�

    // public void OnBuild(InputAction.CallbackContext context) // �߰�
    // {
    //     if (context.phase == InputActionPhase.Started)
    //     {
    //         CharacterManager.Instance.Player.buildingManager.PlaceObject(transform.position + Vector3.forward, buildingPrefab);
    //     }

    //      CharacterManager.Instance.Player.buildingManager.EndVisualisingObject(); ��𼱰� ��������
    // }
}

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
    public bool isPaused = false;
    public bool isCrafting = false;
    public bool isInInventory = false;
    public bool isNpc = false;
    public bool isGameOver = false;

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

    private bool isMainUIState()
    {
        if (!isPaused && !isCrafting && !isInInventory && !isNpc && !isGameOver)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void OnCrafting(InputAction.CallbackContext context)
    {
        if (isMainUIState())
        {
            UIManager.Instance.GetUI<UICrafting>().OpenUI();
            isCrafting = true;
            ToggleCursor();
        }
        else
        {
            UIManager.Instance.CloseUI<UICrafting>();
            isCrafting = false;
            ToggleCursor();
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isMainUIState())
            {
                UIManager.Instance.OpenUI<UIPause>();
                isPaused = true;
                ToggleCursor();
            }
            else
            {
                var upUI = UIManager.Instance.uiStack.Peek();
                if (upUI.GetComponent<UIMain>() || upUI.GetComponent<UIDamageIndicator>() || upUI.GetComponent<UIQuickSlot>())
                {
                    return;
                }
                else
                {
                    UIManager.Instance.uiStack.Peek().CloseUI();
                    isPaused = false;
                    ToggleCursor();
                }

                // UIManager.Instance.CloseUI<UIPause>();
                // isPaused = false;
                // ToggleCursor();
            }

            // if (isCrafting)
            // {
            //     UIManager.Instance.CloseUI<UICrafting>();
            //     ToggleCursor();
            //     isCrafting = false;
            // }
            //
            // if (isInInventory)
            // {
            //     inventory?.Invoke();
            //     ToggleCursor();
            //     isInInventory = false;
            // }
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
            isInInventory = !isInInventory;
            ToggleCursor();
        }
    }
    public void ToggleCursor()
    {
        if (!isMainUIState())
        {
            Cursor.lockState = CursorLockMode.None;
            canLook = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            canLook = true;
        }
        
        // bool toggle = Cursor.lockState == CursorLockMode.Locked;
        // Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // canLook = !toggle;
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

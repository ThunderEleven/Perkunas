using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curinteractable;

    public TextMeshProUGUI prompText;
    private Camera camera;

    // Update is called once per frame
    private void Start()
    {
        camera = Camera.main;
    }
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                curInteractGameObject = hit.collider.gameObject;
                curinteractable = hit.collider.GetComponent<IInteractable>();
                SetPormpText();
                Debug.Log($"감지된 오브젝트: {hit.collider.name}");
            }
            else
            {
                curInteractGameObject = null;
                curinteractable = null;
                prompText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPormpText()
    {
        prompText.gameObject.SetActive(true);
        prompText.text = curinteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curinteractable != null)
        {
            curinteractable.OnInteract();
            curinteractable = null;
            curInteractGameObject = null;
            prompText.gameObject.SetActive(false);
        }
    }
}

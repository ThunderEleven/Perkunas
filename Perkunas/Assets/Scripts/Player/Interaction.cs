using TMPro;
using UnityEngine;
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
        prompText = UIManager.Instance.GetUI<UIMain>().promptText;
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

            }
            else if (Physics.Raycast(ray, out hit, maxCheckDistance))
            {

                if (CharacterManager.Instance.Player.equip.builtObject != null)
                {
                    ItemData data = CharacterManager.Instance.Player.equip.BuildItemData;


                    if (data != null && data.placable)
                    {
                        CharacterManager.Instance.Player.buildingManager.PlaceObject(hit.point, data);

                    }
                    else
                    {
                        CharacterManager.Instance.Player.buildingManager.EndVisualisingObject();
                    }
                }
                else
                {
                    CharacterManager.Instance.Player.buildingManager.EndVisualisingObject();
                }
            }
            else
            {
                curInteractGameObject = null;
                curinteractable = null;
                prompText.gameObject.SetActive(false);
                CharacterManager.Instance.Player.buildingManager.EndVisualisingObject();
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
            //prompText.gameObject.SetActive(false);
        }
    }
}

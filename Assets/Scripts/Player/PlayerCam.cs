using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public PlayerInput pInput;
    private InputAction look, interact;
    [SerializeField] private Transform player;
    [SerializeField] Texture2D cursorTexture;
    private GameObject currLookedAtObj;
    private Highlight hl;
    private float verticalRotation, horizontalRotation;
    public float lookSpeed, interactRange;
    public bool isCursorLocked;
    public bool isInteracting;

    // Start is called before the first frame update
    private void Awake()
    {
        isInteracting = false;
        isCursorLocked = true;
        pInput = new PlayerInput();
        verticalRotation = 0;
        horizontalRotation = 0;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        look = pInput.Player.Look;
        interact = pInput.Player.Interact;
        interact.performed += InteractWithObject;
        look.Enable();
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.performed -= InteractWithObject;
        look.Disable();
        interact.Disable();
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            transform.position = new Vector3(player.position.x, player.position.y + 0.62f, player.position.z);
            Vector2 readLookValue = look.ReadValue<Vector2>();
            horizontalRotation += lookSpeed * readLookValue.x;
            verticalRotation -= lookSpeed * readLookValue.y;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            transform.localEulerAngles = new Vector3(verticalRotation, horizontalRotation, 0);

            // See if player is looking at an interactable object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                if (hit.collider.gameObject.CompareTag("Interactable") && !isInteracting)
                {
                    if (hit.collider.gameObject != currLookedAtObj)
                    {
                        if (hl != null)
                            hl.ToggleHighlight(false);
                        currLookedAtObj = hit.collider.gameObject;
                        hl = currLookedAtObj.GetComponent<Highlight>();
                        hl.ToggleHighlight(true);
                        Cursor.visible = true;
                    }
                }
                else
                {
                    Cursor.visible = false;
                    if (hl != null && !isInteracting)
                    {
                        hl.ToggleHighlight(false);
                        hl = null;
                        currLookedAtObj = null;
                    }
                }
            }
            else
            {
                Cursor.visible = false;
                if (hl != null && !isInteracting)
                {
                    hl.ToggleHighlight(false);
                    hl = null;
                    currLookedAtObj = null;
                }
            }
        }
    }

    private void InteractWithObject(InputAction.CallbackContext ctx)
    {
        if (currLookedAtObj != null)
        {
            Interactable interactedObj = currLookedAtObj.GetComponent<Interactable>();
            isInteracting = interactedObj.Interact();
        }
    }
}

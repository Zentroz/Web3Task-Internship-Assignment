using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction tapAction;
    private InputAction touchPositionAction;

    private Ball attachedBall;

    void Awake()
    {
        touchPositionAction = inputActions.FindAction("TouchPosition");
        tapAction = inputActions.FindAction("Tap");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = touchPositionAction.ReadValue<Vector2>();
        Vector2 cursorToWorldPos = Camera.main.ScreenToWorldPoint(cursorPos);

        if (tapAction.IsPressed())
        {
            TryAttachBall(cursorToWorldPos);
        }

        bool canPullBall = attachedBall != null && attachedBall.AttachedLauncher != null;
        if (canPullBall)
        {
            attachedBall.AttachedLauncher.Pull(cursorToWorldPos);
        }

        bool canReleaseBall = attachedBall != null && tapAction.WasReleasedThisFrame();
        if (canReleaseBall)
        {
            attachedBall.AttachedLauncher.Release();
            attachedBall = null;
        }
    }

    private void TryAttachBall(Vector3 cursorWorldPosition)
    {
        if (attachedBall != null) return;

        RaycastHit2D hit = Physics2D.Raycast(cursorWorldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Ball ball))
        {
            if (ball.AttachedLauncher != null) attachedBall = ball;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [Header("Trajectory Parameters")]
    [SerializeField] private int pointsCount = 20;
    [SerializeField] private float timeStep = 0.125f;
    private LineRenderer lineRenderer;

    private Vector3 gravity;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = (Vector3)Physics2D.gravity;
    }

    public void Draw(Vector3 startPoint, Vector3 startVelocity)
    {
        lineRenderer.positionCount = pointsCount;
        float elapsedTime = 0f;

        for (int i = 0; i < pointsCount; i++)
        {
            // USING KINEMATIC EQATION : x(t) = x0 + v0t + (1/2)at^2
            Vector3 currentPos = startPoint + startVelocity * elapsedTime + 0.5f * gravity * elapsedTime * elapsedTime;
            lineRenderer.SetPosition(i, currentPos);

            elapsedTime += timeStep;
        }
    }
    public void Show()
    {
        lineRenderer.enabled = true;
    }
    public void Hide()
    {
        lineRenderer.enabled = false;
    }
}
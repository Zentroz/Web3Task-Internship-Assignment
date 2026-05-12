using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ballAttachPoint;
    [SerializeField] private TrajectoryRenderer trajectoryRenderer;
    [SerializeField] private GameObject ballPrefab;

    [Header("Launch Parameters")]
    [SerializeField] private float maxForce = 25f;
    [SerializeField] private float minPullDistance = 1f;
    [SerializeField] private float maxPullDistance = 4f;

    private Ball currentBall;
    private Vector3 ballPullOffset;
    private Vector3 pullOffsetVelocity;

    void Awake()
    {
        AddBall();
    }

    void Update()
    {
        if (currentBall != null)
        {
            float ballPullSpeed = 10f;
            currentBall.transform.position = Vector3.SmoothDamp(currentBall.transform.position, ballAttachPoint.transform.position + ballPullOffset, ref pullOffsetVelocity, ballPullSpeed * Time.deltaTime);
        }
    }

    private Vector3 CalculateLaunchVelocity()
    {
        float launchForce = GameUtility.CalculateBallLaunchForce(ballAttachPoint.transform.position, currentBall.transform.position, minPullDistance, maxPullDistance, maxForce);
        Vector3 launchDir = (ballAttachPoint.transform.position - currentBall.transform.position).normalized;
        return launchDir * launchForce;
    }

    public void AddBall()
    {
        if (currentBall != null) return;

        currentBall = Instantiate(ballPrefab, ballAttachPoint.position, Quaternion.identity).GetComponent<Ball>();
        currentBall.SetLauncher(this);
    }

    public void Pull(Vector3 cursorWorldPos)
    {
        Vector3 pullVector = cursorWorldPos - ballAttachPoint.transform.position;
        pullVector = Vector3.ClampMagnitude(pullVector, maxPullDistance);

        ballPullOffset = pullVector;

        Vector3 launchVelocity = CalculateLaunchVelocity();

        if (launchVelocity.sqrMagnitude > 0f)
        {
            trajectoryRenderer.Show();
            trajectoryRenderer.Draw(currentBall.transform.position, launchVelocity);
        }
        else
        {
            trajectoryRenderer.Hide();
        }
    }
    public void Release()
    {
        trajectoryRenderer.Hide();

        Vector3 launchVelocity = CalculateLaunchVelocity();

        ballPullOffset = Vector3.zero;

        if (launchVelocity.sqrMagnitude == 0f) return;

        currentBall.SetLauncher(null);
        currentBall.Launch(launchVelocity);

        currentBall = null;
    }
}
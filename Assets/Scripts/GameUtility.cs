using UnityEngine;

public static class GameUtility
{
    public static float CalculateBallLaunchForce(
        Vector3 attachPoint, Vector3 ballPosition,
        float minPullDistance, float maxPullDistance,
        float maxForce
    )
    {
        Vector2 direction = (attachPoint - ballPosition).normalized;
        float pullDistance = Vector3.Distance(attachPoint, ballPosition);
        pullDistance = Mathf.Clamp(pullDistance, minPullDistance, maxPullDistance);

        if (pullDistance < minPullDistance) return 0f;

        float remappedPull = Mathf.InverseLerp(minPullDistance, maxPullDistance, pullDistance);

        return remappedPull * maxForce;
    }

    public static bool IsFailedImpact(Vector3 impactVelocity, Vector3 surfaceNormal, float angleThreshold)
    {
        float angleBetweenVectors = Vector3.Angle(impactVelocity, surfaceNormal);

        if (angleBetweenVectors < angleThreshold) return true;

        return false;
    }
}
using UnityEngine;

public class VictoryBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Ball>(out _)) return;

        GameManager.Instance?.SuccessfullShotEvent?.Invoke();

        float destroyDelay = 1f;
        Destroy(collision.gameObject, destroyDelay);
    }
}
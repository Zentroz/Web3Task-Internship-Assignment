using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Surface Interaction Parameters")]
    [SerializeField] private float failedImpactAngleThreshold = 45f;
    [SerializeField] private float slidingAngleThreshold = 45f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem ballShatterParticle;

    private Vector2 velocity;
    private Rigidbody2D rb;
    public Launcher AttachedLauncher { get; private set; }
    private TrailRenderer trailRenderer;

    public void SetLauncher(Launcher launcher) => AttachedLauncher = launcher;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        velocity = rb.linearVelocity;

        if (transform.position.y < GameManager.BottomBorder)
        {
            GameManager.Instance?.FailedShotEvent?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Surface")) return;
        
        ContactPoint2D contact = collision.GetContact(0);

        float hitAngleFromNormal = Mathf.Abs(Vector3.Angle(-velocity.normalized, contact.normal));

        // Ball doesn't aligns to the tangent directory of the impacted surface.
        if (hitAngleFromNormal < failedImpactAngleThreshold)
        {
            rb.AddForce(contact.normal * velocity.magnitude / 2, ForceMode2D.Impulse);

            StartCoroutine(ShatterCo(0.25f));
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Surface")) return;
        
        ContactPoint2D contact = collision.GetContact(0);
        
        Vector3 surfaceTangent = Vector3.Cross(contact.normal, Vector3.forward);
        Vector3 slidingVector = velocity.normalized;
        
        float slidingAngleFromTangent = Vector3.Angle(slidingVector, surfaceTangent);
        slidingAngleFromTangent = Mathf.Min(slidingAngleFromTangent, 180f - slidingAngleFromTangent);

        // Ball aligns to the tangent directory of the impacted surface.
        if (slidingAngleFromTangent <= slidingAngleThreshold)
        {
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, contact.normal);
            rb.linearVelocity = projectedVelocity;

            trailRenderer.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Surface")) return;

        trailRenderer.enabled = false;
        trailRenderer.Clear();
    }

    public void Launch(Vector3 force)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.gravityScale = 1f;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private IEnumerator ShatterCo(float delay)
    {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(delay);

        ballShatterParticle.transform.parent = null;
        ballShatterParticle.Play();

        GameManager.Instance?.FailedShotEvent?.Invoke();

        Destroy(gameObject);
    }
}
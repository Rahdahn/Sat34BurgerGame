using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float xLimit = 8f;
    private Rigidbody2D rb;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!canMove) return;

        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    public void DisableControl()
    {
        canMove = false;

        rb.velocity = Vector2.zero;
    }
}
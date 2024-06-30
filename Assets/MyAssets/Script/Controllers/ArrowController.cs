using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;

    [SerializeField] private bool isCountered;
    [SerializeField] private int arrowDirection;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowDirection = 1;
        canMove = true;

        if (xVelocity < 0)
            FlipArrowDirection();

    }

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void SetupArrow(float _speed, int _damage)
    {
        xVelocity = _speed;
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ʵ������˺�
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            collision.GetComponent<CharacterStats>().TakeDamage(damage, arrowDirection, 4);
            StuckIntoTarget(collision);
        }
        // ֹͣ�ƶ�
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckIntoTarget(collision);
        }
    }

    private void StuckIntoTarget(Collider2D collision)
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void CounterArrow()
    {
        if (isCountered)
            return;
        isCountered = true;
        FlipArrowDirection();
        xVelocity = -xVelocity;
        targetLayerName = "Enemy";
    }

    public void FlipArrowDirection()
    {
        arrowDirection = -arrowDirection;
        transform.Rotate(0, 180, 0);
    }

}
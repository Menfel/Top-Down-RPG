using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private KnockBack knockback;

    private void Awake()
    {
        knockback = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
      moveDir = targetPosition;
        
        //Vector2 direction = targetPosition - rb.position;

        //// Если враг уже почти дошёл — не двигай его, чтобы он не дёргался
        //if (direction.magnitude > 0.1f)
        //{
        //    moveDir = direction.normalized;
        //}
        //else
        //{
        //    moveDir = Vector2.zero;
        //} 
    }
}

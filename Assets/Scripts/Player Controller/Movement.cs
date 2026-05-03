using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;


    float moveX;
    float moveY;


    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //MoveTransform();
    }

    private void FixedUpdate()
    {
        MoveRigidBody();
    }

    public void SetMoveDirection(float xDirection, float yDirection)
    {
        moveX = xDirection;
        moveY = yDirection;
    }

    private void MoveRigidBody()
    {
        Vector2 movement = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        rb.linearVelocity = movement;

    }

    private void MoveTransform()
    {
        this.transform.position += new Vector3(moveX, moveY, 0) * moveSpeed * Time.fixedDeltaTime;
    }



}

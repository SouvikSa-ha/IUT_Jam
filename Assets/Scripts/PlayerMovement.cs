using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 0;
  private Rigidbody2D rb;
  private Vector2 moveDirection;
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    MoveInput();
  }
  void FixedUpdate()
  {
    Move();
  }

  private void MoveInput()
  {
    var horizontal = Input.GetAxisRaw("Horizontal");
    var vertical = Input.GetAxisRaw("Vertical");
    moveDirection = new Vector2(horizontal, vertical).normalized;
  }
  
  private void Move(){
    rb.velocity = moveSpeed * moveDirection;
  }
}

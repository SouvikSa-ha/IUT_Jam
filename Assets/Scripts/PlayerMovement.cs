using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 0;
  private float currentMoveSpeed = 0;
  private Rigidbody2D rb;
  private Vector2 moveDirection;
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    currentMoveSpeed = moveSpeed;
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
    rb.velocity = currentMoveSpeed * moveDirection;
  }

  [HideInInspector]
  public void SlowSpeed(float speed){
    currentMoveSpeed = speed;
  }
  
  [HideInInspector]
  public void RegainSpeed(){
    currentMoveSpeed = moveSpeed;
  }
}

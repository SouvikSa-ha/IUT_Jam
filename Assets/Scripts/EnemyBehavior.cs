using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
  #region old
  private Vector2 spawnPoint;
  private readonly float chaseDistance = 6f;
  private readonly float chaseSpeed = 6f;
  private readonly float roamSpeed = 4f;
  private Vector2 roamEdge = new Vector2(3f, 0f);
  private bool movingLeft = true;
  private bool chasing = false;

  private float attackMaxTime = 1.5f;
  private float attackTimer = 1.5f;
  private bool hit = false;
  #endregion old


  [SerializeField] private EnemyData enemyData;
  private Rigidbody2D rb;
  private Vector2 targetDirection;
  private Transform player;
  private FieldOfView fov;
  [SerializeField] private Transform rendererGO;
  private float changeDirectionCoolDown = 2f;
  private bool resting = false;
  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    fov = GetComponent<FieldOfView>();
    spawnPoint = transform.position;
    targetDirection = Vector2.up;
  }
  void Start()
  {
    player = PlayerStat.Instance.transform;
  }

  void Update()
  {
    UpdateTargetDirection();
    FlipSprite();
  }

  private void FixedUpdate()
  {
    RotateToDirection();
    SetVelocity();
  }



  private void UpdateTargetDirection()
  {
    HandleRandomDirection();
    HandlePlayerTargetting();
  }

  private void HandleRandomDirection()
  {
    if(resting || fov.canSeePlayer) return;
    changeDirectionCoolDown -= Time.deltaTime;
    if(changeDirectionCoolDown <= 0){
      resting = true;
      StartCoroutine(WaitBeforeChangeDirection());
    }
  }

  private IEnumerator WaitBeforeChangeDirection(){
    yield return new WaitForSeconds(Random.Range(0f, 3f));
    float angleChange = Random.Range(-90f, 90f);
    Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
    targetDirection = rotation * targetDirection;
    changeDirectionCoolDown = Random.Range(1f, 2f);
    resting = false;
  }

  private void HandlePlayerTargetting()
  {
    if (fov.canSeePlayer) targetDirection = (player.position - transform.position).normalized;
  }

  private void FlipSprite(){
    if(transform.rotation.z > 0 && transform.rotation.z < 180) rendererGO.localScale = new Vector3(-1, 1, 1);
    else rendererGO.localScale = Vector3.one;
  }

  private void RotateToDirection()
  {
    Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, enemyData.RotationSpeed * Time.deltaTime);
  
    rb.SetRotation(rotation);
    rendererGO.rotation = Quaternion.Euler(0f, 0f, -rotation.z);
  }

  private void SetVelocity()
  {
    if ((Vector2.Distance(transform.position, player.position) <= enemyData.AttackRange || resting) && !fov.canSeePlayer)
      rb.velocity = Vector2.zero;
    else 
      rb.velocity = targetDirection * enemyData.MoveSpeed;
  }

  private void OldSystem()
  {
    if (hit)
    {
      if (attackTimer > attackMaxTime)
      {
        PlayerStat.Instance.TakeDamage(20);
        attackTimer = 0f;
      }
      attackTimer += Time.deltaTime;
      return;
    }
    else if (attackTimer < attackMaxTime)
    {
      attackTimer += Time.deltaTime;
    }
    if (Vector2.Distance(player.position, transform.position) < chaseDistance)
    {
      transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
      movingLeft = (transform.position.x - player.position.x) > 0;
      chasing = true;
    }
    else if (Vector2.Distance(transform.position, spawnPoint) > 0.1f && chasing)
    {
      transform.position = Vector2.MoveTowards(transform.position, spawnPoint, roamSpeed * Time.deltaTime);
      movingLeft = (transform.position.x - spawnPoint.x) > 0;
    }
    else
    {
      chasing = false;
      if (Vector2.Distance(transform.position, spawnPoint + roamEdge) > 0.1f && !movingLeft)
      {
        transform.position = Vector2.MoveTowards(transform.position, spawnPoint + roamEdge, roamSpeed * Time.deltaTime);
      }
      else if (!movingLeft)
      {
        movingLeft = true;
      }
      if (Vector2.Distance(transform.position, spawnPoint - roamEdge) > 0.1f && movingLeft)
      {
        transform.position = Vector2.MoveTowards(transform.position, spawnPoint - roamEdge, roamSpeed * Time.deltaTime);
      }
      else if (movingLeft)
      {
        movingLeft = false;
      }
    }
    if (movingLeft) transform.localScale = new Vector2(1, 1);
    else transform.localScale = new Vector2(-1, 1);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player")) hit = true;

  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Player")) hit = false;
  }
}

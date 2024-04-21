using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
  [SerializeField] private EnemyData enemyData;
  private Rigidbody2D rb;
  private Vector2 targetDirection;
  private Transform player;
  private FieldOfView fov;
  [SerializeField] private Transform rendererGO;
  private float changeDirectionCoolDown = 2f;
  private bool resting = false;
  [SerializeField] private Transform area;
  private bool ignorePlayer = false;
  private float attackTimer;
  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    fov = GetComponent<FieldOfView>();
  }
  void Start()
  {
    player = PlayerStat.Instance.transform;
    targetDirection = Vector2.up;
  }

  void Update()
  {
    UpdateTargetDirection();
    FlipSprite();
    AttackPlayer();
  }

  private void FixedUpdate()
  {
    RotateToDirection();
    SetVelocity();
  }

  private void AttackPlayer(){
    if(Vector2.Distance(transform.position, player.position) < enemyData.AttackRange && fov.canSeePlayer){
      attackTimer += Time.deltaTime;
      if(attackTimer >= enemyData.AttackSpeed){
        PlayerStat.Instance.TakeDamage(enemyData.Damage);
        attackTimer = 0f;
      }
    } 
    else attackTimer = enemyData.AttackSpeed;
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
    if (fov.canSeePlayer && !ignorePlayer) targetDirection = (player.position - transform.position).normalized;
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
    if ((Vector2.Distance(transform.position, player.position) <= enemyData.AttackRange && fov.canSeePlayer) || resting && !fov.canSeePlayer)
      rb.velocity = Vector2.zero;
    else 
      rb.velocity = targetDirection * enemyData.MoveSpeed;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("Zone")) {
      ignorePlayer = false;
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if(other.CompareTag("Zone")) {
      targetDirection = (area.position - transform.position).normalized;
      ignorePlayer = true;
    }
  }
}

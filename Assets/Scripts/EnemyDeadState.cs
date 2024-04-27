using UnityEngine;

public class EnemyDeadState : EnemyBasicState
{
  private float destroyCountDown = 10f;
  public override void EnterState(EnemyBehavior enemy)
  {
    enemy.rb.velocity = Vector2.zero;
    enemy.directionIndicator.SetActive(false);
    enemy.gameObject.tag = "SpellIngredient";
    enemy.GetComponent<Collider2D>().isTrigger = true;
    destroyCountDown = Random.Range(8, 16);
  }

  public override void UpdateState(EnemyBehavior enemy)
  {
    if(destroyCountDown > 0){
      destroyCountDown -= Time.deltaTime;
    }
    else {
      Object.Destroy(enemy.gameObject);
    }
  }

  public override void FixedUpdate(EnemyBehavior enemy)
  {
    
  }
}

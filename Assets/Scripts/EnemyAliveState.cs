using System.Collections;
using UnityEngine;

public class EnemyAliveState : EnemyBasicState
{
  public override void EnterState(EnemyBehavior enemy)
  {

  }
  public override void UpdateState(EnemyBehavior enemy)
  {
    enemy.UpdateTargetDirection();
    enemy.FlipSprite();
    enemy.AttackPlayer();
  }

  public override void FixedUpdate(EnemyBehavior enemy)
  {
    enemy.RotateToDirection();
    enemy.SetVelocity();
  }
}

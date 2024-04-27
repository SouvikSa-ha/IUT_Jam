using UnityEngine;

public abstract class EnemyBasicState
{
  public abstract void EnterState(EnemyBehavior enemy);
  public abstract void UpdateState(EnemyBehavior enemy);
  public abstract void FixedUpdate(EnemyBehavior enemy);
}

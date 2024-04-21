using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
  [SerializeField] private EnemyData enemyData;

  [HideInInspector] public float radius;
  [HideInInspector] public float angle;
  [HideInInspector] public GameObject playerRef;
  private LayerMask targetMask;
  private LayerMask obstructionMask;
  [HideInInspector] public bool canSeePlayer;

  void Awake(){
    targetMask = LayerMask.GetMask("Player");
    obstructionMask = LayerMask.GetMask("Obstruction");
  }
  private void Start()
  {
    playerRef = PlayerStat.Instance.gameObject;
    radius = enemyData.PlayerAwarenessRadius;
    angle = enemyData.FovAngle;
    StartCoroutine(FOVRoutine());
  }

  private IEnumerator FOVRoutine()
  {
    WaitForSeconds wait = new(0.2f);

    while (true)
    {
      yield return wait;
      FieldOfViewCheck();
    }
  }

  private void FieldOfViewCheck()
  {
    Collider2D rangeCheck = Physics2D.OverlapCircle(transform.position, radius, targetMask);

    if (rangeCheck != null)
    {
      //Debug.Log(rangeCheck.gameObject.name + " " + transform.position + " TargetMask " + targetMask);
      Transform target = rangeCheck.transform;
      Vector2 directionToTarget = (target.position - transform.position).normalized;

      if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
      {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
          canSeePlayer = true;
        else
          canSeePlayer = false;
      }
      else
        canSeePlayer = false;
    }
    else if (canSeePlayer)
      canSeePlayer = false;
  }
}

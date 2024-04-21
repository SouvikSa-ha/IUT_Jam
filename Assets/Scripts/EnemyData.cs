using UnityEngine;

[CreateAssetMenu(fileName = "Enemy 0", menuName = "ScriptableObjects/Enemy", order = 0)]
public class EnemyData : ScriptableObject {
  [field: SerializeField] public float MoveSpeed { get; private set; }
  [field: SerializeField] public int Damage { get; private set; }
  [field: SerializeField] public int AttackSpeed { get; private set; }
  [field: SerializeField] public float AttackRange { get; private set; }
  [field: SerializeField] public float PlayerAwarenessRadius { get; private set; }
  [field: SerializeField, Range(0, 360)] public float FovAngle { get; private set; }
  [field: SerializeField, Range(0, 360)] public float RotationSpeed { get; private set; }

}
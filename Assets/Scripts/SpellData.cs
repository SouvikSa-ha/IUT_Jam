using System.Collections.Generic;
using UnityEngine;

public enum SpellIngredient {
  EnemyA,
  EnemyB,
  EnemyC,
  EnemyD
}

public enum SpellEffect{
  VitalityImmunity,
  HungerImmunity,
  ToxicImmunity
}

[System.Serializable]
public class SpellIngredientWrapper {
  [field: SerializeField] public SpellIngredient Ingredient { get; private set; }
  [field: Range(1, 5), SerializeField] public int Amount { get; private set; }
}


[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 2)]
public class SpellData : ScriptableObject {
  [field: SerializeField] public List<SpellIngredientWrapper> Ingredients { get; private set; }
  [field: SerializeField] public int PrepTime { get; private set; }
  [field: SerializeField] public SpellEffect SpellEffect { get; private set; }
}

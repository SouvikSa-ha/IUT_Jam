using System.Collections.Generic;
using UnityEngine;

public enum Ingredient{
  IngredientA,
  IngredientB,
  IngredientC,
  IngredientD,
}

[System.Serializable]
public class IngredientWrapper {
  [field: SerializeField] public Ingredient Ingredient { get; private set; }
  [field: Range(1, 5), SerializeField] public int Amount { get; private set; }
}

[CreateAssetMenu(fileName = "Food 0", menuName = "ScriptableObjects/Food", order = 1)]
public class FoodData : ScriptableObject {
  [field: SerializeField] public List<IngredientWrapper> Ingredients { get; private set; }
  [field: SerializeField] public int PrepTime { get; private set; }
  [field: SerializeField] public int HungerRegen { get; private set; }
  [field: SerializeField] public int HealthRegen { get; private set; }
}
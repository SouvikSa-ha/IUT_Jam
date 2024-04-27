using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


#region Dictionary Wrapper
[Serializable]
public class InventoryFoodDictWrapperItem{
  [field: SerializeField] public Ingredient Ingredient { get; private set; }
  [field: SerializeField] public int Amount { get; private set; }
}

[Serializable]
public class InventoryFoodDictWrapper{
  [SerializeField] private InventoryFoodDictWrapperItem[] items;
  public Dictionary<Ingredient, int> ToDictionary(){
    Dictionary<Ingredient, int> newDict = new();
    foreach(var item in items){
      newDict.Add(item.Ingredient, item.Amount);
    }
    return newDict;
  }
}

[Serializable]
public class InventorySpellDictWrapperItem{
  [field: SerializeField] public SpellIngredient Ingredient { get; private set; }
  [field: SerializeField] public int Amount { get; private set; }
}

[Serializable]
public class InventorySpellDictWrapper{
  [SerializeField] private InventorySpellDictWrapperItem[] items;
  public Dictionary<SpellIngredient, int> ToDictionary(){
    Dictionary<SpellIngredient, int> newDict = new();
    foreach(var item in items){
      newDict.Add(item.Ingredient, item.Amount);
    }
    return newDict;
  }
}
#endregion Dictionary Wrapper

public class Inventory : MonoBehaviour
{
  #region Singleton
  public static Inventory Instance {get; private set;}
  private void Awake() {
    if(Instance == null) Instance = this;
  }
  #endregion Singleton
  
  [SerializeField] private InventoryFoodDictWrapper wrapper;
  [SerializeField] private InventorySpellDictWrapper spellWrapper;
  public Dictionary<Ingredient, int> items;
  public Dictionary<SpellIngredient, int> spellItems;
  [SerializeField] private TextMeshProUGUI[] ingredientTexts;
  [SerializeField] private TextMeshProUGUI[] spellIngredientTexts;
  private readonly int foodPoisoningValue = 13;
  private AudioSource source;
  [SerializeField] private AudioClip collect;
  private void Start() {
    items = wrapper.ToDictionary();
    spellItems = spellWrapper.ToDictionary();
    source = GetComponent<AudioSource>();
    UpdateInventorySpellItemsHUD(ingredient: SpellIngredient.EnemyA);
    UpdateInventorySpellItemsHUD(ingredient: SpellIngredient.EnemyB);
    UpdateInventorySpellItemsHUD(ingredient: SpellIngredient.EnemyC);
    UpdateInventorySpellItemsHUD(ingredient: SpellIngredient.EnemyD);
    UpdateInventoryFoodItemsHUD(Ingredient.IngredientA);
    UpdateInventoryFoodItemsHUD(Ingredient.IngredientB);
    UpdateInventoryFoodItemsHUD(Ingredient.IngredientC);
    UpdateInventoryFoodItemsHUD(Ingredient.IngredientD);
  }

  private void UpdateInventoryFoodItemsHUD(Ingredient ingredient){
    ingredientTexts[ingredient.GetHashCode()].text = items[ingredient].ToString() + "/10";
  }

  private void UpdateInventorySpellItemsHUD(SpellIngredient ingredient){
    spellIngredientTexts[ingredient.GetHashCode()].text = spellItems[ingredient].ToString() + "/5";
  }

  private bool AddFoodIngredient(Ingredient ingredient){
    if(items[ingredient] < 10){
      items[ingredient] ++; 
      UpdateInventoryFoodItemsHUD(ingredient);
      GameManager.Instance.UpdateButtonsInteractability();
      return true;
    }
    else return false;
  }
  public void UseFoodIngredient(Ingredient ingredient, int amount){
    items[ingredient] -= amount;
    UpdateInventoryFoodItemsHUD(ingredient);
    GameManager.Instance.UpdateButtonsInteractability();
    var chance = UnityEngine.Random.Range(0, 50);
    if(chance == foodPoisoningValue) SpellEffects.Instance.GetFoodPoisoning();
  }

  private bool AddSpellIngredient(SpellIngredient ingredient){
    if(spellItems[ingredient] < 5){
      spellItems[ingredient] ++; 
      UpdateInventorySpellItemsHUD(ingredient);
      GameManager.Instance.UpdateSpellButtonsInteractability();
      return true;
    }
    else return false;
  }
  public void UseSpellIngredient(SpellIngredient ingredient, int amount){
    spellItems[ingredient] -= amount;
    UpdateInventorySpellItemsHUD(ingredient);
    GameManager.Instance.UpdateSpellButtonsInteractability();
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("Ingredient")){
      var added = AddFoodIngredient(other.GetComponent<IngredientHolder>().ingredient);
      if(added) {
        Destroy(other.gameObject);
        source.PlayOneShot(collect);
      }
    }
    else if(other.CompareTag("SpellIngredient")){
      var added = AddSpellIngredient(other.GetComponent<EnemyBehavior>().spellIngredient);
      if(added) {
        Destroy(other.gameObject);
        source.PlayOneShot(collect);
      }
    }
  }
}

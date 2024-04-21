using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


#region Dictionary Wrapper
[Serializable]
public class InventoryDictWrapperItem{
  [field: SerializeField] public Ingredient Ingredient { get; private set; }
  [field: SerializeField] public int Amount { get; private set; }
}

[Serializable]
public class InventoryDictWrapper{
  [SerializeField] private InventoryDictWrapperItem[] items;
  public Dictionary<Ingredient, int> ToDictionary(){
    Dictionary<Ingredient, int> newDict = new();
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
  
  [SerializeField] private InventoryDictWrapper wrapper;
  public Dictionary<Ingredient, int> items;
  [SerializeField] private TextMeshProUGUI ingredientAText;
  [SerializeField] private TextMeshProUGUI ingredientBText;
  [SerializeField] private TextMeshProUGUI ingredientCText;
  [SerializeField] private TextMeshProUGUI ingredientDText;

  private void Start() {
    items = wrapper.ToDictionary();
  }

  private void UpdateInventoryItemsHUD(){
    ingredientAText.text = items[Ingredient.IngredientA].ToString();
    ingredientBText.text = items[Ingredient.IngredientB].ToString();
    ingredientCText.text = items[Ingredient.IngredientC].ToString();
    ingredientDText.text = items[Ingredient.IngredientD].ToString();
  }

  private void AddIngredient(Ingredient ingredient){
    items[ingredient] ++; 
    UpdateInventoryItemsHUD();
    GameManager.Instance.UpdateButtonsInteractability();
  }
  public void UseIngredient(Ingredient ingredient, int amount){
    items[ingredient] -= amount;
    UpdateInventoryItemsHUD();
    GameManager.Instance.UpdateButtonsInteractability();
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("Ingredient")){
      AddIngredient(other.GetComponent<IngredientHolder>().ingredient);
      Destroy(other.gameObject);
    }
  }
}

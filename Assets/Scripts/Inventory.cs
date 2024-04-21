using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  #region Singleton
  public static Inventory Instance;
  private void Awake() {
    if(Instance == null) Instance = this;
  }
  #endregion Singleton
  
  [SerializeField] private Dictionary<Ingredient, int> inventory = new();
  [SerializeField] private TextMeshProUGUI ingredientAText;
  [SerializeField] private TextMeshProUGUI ingredientBText;
  [SerializeField] private TextMeshProUGUI ingredientCText;
  [SerializeField] private TextMeshProUGUI ingredientDText;

  private void InventoryItemsHUD(){

  }

  public void AddIngredient(Ingredient ingredient){
    inventory[ingredient] ++; 
  }
  public void UseIngredient(Ingredient ingredient, int amount){
    inventory[ingredient] -= amount;
  }
}

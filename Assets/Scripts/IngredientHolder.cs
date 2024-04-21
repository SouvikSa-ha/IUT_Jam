using System.Collections;
using UnityEngine;

public class IngredientHolder : MonoBehaviour
{
  public Ingredient ingredient;
  private void Start (){
    StartCoroutine(Destroy());
  }

  private IEnumerator Destroy(){
    yield return new WaitForSeconds(10);
    Destroy(gameObject);
  }
}

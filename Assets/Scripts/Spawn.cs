using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
  private Vector2 areaDimensions;
  [SerializeField] private GameObject ingredient;
  [SerializeField] private GameObject Enemy;
  private void Start()
  {
    areaDimensions = GetComponent<BoxCollider2D>().size / 2;
    areaDimensions = new Vector2(areaDimensions.x - 0.5f, areaDimensions.y - 0.5f);
    StartCoroutine(SpawnEnemy());
    StartCoroutine(SpawnIngredient());
  }

  private IEnumerator SpawnEnemy()
  {
    for(int i=0; i<5; i++){
      Vector2 pos = (Vector2)transform.position + new Vector2(Random.Range(-areaDimensions.x, areaDimensions.x), Random.Range(-areaDimensions.y, areaDimensions.y));
      Instantiate(Enemy, pos, Quaternion.identity);
    }
    WaitForSeconds wait = new(7);
    while(true){
      Vector2 pos = (Vector2)transform.position + new Vector2(Random.Range(-areaDimensions.x, areaDimensions.x), Random.Range(-areaDimensions.y, areaDimensions.y));
      yield return wait;
      Instantiate(Enemy, pos, Quaternion.identity);
    }
  }

  private IEnumerator SpawnIngredient()
  {
    yield return new WaitForSeconds(Random.Range(7, 15));
    Vector2 pos = (Vector2)transform.position + new Vector2(Random.Range(-areaDimensions.x, areaDimensions.x), Random.Range(-areaDimensions.y, areaDimensions.y));
    Instantiate(ingredient, pos, Quaternion.identity);
    StartCoroutine(SpawnIngredient());
  }
}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  #region Singleton
  public static GameManager Instance { get; private set; }
  private void Awake()
  {
    if (Instance == null) Instance = this;
  }
  #endregion Singleton


  [SerializeField] private GameObject inventoryCV;
  [SerializeField] private GameObject cookingCV;
  [SerializeField] private GameObject pauseMenu;
  [SerializeField] private GameObject mainMenu;
  [SerializeField] private GameObject GameOverMenu;
  [SerializeField] private GameObject PlayerHUD;
  [SerializeField] private Button[] foodBtns;
  [SerializeField] private FoodData[] foods;

  [HideInInspector] public bool inCookingArea = false;

  private Inventory inventory;
  private PlayerStat playerStat;
  private void Start()
  {
    inventoryCV.SetActive(false);
    cookingCV.SetActive(false);
    mainMenu.SetActive(true);
    pauseMenu.SetActive(false);
    GameOverMenu.SetActive(false);
    PlayerHUD.SetActive(false);
    inventory = Inventory.Instance;
    playerStat = PlayerStat.Instance;
    foreach (var btn in foodBtns)
      btn.interactable = false;
    Time.timeScale = 0;
  }

  public void StartGame()
  {
    inventoryCV.SetActive(false);
    cookingCV.SetActive(false);
    mainMenu.SetActive(false);
    pauseMenu.SetActive(false);
    GameOverMenu.SetActive(false);
    PlayerHUD.SetActive(true);
    Time.timeScale = 1;
  }

  public void MainMenu()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.I))
    {
      inventoryCV.SetActive(!inventoryCV.activeSelf);
    }
    if (Input.GetKeyDown(KeyCode.C) && inCookingArea)
    {
      cookingCV.SetActive(!cookingCV.activeSelf);
    }
    if (Input.GetKeyDown(KeyCode.P))
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
  }

  [HideInInspector]
  public void DeactivateCookingCV()
  {
    cookingCV.SetActive(inCookingArea = false);
  }

  [HideInInspector]
  public void UpdateButtonsInteractability()
  {
    for (int i = 0; i < foodBtns.Length; i++)
    {
      var flag = 0;
      foreach (var ingredient in foods[i].Ingredients)
      {
        if (inventory.items[ingredient.Ingredient] < ingredient.Amount)
        {
          flag = 1;
          foodBtns[i].interactable = false;
          break;
        }
      }
      if (flag == 0) foodBtns[i].interactable = true;
    }
  }

  public void Cook(int i)
  {
    foreach (var ingredient in foods[i].Ingredients)
    {
      inventory.UseIngredient(ingredient.Ingredient, ingredient.Amount);
    }
    playerStat.currentHealth = Mathf.Clamp(playerStat.currentHealth += foods[i].HealthRegen, 0, playerStat.Health);
    playerStat.currentHunger = Mathf.Clamp(playerStat.currentHunger += foods[i].HungerRegen, 0, playerStat.Hunger);
    playerStat.currentVitality = Mathf.Clamp(playerStat.currentVitality += foods[i].VitalityRegen, 0, playerStat.Vitality);
    if (i == 2)
    {
      playerStat.vitalityOnBoost = true;
      playerStat.currentVitality = playerStat.Vitality;
      StartCoroutine(playerStat.EnableVitalityBoost());
    }
    playerStat.UpdateAllHUD();
  }

  [HideInInspector]
  public void EndGame()
  {
    Time.timeScale = 0;
    inventoryCV.SetActive(false);
    cookingCV.SetActive(false);
    pauseMenu.SetActive(false);
    mainMenu.SetActive(false);
    GameOverMenu.SetActive(true);
    PlayerHUD.SetActive(false);
  }
}

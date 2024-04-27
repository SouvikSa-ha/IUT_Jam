
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
  [SerializeField] private Button[] spellBtns;

  [SerializeField] private FoodData[] foods;
  [SerializeField] private SpellData[] spells;
  [SerializeField] private GameObject inventoryFoodPan;
  [SerializeField] private GameObject inventorySpellPan;
  [SerializeField] private GameObject cookingFoodPan;
  [SerializeField] private GameObject cookingSpellPan;
  [SerializeField] private Image inventoryFoodBtn;
  [SerializeField] private Image inventorySpellBtn;
  [SerializeField] private Image cookingFoodBtn;
  [SerializeField] private Image cookingSpellBtn;
  [SerializeField] private Color selectedColor;
  [SerializeField] private Color deselectedColor;
  [SerializeField] private TextMeshProUGUI timerTxt;
  private float timer = 0f;
  [SerializeField] private TextMeshProUGUI gameOverTimerTxt;
  [HideInInspector] public bool inCookingArea = false;
  private AudioSource source;
  [SerializeField] private AudioClip clickSound;
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
    foreach (var btn in spellBtns)
      btn.interactable = false;
    source = GetComponent<AudioSource>();
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

    inventoryFoodPan.SetActive(true);
    inventoryFoodBtn.color = selectedColor;
    cookingFoodPan.SetActive(true);
    cookingFoodBtn.color = selectedColor;
    inventorySpellPan.SetActive(false);
    inventorySpellBtn.color = deselectedColor;
    cookingSpellPan.SetActive(false);
    cookingSpellBtn.color = deselectedColor;
    Time.timeScale = 1;
    UpdateSpellButtonsInteractability();
    UpdateButtonsInteractability();
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
    timer += Time.deltaTime;
    float hours = Mathf.Floor(timer / 3600);
    float minutes = Mathf.FloorToInt(timer % 3600 /60); 
    float seconds = Mathf.FloorToInt(timer % 60);
    timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
  }

  [HideInInspector]
  public void DeactivateCookingCV()
  {
    if (cookingCV != null) cookingCV.SetActive(inCookingArea = false);
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

  [HideInInspector]
  public void UpdateSpellButtonsInteractability()
  {
    for (int i = 0; i < spellBtns.Length; i++)
    {
      var flag = 0;
      foreach (var ingredient in spells[i].Ingredients)
      {
        if (inventory.spellItems[ingredient.Ingredient] < ingredient.Amount)
        {
          flag = 1;
          spellBtns[i].interactable = false;
          break;
        }
      }
      if (flag == 0) spellBtns[i].interactable = true;
    }
  }

  public void Cook(int i)
  {
    source.PlayOneShot(clickSound);
    foreach (var ingredient in foods[i].Ingredients)
    {
      inventory.UseFoodIngredient(ingredient.Ingredient, ingredient.Amount);
    }
    playerStat.currentHealth = Mathf.Clamp(playerStat.currentHealth += foods[i].HealthRegen, 0, playerStat.Health);
    playerStat.currentHunger = Mathf.Clamp(playerStat.currentHunger += foods[i].HungerRegen, 0, playerStat.Hunger);
    playerStat.UpdateAllHUD();
  }

  public void PrepareSpell(int i)
  {
    source.PlayOneShot(clickSound);

    foreach (var ingredient in spells[i].Ingredients)
    {
      inventory.UseSpellIngredient(ingredient.Ingredient, ingredient.Amount);
    }
    if(spells[i].SpellEffect == SpellEffect.VitalityImmunity) StartCoroutine(SpellEffects.Instance.EnableVitalityBoost());
    else if(spells[i].SpellEffect == SpellEffect.HungerImmunity) SpellEffects.Instance.EnableHungerBoost();
    else if(spells[i].SpellEffect == SpellEffect.ToxicImmunity) SpellEffects.Instance.EnableToxicImmunity();
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
    gameOverTimerTxt.text = "You survived " + timerTxt.text;
  }

  public void SetInventoryFoodPan(){
    inventoryFoodPan.SetActive(true);
    inventoryFoodBtn.color = selectedColor;
    inventorySpellPan.SetActive(false);
    inventorySpellBtn.color = deselectedColor;
  }
  public void SetInventorySpellPan(){
    inventoryFoodPan.SetActive(false);
    inventoryFoodBtn.color = deselectedColor;
    inventorySpellPan.SetActive(true);
    inventorySpellBtn.color = selectedColor;
  }
  public void SetCookingFoodPan(){
    cookingFoodPan.SetActive(true);
    cookingFoodBtn.color = selectedColor;
    cookingSpellPan.SetActive(false);
    cookingSpellBtn.color = deselectedColor;
  }
  public void SetCookingSpellPan(){
    cookingFoodPan.SetActive(false);
    cookingFoodBtn.color = deselectedColor;
    cookingSpellPan.SetActive(true);
    cookingSpellBtn.color = selectedColor;
  }

  public void DisableHungerBackfire(){
    SpellEffects.Instance.DisableHungerBackfire();
  }
}

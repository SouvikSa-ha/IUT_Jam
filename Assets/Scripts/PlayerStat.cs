using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
  #region Singleton
  public static PlayerStat Instance { get; private set; }
  private void Awake()
  {
    if (Instance == null) Instance = this;
  }
  #endregion

  public readonly int Health = 100, Hunger = 100, Vitality = 100;
  public int currentHealth, currentHunger, currentVitality;
  public float hungerMeterReductionTime = 1f;
  public float vitalityReductionTime = 1f;
  public float vitalityRegenTime = 1.5f;
  private float vRedCurrentTime, vRegCurrentTime;
  public bool vitalityOnBoost;
  private float vitalityBoostTimer = 20f;
  private Color vitalityColor, vitalityBoostedColor;

  [SerializeField] private Slider healthSlider, hungerSlider, vitalitySlider;
  private Rigidbody2D rb;

  void Start()
  {
    currentHealth = Health;
    currentHunger = Hunger;
    currentVitality = Vitality;
    vRedCurrentTime = vitalityReductionTime;
    vRegCurrentTime = vitalityRegenTime;
    healthSlider.value = Health;
    hungerSlider.value = Hunger;
    vitalitySlider.value = Vitality;
    rb = GetComponent<Rigidbody2D>();
    StartCoroutine(UpdateHungerMeter());
    ColorUtility.TryParseHtmlString( "#44DDFF" , out vitalityColor );
    ColorUtility.TryParseHtmlString( "#4488FF" , out vitalityBoostedColor );
  }
  private void Update()
  {
    UpdateVitality();
  }

  private void UpdateVitality()
  {
    if(vitalityOnBoost) return;
    if (rb.velocity.magnitude > 0)
    {
      vRedCurrentTime -= Time.deltaTime;
      vRegCurrentTime = vitalityRegenTime;
      if (vRedCurrentTime <= 0)
      {
        currentVitality--;
        if(currentVitality < 0) Die();
        vitalitySlider.value = currentVitality;
        vRedCurrentTime = vitalityReductionTime;
      }
    }
    else
    {
      if (currentVitality < 100)
      {
        vRegCurrentTime -= Time.deltaTime;
        vRedCurrentTime = vitalityReductionTime;
        if (vRegCurrentTime <= 0)
        {
          currentVitality++;
          vitalitySlider.value = currentVitality;
          vRegCurrentTime = vitalityRegenTime;
        }
      }
    }
  }

  private void Die(){
    GameManager.Instance.EndGame();
  }

  [HideInInspector]
  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    if(currentHealth <= 0){
      Die();
    }
    healthSlider.value = currentHealth;
  }

  IEnumerator UpdateHungerMeter()
  {
    yield return new WaitForSeconds(hungerMeterReductionTime);
    currentHunger--;
    if(currentHunger <= 0) Die();
    hungerSlider.value = currentHunger;
    StartCoroutine(UpdateHungerMeter());
  }

  [HideInInspector]
  public void UpdateAllHUD(){
    healthSlider.value = currentHealth;
    hungerSlider.value = currentHunger;
    vitalitySlider.value = currentVitality;
  }
  [HideInInspector]
  public IEnumerator EnableVitalityBoost(){
    vitalitySlider.fillRect.GetComponent<Image>().color = vitalityBoostedColor;
    yield return new WaitForSeconds(vitalityBoostTimer);
    vitalitySlider.fillRect.GetComponent<Image>().color = vitalityColor;
    vitalityOnBoost = false;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("CookingArea")) GameManager.Instance.inCookingArea = true;
  }
  private void OnTriggerExit2D(Collider2D other) {
    if(other.CompareTag("CookingArea")) GameManager.Instance.DeactivateCookingCV();
  }
}

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

  public readonly int Health = 100, Hunger = 100, Vitality = 60;
  public int currentHealth, currentHunger, currentVitality;
  public float hungerMeterReductionTime = 1.5f;
  public float vitalityReductionTime = 0.7f;
  public float vitalityRegenTime = 0.8f;
  private float vRedCurrentTime, vRegCurrentTime;
  public bool vitalityOnBoost = false;
  public bool vitalityonBackfire = false;
  public bool hungerOnBoost = false;

  [SerializeField] private Slider healthSlider, hungerSlider, vitalitySlider;
  private Rigidbody2D rb;
  private SpriteRenderer sprenderer;
  void Start()
  {
    currentHealth = Health;
    currentHunger = Hunger;
    currentVitality = Vitality;
    vRedCurrentTime = vitalityReductionTime;
    vRegCurrentTime = vitalityRegenTime;
    UpdateAllHUD();
    rb = GetComponent<Rigidbody2D>();
    sprenderer = GetComponent<SpriteRenderer>();
    StartCoroutine(UpdateHungerMeter());
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
    else if(rb.velocity.magnitude == 0 && !vitalityonBackfire )
    {
      if (currentVitality < Vitality)
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

  [HideInInspector]
  public void Die(){
    GameManager.Instance.EndGame();
  }

  [HideInInspector]
  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    if(currentHealth <= 0){
      Die();
    }
    else{
      StartCoroutine(SpriteFlash());
    } 
    healthSlider.value = currentHealth;
  }

  private IEnumerator SpriteFlash(){
    float elapsedTime = 0;
    float elapsedPercentage;
    float numberOfFlashes = 2;
    while(elapsedTime < 1f){
      elapsedTime += Time.deltaTime;
      elapsedPercentage = elapsedTime / 1f;
      if(elapsedPercentage > 1){
        elapsedPercentage = 1;
      }
      float pingPongPercentage = Mathf.PingPong(elapsedPercentage * 2 * numberOfFlashes, 1);
      sprenderer.color = Color.Lerp(Color.white, Color.red, pingPongPercentage);
      yield return null;
    }
  }

  IEnumerator UpdateHungerMeter()
  {
    yield return new WaitForSeconds(hungerMeterReductionTime);
    if(!hungerOnBoost) {
      currentHunger--;
      if(currentHunger <= 0) Die();
      hungerSlider.value = currentHunger;
    }
    StartCoroutine(UpdateHungerMeter());
  }

  [HideInInspector]
  public void UpdateAllHUD(){
    healthSlider.value = currentHealth;
    hungerSlider.value = currentHunger;
    vitalitySlider.value = currentVitality;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("CookingArea")) GameManager.Instance.inCookingArea = true;
  }
  private void OnTriggerExit2D(Collider2D other) {
    if(other.CompareTag("CookingArea")) GameManager.Instance.DeactivateCookingCV();
  }
}

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

  private int Health = 100, Hunger = 100, Vitality = 100;
  [SerializeField] private int currentHealth, currentHunger, currentVitality;
  public float hungerMeterReductionTime = 1f;
  public float vitalityReductionTime = 1f;
  public float vitalityRegenTime = 1.5f;
  private float vRedCurrentTime, vRegCurrentTime;

  [SerializeField] private Slider healthSlider, hungerSlider, vitalitySlider;
  private Rigidbody2D rb;

  void Start()
  {
    currentHealth = Health;
    currentHunger = Hunger;
    currentVitality = Vitality;
    vRedCurrentTime = vitalityReductionTime;
    vRegCurrentTime = vitalityRegenTime;
    rb = GetComponent<Rigidbody2D>();
    StartCoroutine(UpdateHungerMeter());
  }
  private void Update()
  {
    UpdateVitality();
  }

  private void DebugStats()
  {
    Debug.Log(currentHealth);
    Debug.Log(currentHunger);
    Debug.Log(currentVitality);
  }

  private void UpdateVitality()
  {
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
    Debug.Log("Death by exhaustion");
  }

  [HideInInspector]
  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    healthSlider.value = currentHealth;
  }

  IEnumerator UpdateHungerMeter()
  {
    yield return new WaitForSeconds(hungerMeterReductionTime);
    currentHunger--;
    hungerSlider.value = currentHunger;
    StartCoroutine(UpdateHungerMeter());
  }
}

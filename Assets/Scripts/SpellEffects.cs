using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellEffects : MonoBehaviour
{
  #region Singleton
  public static SpellEffects Instance { get; private set; }
  private void Awake()
  {
    if (Instance == null) Instance = this;
  }
  #endregion Singleton

  //[SerializeField] private Transform statusHolder;
  //[SerializeField] private GameObject tmproTemplate;
  //[TextArea(2, 10), SerializeField] private List<string> statusTexts;
  [SerializeField] private Slider healthSlider, hungerSlider, vitalitySlider;

  #region Vitality Immunity
  private readonly float vitalityBoostTimer = 20f;
  [SerializeField] private Color vitalityColor, vitalityBoostedColor, vitalityBackfireColor;
  private readonly float vitalityBackfireTimer = 10f;

  [HideInInspector]
  public IEnumerator EnableVitalityBoost()
  {
    vitalitySlider.fillRect.GetComponent<Image>().color = vitalityBoostedColor;
    PlayerStat.Instance.vitalityOnBoost = true;
    PlayerStat.Instance.currentVitality = PlayerStat.Instance.Vitality;
    vitalitySlider.value = PlayerStat.Instance.Vitality;
    //StartCoroutine(UpdateStatusText(0, vitalityBoostTimer)); 

    yield return new WaitForSeconds(vitalityBoostTimer);
    PlayerStat.Instance.vitalityOnBoost = false;
    StartCoroutine(EnableVitalityBackFire(PlayerStat.Instance.vitalityReductionTime));
  }

  private IEnumerator EnableVitalityBackFire(float prevRedTime)
  {
    vitalitySlider.fillRect.GetComponent<Image>().color = vitalityBackfireColor;
    PlayerStat.Instance.vitalityonBackfire = true;
    PlayerStat.Instance.vitalityReductionTime = .5f;
    
    //StartCoroutine(UpdateStatusText(1, vitalityBackfireTimer));

    yield return new WaitForSeconds(vitalityBackfireTimer);
    PlayerStat.Instance.vitalityReductionTime = prevRedTime;
    PlayerStat.Instance.vitalityonBackfire = false;
    vitalitySlider.fillRect.GetComponent<Image>().color = vitalityColor;
  }

  #endregion Vitality Immunity

  #region Hunger Immunity
  private readonly float hungerBoostTimer = 30f;
  [SerializeField] private Color hungerColor, hungerBoostedColor, hungerBackFireColor;
  private bool isHungerBackfireOn;

  [HideInInspector]
  public void EnableHungerBoost()
  {
    hungerSlider.fillRect.GetComponent<Image>().color = hungerBoostedColor;
    PlayerStat.Instance.hungerOnBoost = true;
    PlayerStat.Instance.currentHunger = PlayerStat.Instance.Hunger;
    hungerSlider.value = PlayerStat.Instance.Hunger;
    StartCoroutine(EnableHungerBackFire());

    //StartCoroutine(UpdateStatusText(2, hungerBoostTimer));
  }
  
  private IEnumerator EnableHungerBackFire()
  {
    yield return new WaitForSeconds(hungerBoostTimer);
    PlayerStat.Instance.hungerOnBoost = false;
    hungerSlider.fillRect.GetComponent<Image>().color = hungerBackFireColor;
    isHungerBackfireOn = true;
    //UpdateStatusText(3);

    PlayerStat.Instance.hungerMeterReductionTime = .5f;
  }

  public void DisableHungerBackfire(){
    if(isHungerBackfireOn){
      hungerSlider.fillRect.GetComponent<Image>().color = hungerColor;
      PlayerStat.Instance.hungerMeterReductionTime = 1.5f; //HardCoded original value
      isHungerBackfireOn = false;
    }
  }

  #endregion Hunger Immunity

  #region Toxic Immunity
  [SerializeField] private Color healthColor, foodPoisoningColor;
  private readonly float toxicBackfireTimer = 8f;
  private readonly float healthReductionTimer = .8f;
  private bool onFoodPoisoning = false;
  [HideInInspector]
  public void GetFoodPoisoning(){
    //UpdateStatusText(4);
    healthSlider.fillRect.GetComponent<Image>().color = foodPoisoningColor;
    onFoodPoisoning = true;
    StartCoroutine(ReduceHealth());
  }
  public IEnumerator ReduceHealth(){
    yield return new WaitForSeconds(healthReductionTimer);
    PlayerStat.Instance.currentHealth--;
    healthSlider.value = PlayerStat.Instance.currentHealth;
    if(PlayerStat.Instance.currentHealth <= 0) PlayerStat.Instance.Die();
    if(onFoodPoisoning) StartCoroutine(ReduceHealth());
  }

  [HideInInspector]
  public void EnableToxicImmunity()
  {
    /*
    TODO: remove Status text 
    */
    onFoodPoisoning = false;
    healthSlider.fillRect.GetComponent<Image>().color = healthColor;
    StartCoroutine(EnableToxicBackFire());
  }

  private IEnumerator EnableToxicBackFire()
  {
    /*
    TODO: Add Status text 
    */
    var player = GetComponent<PlayerMovement>();
    player.SlowSpeed(3f);
    yield return new WaitForSeconds(toxicBackfireTimer);
    player.RegainSpeed();
    /*
    TODO: Remove Status text 
    */
  }

  #endregion Toxic Immunity
/*
  private IEnumerator UpdateStatusText(int i, float time){
    var obj = Instantiate(tmproTemplate, statusHolder.transform.position, Quaternion.identity);
    obj.transform.SetParent(statusHolder.transform);
    var txt = obj.GetComponent<TextMeshProUGUI>(); 
    while(time > 0){
      time -= Time.deltaTime;
      txt.text = statusTexts[i] + " " + time.ToString("00");
      yield return null;
    }
    Destroy(obj);
  }

  private void UpdateStatusText(int i){
    var obj = Instantiate(tmproTemplate, statusHolder.transform.position, Quaternion.identity);
    obj.transform.SetParent(statusHolder.transform);
    var txt = obj.GetComponent<TextMeshProUGUI>(); 
    txt.text = statusTexts[i];
    Destroy(obj, 4f);
  }
*/
}

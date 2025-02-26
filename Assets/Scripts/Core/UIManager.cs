using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Health System")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [Header("Shield System")]
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private TMP_Text shieldText;

    [Header("Ammo System")]
    [SerializeField] private Slider leftGunSlider;
    [SerializeField] private TMP_Text leftGunText;
    [SerializeField] private Slider rightGunSlider;
    [SerializeField] private TMP_Text rightGunText;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    #region Health Methods
    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = $"{currentHealth:0}/{maxHealth:0}";
    }
    #endregion

    #region Shield Methods
    public void SetShield(float currentShield, float maxShield)
    {
        shieldSlider.maxValue = maxShield;
        shieldSlider.value = currentShield;
        shieldText.text = $"{currentShield:0}/{maxShield:0}";
    }
    #endregion

    #region Ammo Methods
    public void SetLeftAmmo(int currentAmmo, int maxAmmo)
    {
        leftGunSlider.maxValue = maxAmmo;
        leftGunSlider.value = currentAmmo;
        leftGunText.text = $"{currentAmmo:0}";
    }

    public void SetRightAmmo(int currentAmmo, int maxAmmo)
    {
        rightGunSlider.maxValue = maxAmmo;
        rightGunSlider.value = currentAmmo;
        rightGunText.text = $"{currentAmmo:0}";
    }
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

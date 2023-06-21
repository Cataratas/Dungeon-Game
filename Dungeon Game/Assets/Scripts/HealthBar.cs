using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour {
    public Image bar;

    public void setHealth(float maxHealth, float health) {
        bar.fillAmount = Mathf.InverseLerp(0f, maxHealth, health);
    }
}

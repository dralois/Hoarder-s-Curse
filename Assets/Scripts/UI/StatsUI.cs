using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    [Header("Objects")]
    [SerializeField]
    private RectTransform healthSize;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Image buffIcon;

    public void SetBuffEnabled(bool enabled)
    {
        buffIcon.gameObject.SetActive(enabled);
    }

    public void SetHealthRemaining(float amount)
    {
        healthSize.localScale = new Vector3(amount, 1, 1);
        healthText.text = Mathf.Round(amount * 100) + " %";
    }
}
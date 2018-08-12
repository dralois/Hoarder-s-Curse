using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarUI : MonoBehaviour {

    [Header("Objects")]
    [SerializeField]
    private RectTransform healthSize;
    [SerializeField]
    private Text healthText;

    public void SetHealthRemaining(float amount)
    {
        healthSize.localScale = new Vector3(amount, 1, 1);
        healthText.text = Mathf.Round(amount * 100) + " %";
    }
}
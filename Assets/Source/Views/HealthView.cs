using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class HealthView : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Image _health;
    //[SerializeField] private TMP_Text _healthChangeText;

    private void Start()
    {
        _unit.Health.Changed += OnHealthChanged;
        OnHealthChanged(_unit.Health);
    }

    private void OnHealthChanged(Parametr health)
    {
        DOTween.To(x => _health.fillAmount = x, _health.fillAmount, health.Current / health.Initial, 0.5f);

        if (health.Current == 0)
            Invoke(nameof(Deactivate), 0.5f);
        //if (health.Delta > 0)
        //{
        //    _healthChangeText.color = Color.green;
        //    _healthChangeText.text = $"+{health.Delta:F1}";
        //    _healthChangeText.Show();
        //}
        //else if (health.Delta < 0)
        //{
        //    _healthChangeText.color = Color.red;
        //    _healthChangeText.text = $"{health.Delta:F1}";
        //    _healthChangeText.Show();
        //}
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}


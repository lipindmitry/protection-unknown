using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _shadow;
    [SerializeField] private Button _skillActivation;

    private SkillSlot _skillSlot;

    private bool _monitoring;

    public event System.Action<SkillSlot> SkillRequestedActivated;

    private void Start()
    {
        _skillActivation.onClick.AddListener(() => SkillRequestedActivated?.Invoke(_skillSlot));
    }

    private void Update()
    {
        if (!_monitoring)
            return;
        UpdateShadow();
    }


    public void SetSkillSlot(SkillSlot skillSlot)
    {
        _skillSlot = skillSlot;
        if (_skillSlot.ActiveSkill != null)
        {
            _icon.sprite = skillSlot.ActiveSkill.Icon;
            _skillSlot.ActiveSkill.Activated += OnSkillActivated;
            UpdateShadow();
        }
    }


    private void UpdateShadow()
    {
        _shadow.fillAmount = _skillSlot.RemainingCooldown;
        if (_skillSlot.Cooldown == 0)
        {
            _monitoring = false;
            _shadow.gameObject.SetActive(false);
        }
    }

    private void OnSkillActivated(Skill skill)
    {
        _shadow.gameObject.SetActive(true);
        _monitoring = true;
    }
}


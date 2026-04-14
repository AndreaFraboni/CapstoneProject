using System;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _currenthp;
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _fullHPOnStart = true;

    public Action<int, int> OnHealthChanged;
    public event Action OnDefeated;

    // Getter
    public int GetHp() => _currenthp;
    public int GetMaxHp() => _maxHP;

    private void Start()
    {
        if (_fullHPOnStart) SetHp(_maxHP);
    }

    public void SetHp(int hp)
    {
        hp = Mathf.Clamp(hp, 0, _maxHP);

        if (hp != _currenthp)
        {
            _currenthp = hp;

            OnHealthChanged?.Invoke(_currenthp, _maxHP);

            if (_currenthp <= 0)
            {
                OnDefeated?.Invoke();
            }
        }
    }

    public void AddHp(int amount)
    {
        if (amount < 0)
        {
            if (_currenthp > 0) AudioManager.Instance.PlaySFX("GetDamage");
        }
        else
        {
            AudioManager.Instance.PlaySFX("PickupHeart");
        }

        SetHp(_currenthp + amount);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TAKE DAMAGE ..");

        AddHp(-damage);
    }
}

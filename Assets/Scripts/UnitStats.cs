using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] int maxHealth;
    [SyncVar] int _curHealth;

    public Stat damage;
    public Stat armor;
    public Stat moveSpeed;

    public int curHealth { get { return _curHealth; } }

    public override void OnStartServer()
    {
        _curHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        if (damage > 0)
        {
            _curHealth -= damage;
            if (_curHealth <= 0)
            {
                _curHealth = 0;
            }
        }
    }

    public void SetHealthRate(float rate)
    {
        _curHealth = rate == 0 ? 0 : (int)(maxHealth / rate);
    }
}

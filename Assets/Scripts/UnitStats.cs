using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] int maxHealth;
    [SyncVar] public int curHealth;

    public Stat damage;

    public int CurHealth { get { return curHealth; } }

    public override void OnStartServer()
    {
        curHealth = maxHealth;
    }

    public void SetHealthRate(float rate)
    {
        curHealth = rate == 0 ? 0 : (int)(maxHealth / rate);
    }
}

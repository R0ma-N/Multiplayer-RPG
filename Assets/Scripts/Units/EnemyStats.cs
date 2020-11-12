using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : UnitStats
{
    public override void OnStartServer()
    {
        CurHealth = _maxHealth;
    }
}

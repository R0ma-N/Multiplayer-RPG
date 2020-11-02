﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserData
{
    public NetworkHash128 CharacterHash = new NetworkHash128();
    public bool JustCreated = true;
    public Vector3 PosCharacter;
    public Vector3 SpawnPosition;
    public List<int> Inventory = new List<int>();
    public List<int> Equipment = new List<int>();
    public List<int> skills = new List<int>();

    public int Level;
    public int StatPoints;
    public int SkillPoints;
    public float Exp;
    public float NextLevelExp;

    public int CurHealth;
    public int StatDamage;
    public int StatArmor;
    public int StatMoveSpeed;
}

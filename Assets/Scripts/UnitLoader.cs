using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

sealed class UnitLoader : NetworkBehaviour
{
    [SerializeField] GameObject unitPrefab;
    public override void OnStartServer()
    {
        GameObject unit = Instantiate(unitPrefab, transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(unit, gameObject);
    }

}


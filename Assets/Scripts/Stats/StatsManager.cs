using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{
    [SyncVar] public int damage, armor, moveSpeed;
}

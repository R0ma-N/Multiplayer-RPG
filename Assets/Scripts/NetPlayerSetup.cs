using UnityEngine;
using UnityEngine.Networking;

public class NetPlayerSetup : NetworkBehaviour
{
    [SerializeField] MonoBehaviour[] disableBeheviors;

    void Awake()
    {
        if (!hasAuthority)
        {
            for (int i = 0; i < disableBeheviors.Length; i++)
            {
                disableBeheviors[i].enabled = false;
            }
        }
    }

    public override void OnStartAuthority()
    {
        for (int i = 0; i < disableBeheviors.Length; i++)
        {
            disableBeheviors[i].enabled = true;
        }
    }
}

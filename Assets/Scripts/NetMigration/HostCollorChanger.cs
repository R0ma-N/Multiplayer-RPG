using UnityEngine;
using UnityEngine.Networking;

public class HostCollorChanger : NetworkBehaviour
{
    [SerializeField] Renderer _renderer;
    [SerializeField] Material clientMaterial;
    [SerializeField] Material hostMaterial;

    [SyncVar] bool isHost;

    private void Update()
    {
        if (isServer)
        {
            if (isLocalPlayer) isHost = true;
            else isHost = false;
        }
        if (isHost) _renderer.material = hostMaterial;
        else _renderer.material = clientMaterial;
    }
}

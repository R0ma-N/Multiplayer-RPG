using UnityEngine;
using UnityEngine.Networking;

sealed class PlayerController : NetworkBehaviour {

    [SerializeField] LayerMask movementMask;

    Character character;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        this.character = character;
        if (isLocalPlayer) cam.GetComponent<CameraController>().target = character.transform;
    }

    void Update () {
        if (isLocalPlayer)
        {
            if (character != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, movementMask))
                    {
                        CmdSetMovePoint(hit.point);
                    }
                }
            if (Input.GetMouseButtonDown(0)) 
                {
                //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;

                //if (Physics.Raycast(ray, out hit, 100f)) {

                //}
                //motor.Attack();
                //print("mouse");
                }
            }
        }

    }

    [Command]
    public void CmdSetMovePoint(Vector3 point)
    {
        character.SetMovePoint(point);
    }

    private void OnDestroy()
    {
        if (character != null) Destroy(character.gameObject);
    }
}

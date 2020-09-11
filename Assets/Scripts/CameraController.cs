using UnityEngine;

sealed class CameraController : MonoBehaviour {

    [SerializeField] Transform m_target;
    [SerializeField] Vector3 offset;
    [SerializeField] float zoomSpeed = 4f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 15f;
    [SerializeField] float pitch = 2f;

    float currentZoom = 15f;
    float currentRot = 0f;
    float prevMouseX;
    bool hasTarget;

    public Transform target { set { m_target = value; } }

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    void Update() {

            currentZoom -= Input.GetAxis(AxesManager.MouseScroll) * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            if (Input.GetMouseButton(2)) {
                currentRot += Input.mousePosition.x - prevMouseX;
            }

        prevMouseX = Input.mousePosition.x;
    }

    void LateUpdate() {
        transform.position = m_target.position - offset * currentZoom;
        transform.LookAt(m_target.position + Vector3.up * pitch);
        transform.RotateAround(m_target.position, Vector3.up, currentRot);
    }
}

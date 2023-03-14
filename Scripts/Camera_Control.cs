using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Camera_Control : MonoBehaviour
{
    [SerializeField] public GameObject cam_handle;
    private CinemachineVirtualCamera self_vc;
    private float targetFOV = 60f;
    private float targetDis = 10f;
    [SerializeField] private float max_targetFOV = 60f;
    [SerializeField] private float min_targetFOV = 10f;
    [SerializeField] private float max_targetDis = 13f;
    [SerializeField] private float min_targetDis = 3f;
    [SerializeField] private float zoom_speed = 10f; 
    [SerializeField] private float rotateSpeed = 10f;
    private Quaternion BeginRotation;
    private float m_deltX = 0;
    private float m_deltY = 0;
    // Start is called before the first frame update
    void Start()
    {
        self_vc = GetComponent<CinemachineVirtualCamera>();
        BeginRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        cam_movement();
        cam_zoom_Dis();
    }

    private void cam_zoom_Dis()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetDis -= 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetDis += 1;
        }
        targetDis = Mathf.Clamp(targetDis, min_targetDis, max_targetDis);

        float CameraDis = self_vc.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        self_vc.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Lerp(CameraDis, targetDis, Time.deltaTime * zoom_speed);

    }

    private void cam_zoom_FOV()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFOV += 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFOV -= 5;
        }
        targetFOV = Mathf.Clamp(targetFOV, min_targetFOV, max_targetFOV);

        self_vc.m_Lens.FieldOfView = Mathf.Lerp(self_vc.m_Lens.FieldOfView, targetFOV, Time.deltaTime * zoom_speed);
    }

    private void cam_movement()
    {
        if (Input.GetMouseButton(1))
        {
            m_deltX = 0;
            m_deltY = 0;
            BeginRotation = transform.localRotation;
            m_deltX += Input.GetAxis("Mouse X") * rotateSpeed;
            m_deltY -= Input.GetAxis("Mouse Y") * rotateSpeed;
            float new_x = BeginRotation.eulerAngles.x + m_deltY;
            float new_y = BeginRotation.eulerAngles.y + m_deltX;
            new_x = ClampAngle(new_x, 0, 70);
            new_y = ClampAngle(new_y, -360, 360);
            transform.localRotation = Quaternion.Euler(new_x, new_y, 0);
        }



    }

    private float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}

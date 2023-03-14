using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOrbitCam : MonoBehaviour
{
    [SerializeField] public GameObject cam_handle;
    private CinemachineVirtualCamera self_vc;
    private float targetDis = 10f; 
    [SerializeField] private float max_targetDis = 13f;
    [SerializeField] private float min_targetDis = 3f;
    [SerializeField] private float zoom_speed = 10f;
    [SerializeField] private float rotateSpeed = 0.05f;
    private Quaternion BeginRotation;

    // Start is called before the first frame update
    void Start()
    {
        self_vc = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        camAutoOrbit();
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

    private void camAutoOrbit()
    {
        BeginRotation = transform.localRotation;
        float new_y = BeginRotation.eulerAngles.y + rotateSpeed;
        new_y = Mathf.Clamp(new_y, -360, 360);
        transform.localRotation = Quaternion.Euler(BeginRotation.eulerAngles.x, new_y, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam = null;
    [SerializeField] float startingFOV;
    [SerializeField] float maxFOV;
    [SerializeField] float minFOV;
    [SerializeField] float zoomSpeed = .2f;
    CinemachineComponentBase componentBase;

    private void Start()
    {
        componentBase = cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        startingFOV = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && (componentBase as CinemachineFramingTransposer).m_CameraDistance > minFOV)
        {
            componentBase = cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase as CinemachineFramingTransposer).m_CameraDistance -= zoomSpeed;
        }
        else if (Input.mouseScrollDelta.y < 0 && (componentBase as CinemachineFramingTransposer).m_CameraDistance < maxFOV)
        {
            componentBase = cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase as CinemachineFramingTransposer).m_CameraDistance += zoomSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeStation : MonoBehaviour, IInteractable
{
    public CinemachineVirtualCamera cameraOn; // กล้องที่จะเปิด
    public CinemachineVirtualCamera[] camerasOff; // กล้องที่จะปิด

    [SerializeField] private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        EnableCamera();
        DisableCameras();
    }

    public void EnableCamera()
    {
        cameraOn.gameObject.SetActive(true);
    }

    public void DisableCameras()
    {
        foreach (CinemachineVirtualCamera camera in camerasOff)
        {
            camera.gameObject.SetActive(false);
        }
    }

    public void DisableOutline() 
    {
        outline.enabled = false;
    }

    public void  EnableOutline() 
    {
        outline.enabled = true;
    }
}

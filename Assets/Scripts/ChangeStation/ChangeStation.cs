using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeStation : MonoBehaviour, IInteractable
{
    public CinemachineVirtualCamera cameraOn; // กล้องที่จะเปิด
    public CinemachineVirtualCamera[] camerasOff; // กล้องที่จะปิด

    [Header("Outline")]
    public Color onpointerEnterColor;
    public Color onpointerExitColor;
    public Outline outline;

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
        outline.OutlineColor = onpointerExitColor;
    }

    public void  EnableOutline() 
    {
        outline.OutlineColor = onpointerEnterColor;
    }
}

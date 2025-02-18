using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Pour : MonoBehaviour
{
    public ColorPotion colorPotion;
    public ParticleSystem liquidEffect;
    public MeshRenderer meshRenderer;

    [Header("============= เอียงเทขวด =============")]
    public Transform rotationPivot;
    public Vector3 rotationTarget;
    public bool isRotating = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float rotationProgress = 0f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float activationAngleThreshold = 60f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        liquidEffect.gameObject.SetActive(false);
        SetBottleColor(); // ตั้งค่าสีน้ำในขวด
        StartRotation();
    }

    void Update()
    {
        if (isRotating)
        {
            rotationProgress += Time.deltaTime * rotationSpeed;
            rotationPivot.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);

            if (Quaternion.Angle(rotationPivot.transform.rotation, targetRotation) <= activationAngleThreshold)
            {
                if (!liquidEffect.isPlaying)
                {
                    animator.Play("Pour");
                    liquidEffect.gameObject.SetActive(true);
                    liquidEffect.Play();
                    Destroy(gameObject, 1.4f);
                }
            }

            if (rotationProgress >= 1f)
            {
                rotationPivot.transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void StartRotation()
    {
        StartRotation(rotationTarget);
    }

    private void StartRotation(Vector3 targetEulerAngles)
    {
        initialRotation = rotationPivot.rotation;
        targetRotation = Quaternion.Euler(targetEulerAngles);
        rotationProgress = 0f;
        isRotating = true;
    }

    public void SetBottleColor()
    {
        if (meshRenderer == null) return;

        Material selectedMaterial = null;
        Color selectedColor = Color.white; // ใช้ Color ไม่ใช่ ParticleSystem

        switch (colorPotion)
        {
            case ColorPotion.red:
                selectedMaterial = ParticleEffectManager.Instance.redColorBottleMtr;
                selectedColor = ParticleEffectManager.Instance.redColorBottle;
                break;
            case ColorPotion.yellow:
                selectedMaterial = ParticleEffectManager.Instance.yellowColorBottleMtr;
                selectedColor = ParticleEffectManager.Instance.yellowColorBottle;
                break;
            case ColorPotion.blue:
                selectedMaterial = ParticleEffectManager.Instance.blueColorBottleMtr;
                selectedColor = ParticleEffectManager.Instance.blueColorBottle;
                break;
            default:
                Debug.LogWarning("No matching color for this potion");
                break;
        }

        if (selectedMaterial != null)
        {
            meshRenderer.material = selectedMaterial;
        }

        // กำหนดสีให้ ParticleSystem
        var main = liquidEffect.main;
        main.startColor = selectedColor;
    }
}
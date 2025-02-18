using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager Instance;

    [Header("Material น้ำในหม้อปรุงยา")]
    public Material noColorMtr;
    public Material redColorMtr;
    public Material yellowColorMtr;
    public Material blueColorMtr;
    public Material orangeColorMtr;
    public Material greenColorMtr;
    public Material purpleColorMtr;
    public Material brownColorMtr;
    public Material blackColorMtr;

    [Header("Material น้าในขวด")]
    public Material redColorBottleMtr;
    public Color redColorBottle;
    public Material yellowColorBottleMtr;
    public Color yellowColorBottle;
    public Material blueColorBottleMtr;
    public Color blueColorBottle;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

/// <summary>
/// เปลี่ยนสีน้ำในหม้อ
/// </summary>
/// <param name="colorPotion"></param>
/// <param name="waterInWitchPot"></param>
    public void ChangeWaterColor(ColorPotion colorPotion, MeshRenderer waterInWitchPot)
    {
        switch (colorPotion)
        {
            case ColorPotion.NoColor:
                waterInWitchPot.material = noColorMtr;
                break;
            case ColorPotion.red:
                waterInWitchPot.material = redColorMtr;
                break;
            case ColorPotion.yellow:
                waterInWitchPot.material = yellowColorMtr;
                break;
            case ColorPotion.blue:
                waterInWitchPot.material = blueColorMtr;
                break;
            case ColorPotion.orange:
                waterInWitchPot.material = orangeColorMtr;
                break;
            case ColorPotion.green:
                waterInWitchPot.material = greenColorMtr;
                break;
            case ColorPotion.purple:
                waterInWitchPot.material = purpleColorMtr;
                break;
            case ColorPotion.brown:
                waterInWitchPot.material = brownColorMtr;
                break;
            case ColorPotion.black:
                waterInWitchPot.material = blackColorMtr;
                break;   
            default:
                waterInWitchPot.material = noColorMtr;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    Renderer rend; // ใช้สำหรับอ้างอิงถึง Renderer ของวัตถุ

    Vector3 lastPos; // ตำแหน่งของวัตถุในเฟรมที่แล้ว
    Vector3 velocity; // ความเร็วของวัตถุ
    Vector3 lastRot; // การหมุนของวัตถุในเฟรมที่แล้ว
    Vector3 angularVelocity; // ความเร็วในการหมุนของวัตถุ

    public float MaxWobble = 0.03f; // ค่าสูงสุดของการสั่น
    public float WobbleSpeed = 1f; // ความเร็วของการสั่น
    public float Recovery = 1f; // ความเร็วในการคืนค่าสู่สภาพปกติ

    float wobbleAmountX; // ค่าการสั่นในแกน X
    float wobbleAmountZ; // ค่าการสั่นในแกน Z
    float wobbleAmountToAddX; // ค่าการสั่นที่เพิ่มเข้ามาในแกน X
    float wobbleAmountToAddZ; // ค่าการสั่นที่เพิ่มเข้ามาในแกน Z
    float pulse; // ค่าคลื่นไซน์สำหรับการสั่น
    float time = 0.5f; // เวลาเพื่อใช้ในการคำนวณคลื่นไซน์

    // ใช้สำหรับการตั้งค่าเริ่มต้น
    void Start()
    {
        rend = GetComponent<Renderer>(); // ดึง Component Renderer ของวัตถุ
    }

    private void Update()
    {
        time += Time.deltaTime; // เพิ่มค่าเวลาในแต่ละเฟรม

        // ลดค่าการสั่นลงเรื่อยๆ ตามเวลา
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // สร้างคลื่นไซน์สำหรับการสั่น
        pulse = 2 * Mathf.PI * WobbleSpeed; // คำนวณความถี่ของคลื่นไซน์
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time); // คำนวณค่าการสั่นในแกน X
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time); // คำนวณค่าการสั่นในแกน Z

        // ส่งค่าการสั่นไปยัง Shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX); // ส่งค่าการสั่นแกน X ไปยัง Shader
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ); // ส่งค่าการสั่นแกน Z ไปยัง Shader

        // คำนวณความเร็วของวัตถุ
        velocity = (lastPos - transform.position) / Time.deltaTime; // คำนวณความเร็วจากตำแหน่งปัจจุบันและตำแหน่งก่อนหน้า
        angularVelocity = transform.rotation.eulerAngles - lastRot; // คำนวณความเร็วในการหมุน

        // เพิ่มค่าความเร็วเข้าไปในการสั่น (จำกัดค่าไม่ให้เกิน MaxWobble)
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // บันทึกตำแหน่งและการหมุนปัจจุบันเพื่อใช้ในเฟรมถัดไป
        lastPos = transform.position; // บันทึกตำแหน่งปัจจุบัน
        lastRot = transform.rotation.eulerAngles; // บันทึกการหมุนปัจจุบัน
    }
}
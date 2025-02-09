using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float raycastDistance = 10f; // ระยะทางของ Raycast
    private Color rayColor = Color.red; // กำหนดค่าเริ่มต้นให้ Ray เป็นสีแดง

    private IInteractable lastInteractable = null; // เก็บค่า IInteractable ล่าสุดที่เคยโดน Ray

    void Update()
    {
        // สร้าง Ray จากตำแหน่งของเมาส์บนหน้าจอ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ตรวจสอบว่า Raycast ชนกับวัตถุหรือไม่
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // ตรวจสอบว่า GameObject ที่โดน Raycast มี IInteractable หรือไม่
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                rayColor = Color.green; // ถ้าเจอ IInteractable เปลี่ยนเป็นสีเขียว

                // เปิด Outline เมื่อ Ray ชี้ไปที่วัตถุนี้
                if (interactable != lastInteractable)
                {
                    if (lastInteractable != null)
                    {
                        lastInteractable.DisableOutline(); // ปิด Outline ของอันก่อนหน้า
                    }

                    interactable.EnableOutline(); // เปิด Outline ของอันปัจจุบัน
                    lastInteractable = interactable; // อัปเดตค่า IInteractable ล่าสุด
                }

                // ถ้าผู้เล่นคลิกเมาส์ซ้าย
                if (Input.GetMouseButtonDown(0))
                {
                    interactable.Interact(); // เรียกใช้ Interact() ถ้าพบว่า GameObject นั้นมี IInteractable
                }
            }
            else
            {
                rayColor = Color.red; // ถ้าไม่ใช่ IInteractable ให้เป็นสีแดง
                ResetOutline();
            }
        }
        else
        {
            rayColor = Color.red; // ถ้าไม่ชนอะไรเลยก็ให้เป็นสีแดง
            ResetOutline();
        }

        // วาดเส้น Raycast ใน Scene View ตามสีที่กำหนด
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, rayColor);
    }

    /// <summary>
    /// รีเซ็ต Outline ของวัตถุที่เคยถูกชี้ก่อนหน้า
    /// </summary>
    private void ResetOutline()
    {
        if (lastInteractable != null)
        {
            lastInteractable.DisableOutline();
            lastInteractable = null;
        }
    }
}

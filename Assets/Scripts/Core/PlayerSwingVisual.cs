using System.Collections;
using UnityEngine;

public class PlayerSwingVisual : MonoBehaviour
{
    [Header("References")]
    public Transform swingPivot;
    public GameObject swingVisual;

    [Header("Swing Settings")]
    public float swingAngle = 120f;

    private bool isSwinging;

    void Update()
    {
        AimAtMouse();
    }

    void AimAtMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - swingPivot.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        swingPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void PlaySwing(float toolCooldown)  // Accept cooldown as parameter
    {
        if (!isSwinging)
        {
            float swingDuration = Mathf.Max(0.1f, toolCooldown - 0.1f); // Cooldown - 0.1, minimum 0.1
            StartCoroutine(Swing(swingDuration));
        }
    }

    IEnumerator Swing(float duration)  // Accept duration as parameter
    {
        isSwinging = true;
        swingVisual.SetActive(true);

        float startAngle = -swingAngle / 2f;
        float endAngle = swingAngle / 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            swingVisual.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        swingVisual.transform.localRotation = Quaternion.identity;
        swingVisual.SetActive(false);
        isSwinging = false;
    }
}
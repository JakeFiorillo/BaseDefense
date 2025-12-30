using System.Collections;
using UnityEngine;

public class PlayerSwingVisual : MonoBehaviour
{
    [Header("References")]
    public Transform swingPivot;
    public GameObject swingVisual;

    [Header("Swing Settings")]
    [Range(0.1f, 2f)]
    public float swingRadius = 0.4f;
    [Range(60f, 240f)]
    public float swingArcAngle = 120f;
    public AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isSwinging;
    private Vector3 idleLocalPosition = new Vector3(0.2f, -0.2f, 0f);
    private Quaternion idleLocalRotation = Quaternion.Euler(0, 0, 300f);
    private float idleRotationAngle = 300f;  // Store the Z rotation value

    void Start()
    {
        if (swingVisual != null)
        {
            swingVisual.SetActive(true);
            swingVisual.transform.localPosition = idleLocalPosition;
            swingVisual.transform.localRotation = idleLocalRotation;
        }
    }

    void Update()
    {
        if (!isSwinging)
        {
            AimAtMouse();
        }
    }

    void AimAtMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - swingPivot.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        swingPivot.rotation = Quaternion.Euler(0, 0, angle);
        
        swingVisual.transform.localPosition = idleLocalPosition;
        swingVisual.transform.localRotation = idleLocalRotation;
    }

    public void PlaySwing(float toolCooldown)
    {
        if (!isSwinging)
        {
            float swingDuration = Mathf.Max(0.1f, toolCooldown - 0.1f);
            StartCoroutine(Swing(swingDuration));
        }
    }

    public bool IsSwinging => isSwinging;

    IEnumerator Swing(float duration)
    {
        isSwinging = true;

        // Start the swing from the idle rotation angle
        // If idle is 300°, that's the same as -60° (300 - 360 = -60)
        float startAngle = idleRotationAngle;
        float endAngle = startAngle + swingArcAngle;
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curveT = swingCurve.Evaluate(t);
            
            float currentAngle = Mathf.Lerp(startAngle, endAngle, curveT);
            float angleRad = currentAngle * Mathf.Deg2Rad;
            
            Vector3 position = new Vector3(
                Mathf.Cos(angleRad) * swingRadius,
                Mathf.Sin(angleRad) * swingRadius,
                0f
            );
            swingVisual.transform.localPosition = position;
            
            // This should now match the idle rotation at the start
            float tangentAngle = currentAngle;
            swingVisual.transform.localRotation = Quaternion.Euler(0, 0, tangentAngle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        swingVisual.transform.localPosition = idleLocalPosition;
        swingVisual.transform.localRotation = idleLocalRotation;
        isSwinging = false;
    }
}
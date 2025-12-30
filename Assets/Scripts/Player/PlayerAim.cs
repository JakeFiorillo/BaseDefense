using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform swingPivot;

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2 direction = mouseWorld - swingPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        swingPivot.rotation = Quaternion.Euler(0, 0, angle);
    }
}

using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    private PlayerHarvest playerHarvest;

    void Start()
    {
        // Get reference to PlayerHarvest on the parent player object
        playerHarvest = GetComponentInParent<PlayerHarvest>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit something harvestable
        if (other.GetComponent<Harvestable>() != null)
        {
            Debug.Log($"Weapon hit: {other.gameObject.name}");
            
            // Tell PlayerHarvest we hit something
            if (playerHarvest != null)
            {
                playerHarvest.OnWeaponHit(other);
            }
        }
    }
}
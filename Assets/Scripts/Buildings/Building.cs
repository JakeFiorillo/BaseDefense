using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building Info")]
    public BuildingType buildingType;
    public string buildingName;
    public int maxHealth;
    public int currentHealth;
    
    [Header("Upgrade Info")]
    public int upgradeLevel = 0;  // 0 = Wooden, 1 = Stone, 2 = Gold, 3 = Emerald, 4 = Diamond, 5 = Ruby
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{buildingName} took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            OnDestroyed();
        }
    }
    
    protected virtual void OnDestroyed()
    {
        Debug.Log($"{buildingName} destroyed!");
        
        // Notify BuildingManager that this building was destroyed
        if (BuildingManager.Instance != null)
        {
            BuildingManager.Instance.OnBuildingDestroyed(this);
        }
        
        Destroy(gameObject);
    }
}
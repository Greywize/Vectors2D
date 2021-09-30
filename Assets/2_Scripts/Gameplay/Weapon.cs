using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Projectile Weapon", menuName = "Weapons/Basic", order = 0)]
public class Weapon : MonoBehaviour
{
    // --- Objects
    public Color color;
    public GameObject projectilePrefab;

    // --- Varaibles
    [SerializeField]
    [Tooltip("Damage dealt by each projectile")]
    public float damage;
    [SerializeField]
    [Tooltip("How many units projectiles fired from this weapon will fire")]
    public float range;
    [SerializeField]
    [Tooltip("How many projectiles fired at a time")]
    public float count;
    [SerializeField]
    [Tooltip("How accurate each projectile will be")]
    [Range(0,100)]
    public float accuracy;
    [SerializeField]
    [Tooltip("How long im seconds it takes to reload")]
    public float cooldown;
    private float time;

    [HideInInspector]
    public bool onCooldown = false;

    // --- Vectors
    private Vector2 spread;

    public void Shoot(Vector2 direction, Vector2 position)
    {
        // Scale accuracy to be between 0-1
        float scaledAccuracy = (100 - accuracy) * 0.01f;

        // For every projectile we want to fire
        for (int i = 0; i < count; i++)
        {   
            // Spawn in projectile at origin without a parent
            GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity, null);

            spread = Random.insideUnitCircle * scaledAccuracy;

            Player player = transform.parent.GetComponent<Player>();
            player.projectiles.Add(projectile, 1f);

            projectile.GetComponent<SpriteRenderer>().color = color;
            projectile.GetComponent<TrailRenderer>().startColor = color;
            projectile.GetComponent<TrailRenderer>().endColor = color;
            projectile.GetComponent<Rigidbody2D>().AddForce((direction + spread).normalized * range, ForceMode2D.Impulse);
        }
    }

    public IEnumerator StartCooldown()
    {
        time = cooldown;
        onCooldown = true;

        while (onCooldown)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                onCooldown = false;

                time = cooldown;
            }
            else
                yield return null;
        }
    }
}
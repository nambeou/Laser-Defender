using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject laser;
    [SerializeField] int health = 100;
    [SerializeField] int initialHealth;
    [SerializeField] AudioClip shootingSound;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0,1)] float explosionVolume = 0.3f;
    [SerializeField] [Range(0,1)] float shootingVolume = 0.7f;
    
    // Shooting related stuff
    float minProjectileSpeed = 5;
    float maxProjectileSpeed = 12;
    float projectileSpeed;
    float minTimeBetweenShots = 0.5f;
    float maxTimeBetweenShots = 3;

    // Screen stuff
    float xMin, xMax, yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        Level level = FindObjectOfType<Level>();
        this.initialHealth = (int)( this.health + (1+level.GetCurrentLevel()*0.2));
        this.GetComponent<DamageDealer>().SetDamage((int)this.initialHealth/10);
        projectileSpeed = Random.Range(minProjectileSpeed, maxProjectileSpeed);
        StartCoroutine(ContinuousFire());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageDealer>() != null) {
            HandleDamage(other.gameObject.GetComponent<DamageDealer>());
        }
    }

    private void HandleDamage(DamageDealer damageDealer) {
        this.health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (this.health <= 0) {
            Die();
        }
    }

    private void Die() {
            FindObjectOfType<GameSession>().AddScore(this.initialHealth);
            TriggerExplosion();
            Destroy(gameObject, 0.1f);
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionVolume);
    }

    private void TriggerExplosion() {
        GameObject sparkle = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(sparkle, 1f);
    }
    IEnumerator ContinuousFire() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenShots, maxTimeBetweenShots));
            SpawnLaser();
        }
    }

    private void SpawnLaser() {
        GameObject projectile = Instantiate(
            laser, 
            transform.position, 
            Quaternion.Euler(0, 0, CalculateRotationToPlayer()));
        projectile.GetComponent<Rigidbody2D>().velocity = CalculateLaserVelocity(CalculateToPlayerVector());
        projectile.AddComponent<DamageDealer>();
        projectile.GetComponent<DamageDealer>().SetDamage(GetComponent<DamageDealer>().GetDamage());
        AudioSource.PlayClipAtPoint(shootingSound, Camera.main.transform.position, shootingVolume);
    }

    private Vector2 CalculateLaserVelocity(Vector2 toPlayerVector)
    {
        float x = toPlayerVector.x;
        float y = toPlayerVector.y;
        float sin = y/(float)System.Math.Sqrt(x*x + y*y);
        float cos = x/(float)System.Math.Sqrt(x*x + y*y);
        return new Vector2 (
            cos * projectileSpeed, 
            sin * projectileSpeed);
    }

    private float CalculateRotationToPlayer()
    {
        Vector2 playerToMouseVector = CalculateToPlayerVector();
        float x = playerToMouseVector.x;
        float y = playerToMouseVector.y;

        float sin = y/(float)System.Math.Sqrt(x*x + y*y);
        float cos = x/(float)System.Math.Sqrt(x*x + y*y);
        float rot = -90*cos;
        if (sin < 0) {
            rot = (cos < 0 ? 180 : -180) - rot;
        }
        return rot;
    }

    private Vector2 CalculateToPlayerVector()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null) {
            return new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        } 
        return transform.position;
    }
}

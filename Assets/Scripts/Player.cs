using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [Range(0.1f, 20)] [SerializeField] float velocityFactor = 10;
    [Range(0.1f, 20)] [SerializeField] float rotationFactor = 1;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip loseSound;
    [SerializeField] [Range(0,1)] float loseVolume = 0.3f;
    Vector2 size;

    [Header("Game Area")]
    [Range(1f, 10)] [SerializeField] float padding = 1f; // maybe for vertical movements
    float xMin, xMax, yMin, yMax;

    [Header("Firing")]
    [SerializeField] GameObject laser;
    [Range(0.01f, 0.5f)] [SerializeField] float fireDelay = 0.05f;
    [Range(1, 10)] [SerializeField] float laserVelocityFactor = 10f;
    Coroutine firingCoroutine;


    void Start()
    {
        this.size = GetComponent<SpriteRenderer>().bounds.size; 
        DefineBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Move() {
        float newX = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * velocityFactor;
        float newY = transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * velocityFactor;
        // move
        transform.position = new Vector2(
            Mathf.Clamp(newX, xMin, xMax), 
            Mathf.Clamp(newY, yMin, yMax));
        // rotate
        Rotate(gameObject);
    }

    private void DefineBoundaries() {
        Camera camera = Camera.main;
        xMin = camera.ViewportToWorldPoint(new Vector3(0,0,0)).x;
        xMax = camera.ViewportToWorldPoint(new Vector3(1,0,0)).x;
        yMin = camera.ViewportToWorldPoint(new Vector3(0,0,0)).y;
        yMax = camera.ViewportToWorldPoint(new Vector3(0,1,0)).y;

    }

    private void Fire() {
        if (Input.GetButtonDown("Fire1") ) {
            firingCoroutine = StartCoroutine(ContinuousFire());
        }
        if(Input.GetButtonUp("Fire1")) {
            StopCoroutine(firingCoroutine);
        }
    }

    private void SpawnLaser() {
        GameObject projectile = Instantiate(
            laser, 
            transform.position, 
            Quaternion.Euler(0, 0, CalculateRotationToMouse()));
        projectile.AddComponent<DamageDealer>();
        projectile.GetComponent<DamageDealer>().SetDamage(GetComponent<DamageDealer>().GetDamage());
        projectile.GetComponent<Rigidbody2D>().velocity = CalculateLaserVelocity(CalculateMouseToPlayerVector());
    }

    private void Rotate(GameObject obj) {
        Quaternion target = Quaternion.Euler(0, 0, CalculateRotationToMouse());

        // Dampen towards the target rotation
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, target, rotationFactor);
    }

    private float CalculateRotationToMouse() {
        Vector2 playerToMouseVector = CalculateMouseToPlayerVector();
        float x = playerToMouseVector.x;
        float y = playerToMouseVector.y;

        float sin = y/(float)Math.Sqrt(x*x + y*y);
        float cos = x/(float)Math.Sqrt(x*x + y*y);
        float rot = -90*cos;
        if (sin < 0) {
            rot = (cos < 0 ? 180 : -180) - rot;
        }
        return rot;
    }

    private Vector2 CalculateLaserVelocity(Vector2 playerToMouseVector) {
        float x = playerToMouseVector.x;
        float y = playerToMouseVector.y;
        float sin = y/(float)Math.Sqrt(x*x + y*y);
        float cos = x/(float)Math.Sqrt(x*x + y*y);
        return new Vector2 (
             cos * laserVelocityFactor, 
             sin * laserVelocityFactor);
    }

    private Vector2 CalculateMouseToPlayerVector() {
        Vector2 mousePos = Input.mousePosition;
        float x = (Mathf.Clamp(mousePos.x/Screen.width, 0, 1) - 0.5f) * (xMax - xMin);
        float y = (Mathf.Clamp(mousePos.y/Screen.height, 0, 1) - 0.5f) * (yMax - yMin);
        return new Vector2(x - transform.position.x,y - transform.position.y);
    }
    IEnumerator ContinuousFire() {
        while (true) {
            SpawnLaser();
            yield return new WaitForSeconds(fireDelay);
        }
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
        Destroy(gameObject, 0.1f);
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, loseVolume);
        FindObjectOfType<Level>().LoadGameOver();
    }

    public int GetHealth() {
        return this.health <= 0 ? 0 : this.health;
    }
}

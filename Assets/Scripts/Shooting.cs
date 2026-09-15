using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public static Shooting instance;
    // The sprite used when not shooting
    public Sprite spriteIdle;
    // The sprite used when shooting
    public Sprite spriteShooting;
    // The sprite used when reloading
    public Sprite spriteReloading;
    // The sound used when shooting
    public AudioSource shootSound;

    // The bullet prefab
    public GameObject bullet;
    // The shoot effect light
    public GameObject shootLight;

    // The cooldown on shooting your gun
    float cooldown = 0f;
    // The wait time for reloading
    float reloadTimer = 0f;
    // The visual effect for shooting
    float shootEffectTimer = 0f;
    // The cooldown for punching
    float punchCoolDown = 0f;

    // How much ammo the player can have
    int maxAmmo = 10;
    // How much ammo the player has
    int ammo = 10;
    // How much ammo the player has stockpiled
    int totalAmmo = 15;

    SpriteRenderer spriteRenderer;

    public GameObject blood;
    public GameObject wood;
    private void SpawnDecals(GameObject decal)
    {
        // Spawn a bunch of random decals
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
            Instantiate(decal, transform.position + new Vector3((UnityEngine.Random.value - 0.5f) * 1.4f, (UnityEngine.Random.value - 0.5f) * 1.4f, 0), transform.rotation).transform.localScale = new Vector3(UnityEngine.Random.Range(1.2f, 1.7f), UnityEngine.Random.Range(1.2f, 1.7f), 0);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        // Set the ammo by default to fill the entire magazine
        ammo = maxAmmo;
        // The sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        UIManager.instance.SetBulletCount(ammo, maxAmmo);
        UIManager.instance.SetTotalBulletCount(totalAmmo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("AmmoBox"))
        {
            totalAmmo = totalAmmo + 10;
            UIManager.instance.SetTotalBulletCount(totalAmmo);
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        if (UIManager.instance.gamePaused) return;
        Debug.DrawRay(transform.position, transform.right, Color.blue);

        // Reduce the remaining time the player has left on shooting cooldown
        cooldown -= Time.deltaTime;
        // Reduce the remaining time of the shooting effect
        shootEffectTimer -= Time.deltaTime;
        // Reduce the remaining time of the reload
        reloadTimer -= Time.deltaTime;
        // Reduce the remaining time of the punch cooldown
        punchCoolDown -= Time.deltaTime;

        // If the shoot effect is playing
        if (shootEffectTimer > 0f && reloadTimer <= 0f)
        {
            // Enable the gun light
            shootLight.SetActive(true);
            // Show the shooting sprite
            spriteRenderer.sprite = spriteShooting;
        }
        else
        {
            // Disable the gun light
            shootLight.SetActive(false);
            // Show the idle sprite
            spriteRenderer.sprite = spriteIdle;
        }

        if (reloadTimer > 0f)
            spriteRenderer.sprite = spriteReloading;

        /* Check if the player:
             1. Is holding R
             2. Is not on shoot cooldown
             3. Does not have a full magazine
        */
        if (Input.GetKeyDown("r") && (cooldown < 0f && reloadTimer <= 0f) && ammo < maxAmmo && totalAmmo > 0)
        {
            /*
            Calculate how many bullets we take from the storage,
            We need to make sure we don't take more bullets than we have, and also that we dont take more bullets than can fit in the gun
            math.min means that it returns the value with the lowest number. So if maxAmmo - ammo (the amount we need to fill up the gun)
            is higher than totalAmmo (how many bullets we have left), it will only add as many bullets as we have.
            So if your bullet count looks like 2/10 5 and you reload, it will look like 7/10 0 and not 10/10 -3.
            */
            int bulletsToTake = math.min(maxAmmo - ammo, totalAmmo);
            // Add the new ammo
            ammo += bulletsToTake;
            // Remove ammo from storage
            totalAmmo -= bulletsToTake;

            reloadTimer = 1.4f;

            // Update the gui
            UIManager.instance.SetBulletCount(ammo, maxAmmo);
            UIManager.instance.SetTotalBulletCount(totalAmmo);
        }

        if (Input.GetKeyDown("f") && reloadTimer <= 0f && punchCoolDown <= 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1);
            if (hit)
            {
                if (hit.collider.CompareTag("Zombie"))
                {
                    hit.collider.gameObject.GetComponent<ZHealth>().DamageZombie(1);
                    SpawnDecals(blood);
                }
                if (hit.collider.CompareTag("Crate"))
                {
                    hit.collider.gameObject.GetComponent<CrateHealth>().DamageCrate(2);
                    SpawnDecals(wood);
                }
            }
            punchCoolDown = 0.2f;
        }

        /* Check if the player:
             1. Is holding the left mouse button
             2. Is not on shoot cooldown
             3. Has ammo
        */
        if (Input.GetMouseButton(0) && (cooldown < 0f && reloadTimer <= 0f) && ammo > 0)
        {
            shootEffectTimer = 0.03f;

            shootSound.Play();

            // Spawn in a bullet and rotate it by 90 degrees to match the player
            var shotBullet = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, 90));
            // Get the rigidbody of the player and add a lot of velocity to it
            var bulletRb = shotBullet.GetComponent<Rigidbody2D>(); bulletRb.linearVelocity = transform.right * 100;

            // Remove 1 ammo
            ammo--;

            UIManager.instance.SetBulletCount(ammo, maxAmmo);

            // Set the shoot cooldown to 0.2 seconds
            cooldown = 0.2f;
        }

    }
}
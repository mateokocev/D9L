using UnityEngine;

public class PlayerProjectileLauncher : MonoBehaviour
{

    public GameObject pistolProjectilePrefab;
    public GameObject arProjectilePrefab;
    public GameObject shotgunProjectilePrefab;

    private bool isFiringAR = false;
    private float lastARBullet;
    private float arFireDelay = 1f / 8f;

    private float lastPistolBullet;
    private float pistolFireDelay = 1f / 4f;

    private float lastShotgunBullet;
    private float shotgunFireDelay = 1f;

    private WeaponType lastPickedUpWeapon = WeaponType.Pistol;  // Pištolj je osnovna vrijednost

    private PlayerProjectileGenerator playerProjectileGenerator;

    void Start()
    {

        playerProjectileGenerator = new PlayerProjectileGenerator(pistolProjectilePrefab, arProjectilePrefab, shotgunProjectilePrefab);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && lastPickedUpWeapon == WeaponType.AR)
        {

            SetFiringModeTrue();
        }
        else if (Input.GetMouseButtonDown(0) && lastPickedUpWeapon != WeaponType.AR)
        {

            if(Time.time - lastPistolBullet > pistolFireDelay && lastPickedUpWeapon == WeaponType.Pistol)
            {

                PlayerProjectileLaunch();
                lastPistolBullet = Time.time;
            }

            if (Time.time - lastShotgunBullet > shotgunFireDelay && lastPickedUpWeapon == WeaponType.Shotgun)
            {

                for (int i = 0; i < 9; i++)
                {

                    PlayerProjectileLaunch();
                }
                lastShotgunBullet = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0) && lastPickedUpWeapon == WeaponType.AR)
        {

            SetFiringModeFalse();
        }
        else if (isFiringAR && lastPickedUpWeapon == WeaponType.AR)
        {

            if (Time.time - lastARBullet >= arFireDelay)
            {

                PlayerProjectileLaunch();
                lastARBullet = Time.time;
            }
        }
    }

    void PlayerProjectileLaunch()
    {

        GameObject projectile = playerProjectileGenerator.CreateProjectile(lastPickedUpWeapon);

        if (projectile != null)
        {

            projectile.transform.position = transform.position;
        }
    }

    public void SetFiringModeTrue()
    {

        isFiringAR = true;
    }

    public void SetFiringModeFalse()
    {

        isFiringAR = false;
    }

    public void SetWeaponType(WeaponType weaponType)
    {

        lastPickedUpWeapon = weaponType;
    }

    public WeaponType GetWeaponType()
    {

        return lastPickedUpWeapon;
    }
}

public class PlayerProjectileGenerator
{

    private GameObject pistolProjectilePrefab;
    private GameObject arProjectilePrefab;
    private GameObject shotgunProjectilePrefab;

    public PlayerProjectileGenerator(GameObject pistolProjectilePrefab, GameObject arProjectilePrefab, GameObject shotgunProjectilePrefab)
    {

        this.pistolProjectilePrefab = pistolProjectilePrefab;
        this.arProjectilePrefab = arProjectilePrefab;
        this.shotgunProjectilePrefab = shotgunProjectilePrefab;
    }

    public GameObject CreateProjectile(WeaponType weaponType)
    {

        GameObject projectilePrefab = null;

        switch (weaponType)
        {

            case WeaponType.Pistol:
                projectilePrefab = pistolProjectilePrefab;
                break;

            case WeaponType.AR:
                projectilePrefab = arProjectilePrefab;
                break;

            case WeaponType.Shotgun:
                projectilePrefab = shotgunProjectilePrefab;
                break;

            default:
                break;
        }

        if (projectilePrefab != null)
        {

            GameObject projectile = GameObject.Instantiate(projectilePrefab);

            switch (weaponType)
            {

                case WeaponType.Pistol:
                    projectile.AddComponent<PistolBehaviour>();
                    break;

                case WeaponType.AR:
                    projectile.AddComponent<ARBehaviour>();
                    break;

                case WeaponType.Shotgun:
                    projectile.AddComponent<shotgunBehaviour>();
                    break;

                default:
                    break;
            }

            return projectile;
        }

        return null;
    }
}
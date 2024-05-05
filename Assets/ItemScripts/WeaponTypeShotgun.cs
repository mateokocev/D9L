using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponTypeShotgun : MonoBehaviour
{

    public float duration = 30f;

    private float timer = 0f;
    private bool isPlayerInRange = false;
    private PlayerProjectileLauncher playerProjectileLauncher;
    private WeaponType originalWeaponType = WeaponType.Pistol;
    private WeaponType newWeaponType = WeaponType.Shotgun;
    private Collider2D itemCollider;
    private Renderer itemRenderer;

    void Start()
    {

        itemCollider = GetComponent<Collider2D>();
        itemRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {

        if (otherObject.CompareTag("Player"))
        {

            playerProjectileLauncher = FindAnyObjectByType<PlayerProjectileLauncher>();
            if (playerProjectileLauncher != null)
            {

                playerProjectileLauncher.SetWeaponType(newWeaponType);

                itemCollider.enabled = false;
                itemRenderer.enabled = false;

                StartCoroutine(SetWeaponTypeToOriginal());
            }
        }
    }

    IEnumerator SetWeaponTypeToOriginal()
    {

        yield return new WaitForSeconds(duration);

        playerProjectileLauncher.SetWeaponType(originalWeaponType);

        Destroy(gameObject);
    }
}
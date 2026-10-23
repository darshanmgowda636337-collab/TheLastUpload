using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Ammo")]
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private int reserveAmmo = 48;

    private int currentAmmo;
    private float nextFireTime;

    private void Start()
    {
        currentAmmo = magazineSize;

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Mouse.current == null || playerCamera == null)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime)
        {
            Shoot();
        }

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
            return;

        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            TargetHealth target =
                hit.collider.GetComponentInParent<TargetHealth>();

            if (target != null)
                target.TakeDamage(damage);
        }

        Debug.Log("Ammo: " + currentAmmo + "/" + reserveAmmo);
    }

    private void Reload()
    {
        if (currentAmmo == magazineSize || reserveAmmo <= 0)
            return;

        int needed = magazineSize - currentAmmo;
        int loaded = Mathf.Min(needed, reserveAmmo);

        currentAmmo += loaded;
        reserveAmmo -= loaded;

        Debug.Log("Reloaded: " + currentAmmo + "/" + reserveAmmo);
    }
}
using UnityEngine;
using System.Collections.Generic;

public class shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private Animator crossbowAnimator;
    [SerializeField] private string shootTrigger = "Shoot";

    private readonly List<GameObject> bullets = new List<GameObject>();
    private float nextFireTime;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearBullets);
    }

    public void Shoot()
    {
        if (Time.timeScale == 0f)
            return;

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireCooldown;

        if (crossbowAnimator != null)
            crossbowAnimator.SetTrigger(shootTrigger);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullets.Add(bullet);
    }

    private void ClearBullets()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            if (bullets[i] != null)
                Destroy(bullets[i]);
        }

        bullets.Clear();
    }
}

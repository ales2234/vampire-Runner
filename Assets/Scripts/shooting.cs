using UnityEngine;
using System.Collections.Generic;

public class shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private readonly List<GameObject> bullets = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearBullets);
    }

    public void Shoot()
    {
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

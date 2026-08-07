using UnityEngine;
using System.Collections.Generic;

public class shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private Animator crossbowAnimator;
    [SerializeField] private string shootTrigger = "Shoot";
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0f, 1f)] private float shootVolume = 1f;

    private readonly List<GameObject> bullets = new List<GameObject>();
    private float nextFireTime;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

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

        PlayShootSound();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullets.Add(bullet);
    }

    private void PlayShootSound()
    {
        if (shootSound == null)
            return;

        if (audioSource != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
        else
            AudioSource.PlayClipAtPoint(shootSound, firePoint != null ? firePoint.position : transform.position, shootVolume);
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

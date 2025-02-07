using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public float timeBetweenBullets = 0.15f;
    public GameObject projectile;
    public Slider playerAmmoSlider;
    public int maxRounds;
    public int startingRounds;
    int remainingRounds;
    private float nextbullet;
    AudioSource gunMuzzleAS;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public Sprite weaponSprite;
    public Image weaponImage;
    // Start is called before the first frame update
    void Awake()
    {
        nextbullet = 0f;
        remainingRounds = startingRounds;
        playerAmmoSlider.maxValue = maxRounds;
        playerAmmoSlider.value = remainingRounds;
        gunMuzzleAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController myPlayer = transform.root.GetComponent<PlayerController>();
        if (Input.GetAxisRaw("Fire1") > 0 && nextbullet < Time.time && remainingRounds > 0)
        {
            nextbullet = Time.time + timeBetweenBullets;
            Vector3 rot;
            if (myPlayer.GetFacing() == -1f)
            {
                rot = new Vector3(0, -90, 0);
            }
            else 
                rot = new Vector3(0, 90, 0);
            Instantiate(projectile, transform.position, Quaternion.Euler(rot));
            playASound(shootSound);
            remainingRounds -= 1;
            playerAmmoSlider.value = remainingRounds;
        }
    }
    public void reload()
    {
        remainingRounds = maxRounds;
        playerAmmoSlider.value = remainingRounds;
        playASound(reloadSound);
    }
    void playASound(AudioClip playTheSound)
    {
        gunMuzzleAS.clip = playTheSound;
        gunMuzzleAS.Play();
    }
    public void initializeWeapon()
    {
        gunMuzzleAS.clip = reloadSound;
        gunMuzzleAS.Play();
        nextbullet = 0;
        playerAmmoSlider.maxValue = maxRounds;
        playerAmmoSlider.value = remainingRounds;
        weaponImage.sprite = weaponSprite;
    }
}

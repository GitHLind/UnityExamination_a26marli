using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string playerTag = "Player";
    public int value = 1;
    public AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (ScoreCounter.Instance != null) ScoreCounter.Instance.Add(value);
        if (pickupSfx != null) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Fade : MonoBehaviour
{
    public float duration;

    int direction;
    float remaining;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        remaining = duration;
        direction = 1;
    }

    private void Update()
    {
        remaining -= Time.deltaTime * direction;
        Color color = sprite.color;
        color.a = Mathf.Clamp01( (duration - remaining) / duration);
        sprite.color = color;
    }
}

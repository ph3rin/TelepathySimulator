using UnityEngine;

internal class ShatteredBlockBehaviour : MonoBehaviour
{
    private SpriteRenderer sprite_renderer_;
    private void Awake()
    {
        GetComponent<Collider2D>().enabled = false;
        sprite_renderer_ = GetComponent<SpriteRenderer>();
        gameObject.AddComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 3f;
    }
    private void Update()
    {
        sprite_renderer_.color = new Color(sprite_renderer_.color.r, sprite_renderer_.color.g, sprite_renderer_.color.g, 
            sprite_renderer_.color.a - Time.deltaTime * 0.8f);
        if (sprite_renderer_.color.a <= 0.01f)
        {
            Destroy(this.gameObject);
        }
    }
}
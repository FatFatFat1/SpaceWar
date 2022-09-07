using UnityEngine;

public class SpriteRandom : MonoBehaviour
{
    private SpriteRenderer _sRender;
    [SerializeField] private Sprite[] _sprite;

    void Start()
    {
        _sRender = GetComponent<SpriteRenderer>();
        _sRender.sprite = _sprite[Random.Range(0, _sprite.Length)];
    }
}

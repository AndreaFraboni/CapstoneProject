using System.Collections;
using UnityEngine;
public class ExplosionWave : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 5f;
    [SerializeField] private float _duration = 0.5f;

    private Renderer _renderer;
    public int _damage = 10;
    public float speed = 1f;

    public DamageTarget _damageTarget = DamageTarget.Enemy;

    private Coroutine _explosionScaling;

    private void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        _explosionScaling = StartCoroutine(SphereScaling());
    }

    private IEnumerator SphereScaling()
    {
        float time = 0f;
        float diameter = _maxRadius * 2f;

        while (time < _duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp(time / _duration, 0f, 1f);
            float scale = diameter * t;
            transform.localScale = Vector3.one * scale;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (_explosionScaling != null)
        {
            StopCoroutine(_explosionScaling);
            _explosionScaling = null;
        }
    }

    public void SetDamageWave(int damage)
    {
        _damage = damage;
    }

    public void SetMaterialWave(Material material)
    {
        if (_renderer != null)
        {
            Material newMaterial = new Material(material);
            newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, 0f);
            _renderer.material = newMaterial;
        }
    }

    public void SetDamageTarget(DamageTarget damageTarget)
    {
        _damageTarget = damageTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LifeController>(out LifeController life))
        {
            switch (_damageTarget)
            {
                case DamageTarget.Enemy:
                    if (other.CompareTag(Tags.Enemy))
                    {
                        life.TakeDamage(_damage);
                    }
                    break;

                case DamageTarget.Player:
                    if (other.CompareTag(Tags.Player))
                    {
                        life.TakeDamage(_damage);
                    }
                    break;
            }
        }
    }
}

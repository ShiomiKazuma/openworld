using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _effects;
    [SerializeField] ParticleSystem[] _hitEffects;

    public void EffectPlay(int number)
    {
        _effects[number].Play();
    }

    public void EffectStop(int number)
    {
        _effects[number].Stop();
    }

    public void HitEffectPlay(int number)
    {
        ParticleSystem particle = Instantiate(_hitEffects[number], this.transform.position, transform.rotation);
        particle.Play();
    }
}

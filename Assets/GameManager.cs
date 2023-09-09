using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Player _player;

    void Awake()
    {
        _player.OnFall.Subscribe(life => _audioSource.PlayOneShot(_audioSource.clip)).AddTo(this);
        _player.OnDead.Subscribe(_ => Debug.Log("Game Over")).AddTo(this);
    }
}
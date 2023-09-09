using UnityEngine;
using System;
using UniRx.Triggers;
using UniRx;

public class Player : MonoBehaviour
{
    int _life = 3;
    Vector3 _initialPosition;
    Subject<int> _fallSubject = new Subject<int>();
    Subject<Unit> _deadSubject = new Subject<Unit>();
    public IObservable<int> OnFall => _fallSubject;
    public IObservable<Unit> OnDead => _deadSubject;

    void Start()
    {
        _initialPosition = transform.position;

        // 落下に関するメインストリーム
        var fallObservable = this.UpdateAsObservable()
            .Select(_ => transform.position.y)
            .Where(y => y <= -20.0f)
            .Publish()
            .RefCount();

        // 落下監視
        fallObservable
            .Subscribe(_ =>
            {
                _fallSubject.OnNext(--_life);
                transform.position = _initialPosition;
            })
            .AddTo(gameObject);

        // ゲームオーバー監視
        fallObservable
            .Select(_ => _life)
            .Where(life => life == 0)
            .Subscribe(_ => { _deadSubject.OnNext(Unit.Default); })
            .AddTo(gameObject);
    }

    void OnDestroy()
    {
        // Subject の破棄
        _fallSubject.Dispose();
        _deadSubject.Dispose();
    }
}
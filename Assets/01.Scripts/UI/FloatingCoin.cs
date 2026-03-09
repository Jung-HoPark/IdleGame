using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class FloatingCoin : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 100.0f;
    [SerializeField] private float _lifeTime = 1f;

    [SerializeField] private Image _coinImage;

    public async UniTaskVoid AnimateUpward()
    {
        transform.SetAsLastSibling();

        Color c = _coinImage.color;
        c.a = 1f;
        _coinImage.color = c;

        float elapsedTime = 0f;

        RectTransform rect = transform as RectTransform;

        while (elapsedTime < _lifeTime)
        {
            rect.anchoredPosition += Vector2.up * _moveSpeed * Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, elapsedTime / _lifeTime);
            _coinImage.color = c;

            elapsedTime += Time.deltaTime;

            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }

        GameManager.Instance.Pool.ReturnFloatingCoin(this);
    }

}

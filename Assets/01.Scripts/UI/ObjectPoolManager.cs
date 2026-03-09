using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public FloatingCoin CoinPrefab;
    private int _poolSize = 30;


    public Transform UIPoolParent;

    public Queue<FloatingCoin> _coinPool = new Queue<FloatingCoin>();

    public void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            FloatingCoin newText = Instantiate(CoinPrefab, UIPoolParent);
            newText.gameObject.SetActive(false);
            _coinPool.Enqueue(newText);
        }
    }

    public FloatingCoin GetFloatingCoin()
    {
        if (_coinPool.Count > 0)
        {
            FloatingCoin text = _coinPool.Dequeue();
            text.gameObject.SetActive(true);        
            return text;                            
        }
        else
        {
            FloatingCoin newText = Instantiate(CoinPrefab, UIPoolParent);
            newText.gameObject.SetActive(true);
            return newText;
        }
    }
    public void ReturnFloatingCoin(FloatingCoin textToReturn)
    {
        textToReturn.gameObject.SetActive(false); 
        _coinPool.Enqueue(textToReturn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockMarketManager : MonoBehaviour
{
    public List<StockData> stockList = new List<StockData>();
    public float updateInterval = 1.0f; // 1초마다 갱신

    // 상속받거나 참조해야 할 다른 매니저들 

    void Start()
    {
        // 샘플 데이터 
        stockList.Add(new StockData("애플망고", 50000));
        stockList.Add(new StockData("이성전자", 70000));
        stockList.Add(new StockData("SK로우닉스", 100000));

        StartCoroutine(UpdateStockMarket());
    }

    IEnumerator UpdateStockMarket()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            UpdatePrices();
        }
    }

    void UpdatePrices()
    {
        foreach (var stock in stockList)
        {
            // 1. 기본 변동 (Random Walk: -5% ~ +5%)
            float randomChange = Random.Range(-0.05f, 0.05f);

            // 2. 모멘텀 및 기본 우상향 반영 (+0.2% 기본 성장세 추가)
            float trend = stock.momentum + 0.002f;

            // 3. 폭락 확률 (약 2% 확률로 대폭락 발생)
            if (Random.value < 0.02f)
            {
                randomChange = Random.Range(-0.2f, -0.4f); // -20% ~ -40% 폭락
                stock.momentum = -0.05f; // 폭락 후엔 한동안 하락세 유지
                Debug.Log($"{stock.stockName} 폭락 발생!");
            }
            else
            {
                // 평소에는 모멘텀을 조금씩 회복 (0으로 수렴하게 하거나 우상향으로 유도)
                stock.momentum = Mathf.Lerp(stock.momentum, 0.01f, 0.1f);
            }

            // 4. 가격 계산 적용
            double nextPrice = stock.currentPrice * (1 + randomChange + trend);

            // 주가가 1원 밑으로는 안 떨어지게 방어
            if (nextPrice < 1) nextPrice = 1;

            stock.currentPrice = nextPrice;

            // 5. Queue 데이터 갱신 (최근 50개 유지)
            stock.priceHistory.Enqueue(nextPrice);
            if (stock.priceHistory.Count > 50)
            {
                stock.priceHistory.Dequeue();
            }
        }

        // UI 매니저에게 갱신 알림 
        // UIManager.Instance.RefreshStockUI();
    }

    // 매수 로직
    public void BuyStock(StockData stock, int amount)
    {
        double totalCost = stock.currentPrice * amount;

        // TODO: 플레이어쪽에서 돈이 있는지 체크 후 차감
       

        // 평균 단가 계산: (기존총액 + 신규총액) / 전체수량
        double currentTotal = stock.purchasePrice * stock.ownedCount;
        stock.ownedCount += amount;
        stock.purchasePrice = (currentTotal + totalCost) / stock.ownedCount;
    }

    // 매도 로직
    public void SellStock(StockData stock, int amount)
    {
        if (stock.ownedCount >= amount)
        {
            stock.ownedCount -= amount;
            double gain = stock.currentPrice * amount;
            // TODO: PlayerAssetManager에 돈 추가
        }
    }
}
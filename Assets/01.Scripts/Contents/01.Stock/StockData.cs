using System.Collections.Generic;
using System.Numerics;

[System.Serializable]
public class StockData
{
    public string stockName;        // 주식 이름
    public double currentPrice;     // 현재가 (소수점 계산을 위해 double 사용)
    public double purchasePrice;    // 내 평균 매수 단가
    public int ownedCount;          // 보유 수량

    // 가격 히스토리 (최근 50개 데이터 저장용)
    public Queue<double> priceHistory = new Queue<double>();

    // 추세 변수 (양수면 우상향 기세, 음수면 우하향 기세)
    public float momentum = 0.01f;

    public StockData(string name, double startPrice)
    {
        stockName = name;
        currentPrice = startPrice;
        for (int i = 0; i < 50; i++) priceHistory.Enqueue(startPrice);
    }
}
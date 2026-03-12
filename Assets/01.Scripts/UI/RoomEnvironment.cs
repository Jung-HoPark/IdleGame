using UnityEngine;

public class RoomEnvironment : MonoBehaviour
{
    [Header("방 업그레이드 단계별 이미지")]
    public Sprite[] roomLevelSprites;

    [Header("연결할 배경 오브젝트")]
    public SpriteRenderer roomSpriteRenderer;

    public void ChangeRoomLevel(int level)
    {
        // 에러 방지용
        if (level < 0 || level >= roomLevelSprites.Length)
        {
            Debug.LogWarning($"{level}레벨에 해당하는 방 이미지가 없음");
            return;
        }
        
        //여기서 스프라이트 교체
        roomSpriteRenderer.sprite = roomLevelSprites[level];
    }
}
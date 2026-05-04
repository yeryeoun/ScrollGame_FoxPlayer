using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    float itemSpawnInterval = 2f;

    [SerializeField]
    Vector2 itemSpawnYRange = new Vector2(-2f, 2f);

    [SerializeField]
    Vector2 itemSpawnXRange = new Vector2(-2f, 2f);

    [SerializeField]
    Transform itemParent;

    float itemSpawnTimer;

    void Update()
    {
        if (itemPrefab == null)
            return;

        itemSpawnTimer += Time.deltaTime;

        if (itemSpawnTimer < itemSpawnInterval)
            return;

        itemSpawnTimer = 0f;
        SpawnItem();
    }

    void SpawnItem()
    {
        // X와 Y 범위를 랜덤하게 설정하여 생성 위치 계산
        float spawnX = Random.Range(itemSpawnXRange.x, itemSpawnXRange.y);
        float spawnY = Random.Range(itemSpawnYRange.x, itemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        // 아이템 프리팹 생성
        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, itemParent);
    }
}
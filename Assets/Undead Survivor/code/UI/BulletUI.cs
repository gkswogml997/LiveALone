using UnityEngine;

public class BulletUI : MonoBehaviour
{
    RectTransform rectTransform;
    Vector3 initialPosition;
    Vector3 initialVelocity;
    float gravity = -9.8f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        initialVelocity = new Vector3(Random.Range(45f, 90f), 30f, 0f); // 랜덤한 초기 속도 설정
        Invoke("ObjectDestroy", 3f);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 5));

        rectTransform.anchoredPosition += (Vector2)initialVelocity * Time.deltaTime;

        // 중력 적용
        initialVelocity.y += gravity * Time.deltaTime * 10;
    }

    void ObjectDestroy()
    {
        Destroy(gameObject);
    }
}
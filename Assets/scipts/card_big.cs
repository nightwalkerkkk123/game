using UnityEngine;

public class Card_big : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isSelected = false;
    public float scaleFactor = 1.2f;
    public float liftHeight = 0.5f;
    public float rotationAngle = 15f;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (!isSelected)
        {
            // 放大、升高并旋转
            transform.localScale *= scaleFactor;
            transform.position += Vector3.up * liftHeight;
            transform.Rotate(Vector3.up, rotationAngle);
            isSelected = true;
        }
        else
        {
            // 恢复原状
            ResetCard();
        }
    }

    void Update()
    {
        // 检测是否点击了其他物体
        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject)
            {
                ResetCard();
            }
        }
    }

    private void ResetCard()
    {
        transform.localScale = originalScale;
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
        isSelected = false;
    }
}
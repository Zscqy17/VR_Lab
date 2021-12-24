using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 上下屏UI图片同步
/// </summary>
public class ImageSyncronizer : MonoBehaviour
{
    [Header("同步下屏的Image组件")]
    public Image target_image;

    [Header("是否对图片进行SetNativeSize")]
    public bool setNativeSize;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.sprite = target_image.sprite;
        if (setNativeSize)
            image.SetNativeSize();
    }
}
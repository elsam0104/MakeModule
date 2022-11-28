using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(Image))]
public class LoadImageWithAddress : MonoBehaviour
{
    //에디터에서 정의함
    public string address;
    private Image image;
    private AsyncOperationHandle<Sprite> handle;

    private void Start()
    {
        image = GetComponent<Image>();
        handle = Addressables.LoadAssetAsync<Sprite>(address);
        handle.Completed += HandleCompleted;
    }
    private void HandleCompleted(AsyncOperationHandle<Sprite> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            image.sprite = operation.Result as Sprite;
            image.SetNativeSize();
        }
        else
        {
            Debug.LogError($"AssetReference {address} failed to load.");
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] //이미지가 없는 오브젝트에 붙일 경우 방지
public class CustomButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.001f;
    }

}
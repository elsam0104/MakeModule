using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] //�̹����� ���� ������Ʈ�� ���� ��� ����
public class CustomButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.001f;
    }

}
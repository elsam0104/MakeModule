using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrypto : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string test_str;
    private string encryptStr;
    void Start()
    {
        encryptStr = Crypto.AESEncrypt128(test_str);
        Debug.Log("암호화 : "+encryptStr);
        encryptStr = Crypto.AESDecrypt128(encryptStr);
        Debug.Log("복호화 : " + encryptStr);
    }

}

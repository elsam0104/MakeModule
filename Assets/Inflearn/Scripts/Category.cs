using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName ="Category",fileName ="Category_")]
public class Category : ScriptableObject
{
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName; //���ӻ� ������ �̸�

    public string CodeName => codeName;
    public string DisplayName => displayName;

    #region Operator
    public bool Equal(Category other)
    {
        if (other is null) return false;
        if (ReferenceEquals(other, this)) return true;
        if (GetType() != other.GetType()) return false;

        return codeName == other.codeName;
    }

    public override int GetHashCode() => (codeName, displayName).GetHashCode();

    public override bool Equals(object other) => base.Equals(other);
    /// <summary>
    /// category.CodeName���� ���� �ʿ� ���� ������ �������̵����� ó��.
    /// category == "���� ī�װ� �̸�"���� ���
    /// </summary>
    /// <param name="lhs">���� ī�װ�</param>
    /// <param name="rhs">���ϰ��� �ϴ� ī�װ��� �̸� string</param>
    public static bool operator ==(Category lhs,string rhs)
    {
        if (lhs is null) return ReferenceEquals(rhs, null);
        return lhs.CodeName == rhs || lhs.DisplayName == rhs;
    }
    /// <summary>
    /// category.CodeName���� ���� �ʿ� ���� ������ �������̵����� ó��.
    /// category != "���� ī�װ� �̸�"���� ���
    /// </summary>
    /// <param name="lhs">���� ī�װ�</param>
    /// <param name="rhs">���ϰ��� �ϴ� ī�װ��� �̸� string</param>
    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);
    
    #endregion
}

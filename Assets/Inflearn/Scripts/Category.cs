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
    private string displayName; //게임상에 보여줄 이름

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
    /// category.CodeName으로 비교할 필요 없이 연산자 오버라이딩으로 처리.
    /// category == "비교할 카테고리 이름"으로 사용
    /// </summary>
    /// <param name="lhs">현재 카테고리</param>
    /// <param name="rhs">비교하고자 하는 카테고리의 이름 string</param>
    public static bool operator ==(Category lhs,string rhs)
    {
        if (lhs is null) return ReferenceEquals(rhs, null);
        return lhs.CodeName == rhs || lhs.DisplayName == rhs;
    }
    /// <summary>
    /// category.CodeName으로 비교할 필요 없이 연산자 오버라이딩으로 처리.
    /// category != "비교할 카테고리 이름"으로 사용
    /// </summary>
    /// <param name="lhs">현재 카테고리</param>
    /// <param name="rhs">비교하고자 하는 카테고리의 이름 string</param>
    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);
    
    #endregion
}

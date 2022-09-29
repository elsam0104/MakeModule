using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlug : MonoBehaviour
{
    //기능들
    private List<BasePlugAble> plugs;
    //우선권이 필요한 기능 (덮어씌운다)
    private List<BasePlugAble> overridePlugs;

    //현재 기능 코드들
    private int currentPlugs;
    //기본 기능 코드들
    private int defaultPlugs;
    //잠긴 기능 코드들
    private int plugsLocked;

    //캐싱 위한 변수


    //지금 어떤 기본 플러그가 꽃혀 있는가
    public int getDefaultPlugs { get => defaultPlugs; }

    private void Awake()
    {
        //캐싱
    }

    //키 조작 키 전환 상태 체크
    private void Update()
    {
    }
    //플러거블 조정
    private void FixedUpdate()
    {
        bool flagAnyPlugActive = false;

        if (plugsLocked > 0 || overridePlugs.Count == 0)  //잠긴 플러그가 있거나 오버라이딩 된 플러그가 없으면
        {
            foreach (BasePlugAble basePlugAble in plugs)
            {
                if (basePlugAble.isActiveAndEnabled && currentPlugs == basePlugAble.GetPlugCode)
                {
                    flagAnyPlugActive = true;
                    basePlugAble.childFixedUpdate();
                }
            }
        }
        else
        {
            foreach (BasePlugAble basePlugAble in overridePlugs)
            {
                basePlugAble.childFixedUpdate();
            }
        }
    }

    private void LateUpdate()
    {
        if (plugsLocked > 0 || overridePlugs.Count == 0)
        {
            foreach (BasePlugAble basePlugAble in plugs)
            {
                if (basePlugAble.isActiveAndEnabled && currentPlugs == basePlugAble.GetPlugCode)
                {
                    basePlugAble.childLateUpdate();
                }
            }
        }
        else
        {
            foreach (BasePlugAble basePlugAble in overridePlugs)
            {
                basePlugAble.childLateUpdate();
            }
        }
    }
    //플러그 추가
    public void AddPlugs(BasePlugAble basePlugAble)
    {
        plugs.Add(basePlugAble);
    }
    //디폴트,현재 코드에 플러그 추가
    public void regDefaultPlugs(int plugCode)
    {
        defaultPlugs = plugCode;
        currentPlugs = plugCode;
    }

    public void regPlug(int plugCode)
    {
        if (currentPlugs == defaultPlugs)
        {
            currentPlugs = plugCode;
        }
    }

    public void UnRegPlug(int plugCode)
    {
        if (currentPlugs == plugCode)
        {
            currentPlugs = defaultPlugs;
        }
    }
    //plug 우선순위로 만들기
    public bool OverrideWithPlugs(BasePlugAble basePlugAble)
    {
        if (!overridePlugs.Contains(basePlugAble))
        {
            if (overridePlugs.Count == 0)
            {
                foreach (BasePlugAble plugAble in plugs)
                {
                    if (plugAble.isActiveAndEnabled && currentPlugs == plugAble.GetPlugCode)
                    {
                        plugAble.OnOverride();
                        break;
                    }
                }
            }
            overridePlugs.Add(basePlugAble);
            return true;
        }
        return false;
    }
    //우선순위 해제
    public bool UnOverridingPlugs(BasePlugAble plugAble)
    {
        if (overridePlugs.Contains(plugAble))
        {
            overridePlugs.Remove(plugAble);
            return true;
        }
        return false;
    }
    //우선순위 플러그 탐색. 매개변수 없으면 우선순위 플러그가 있는지 확인
    public bool getOverriding(BasePlugAble basePlugAble = null)
    {
        if (basePlugAble == null)
        {
            return overridePlugs.Count > 0;
        }
        return overridePlugs.Contains(basePlugAble);
    }
    //현재 플러그인가?
    public bool getFlagCurrentPlugs(int plugCode)
    {
        return currentPlugs == plugCode;
    }

    public bool getLockStatus(int plugCode = 0)
    {
        return (plugsLocked != 0 && plugsLocked != plugCode);
    }

    public void LockPlugs(int plugCode)
    {
        if (plugsLocked == 0)
        {
            plugsLocked = plugCode;
        }
    }

    public void UnLockPlugs(int plugCode)
    {
        if (plugsLocked == plugCode)
        {
            plugsLocked = 0;
        }
    }
}

//추상화 클래스 만드는 이유
//자식들을 하나의 클래스로 형변환 하여 거기에 플러그 관리를 할 수 있다.
public abstract class BasePlugAble : MonoBehaviour
{
    //속도 구별 (걷고 달리는거 구별)
    protected int spdFloat;
    protected ControllerPlug controllerPlug;
    //해쉬코드로 고유 코드 만들기
    protected int plugsCode;
    //할 수 있는가 여부
    protected bool getFlagRun;

    private void Awake()
    {
        this.controllerPlug = GetComponent<ControllerPlug>();
        getFlagRun = true;
        //해쉬코드로 고유 코드 만들기
        spdFloat = Animator.StringToHash("Speed");
        plugsCode = this.GetType().GetHashCode();
    }

    //플러그 고유 코드 가져오기
    public int GetPlugCode { get => plugsCode; }
    //달릴 수 있는가 여부
    public bool flagAllowRun { get => getFlagRun; }

    //여기 있는 플러그도 업데이트 해줘
    public virtual void childLateUpdate() { }
    public virtual void childFixedUpdate() { }

    //이 기능은 아주 중요해서 먼저 해야함
    public virtual void OnOverride() { }
}

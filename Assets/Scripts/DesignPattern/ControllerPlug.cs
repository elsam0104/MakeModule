using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlug : MonoBehaviour
{
    //��ɵ�
    private List<BasePlugAble> plugs;
    //�켱���� �ʿ��� ��� (������)
    private List<BasePlugAble> overridePlugs;

    //���� ��� �ڵ��
    private int currentPlugs;
    //�⺻ ��� �ڵ��
    private int defaultPlugs;
    //��� ��� �ڵ��
    private int plugsLocked;

    //ĳ�� ���� ����


    //���� � �⺻ �÷��װ� ���� �ִ°�
    public int getDefaultPlugs { get => defaultPlugs; }

    private void Awake()
    {
        //ĳ��
    }

    //Ű ���� Ű ��ȯ ���� üũ
    private void Update()
    {
    }
    //�÷��ź� ����
    private void FixedUpdate()
    {
        bool flagAnyPlugActive = false;

        if (plugsLocked > 0 || overridePlugs.Count == 0)  //��� �÷��װ� �ְų� �������̵� �� �÷��װ� ������
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
    //�÷��� �߰�
    public void AddPlugs(BasePlugAble basePlugAble)
    {
        plugs.Add(basePlugAble);
    }
    //����Ʈ,���� �ڵ忡 �÷��� �߰�
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
    //plug �켱������ �����
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
    //�켱���� ����
    public bool UnOverridingPlugs(BasePlugAble plugAble)
    {
        if (overridePlugs.Contains(plugAble))
        {
            overridePlugs.Remove(plugAble);
            return true;
        }
        return false;
    }
    //�켱���� �÷��� Ž��. �Ű����� ������ �켱���� �÷��װ� �ִ��� Ȯ��
    public bool getOverriding(BasePlugAble basePlugAble = null)
    {
        if (basePlugAble == null)
        {
            return overridePlugs.Count > 0;
        }
        return overridePlugs.Contains(basePlugAble);
    }
    //���� �÷����ΰ�?
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

//�߻�ȭ Ŭ���� ����� ����
//�ڽĵ��� �ϳ��� Ŭ������ ����ȯ �Ͽ� �ű⿡ �÷��� ������ �� �� �ִ�.
public abstract class BasePlugAble : MonoBehaviour
{
    //�ӵ� ���� (�Ȱ� �޸��°� ����)
    protected int spdFloat;
    protected ControllerPlug controllerPlug;
    //�ؽ��ڵ�� ���� �ڵ� �����
    protected int plugsCode;
    //�� �� �ִ°� ����
    protected bool getFlagRun;

    private void Awake()
    {
        this.controllerPlug = GetComponent<ControllerPlug>();
        getFlagRun = true;
        //�ؽ��ڵ�� ���� �ڵ� �����
        spdFloat = Animator.StringToHash("Speed");
        plugsCode = this.GetType().GetHashCode();
    }

    //�÷��� ���� �ڵ� ��������
    public int GetPlugCode { get => plugsCode; }
    //�޸� �� �ִ°� ����
    public bool flagAllowRun { get => getFlagRun; }

    //���� �ִ� �÷��׵� ������Ʈ ����
    public virtual void childLateUpdate() { }
    public virtual void childFixedUpdate() { }

    //�� ����� ���� �߿��ؼ� ���� �ؾ���
    public virtual void OnOverride() { }
}

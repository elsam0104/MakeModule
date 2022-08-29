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
    public Transform playerCamera;
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private Transform playerTransform;

    //horizontal, vertical axis
    private float h;
    private float v;

    //ī�޶� ���ϵ��� ������ �� ȸ���Ǵ� �ӵ�
    public float lerpTurn = 0.05f;
    //�޸��� ����� ī�޶�FOV�� ��ϵǾ���?
    private bool flagChangerFOV;
    //�޸��� �þ߰�
    public float runFOV = 100;
    //���������� ���ߴ� ����
    private Vector3 dirLast;
    //�޸��� ���ΰ�?
    private bool flagRun;
    //�ִϸ��ռ� h,v�� ��
    private int hFloat;
    private int vFloat;
    //�� ���� �پ� �ִ°�?
    private int flagOnGround;
    //���� �浹üũ�� ���� �浹ü ����
    private Vector3 colliderGround;

    public float GetHorizontal { get => h; }
    public float GetVertial { get => v; }
    public Rigidbody GetRigidbody { get => playerRigidbody; }
    public Animator GetAnimator { get => playerAnimator; }

    //���� � �⺻ �÷��װ� ���� �ִ°�
    public int getDefaultPlugs { get => defaultPlugs; }

    private void Awake()
    {
        playerTransform = transform;
        plugs = new List<BasePlugAble>();
        overridePlugs = new List<BasePlugAble>();
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        colliderGround = GetComponent<Collider>().bounds.extents; //���� �ȿ� ���Դ���

        //�ִϸ��̼ǿ� �޷��ִ°Ŷ� �ӽ�
        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        flagOnGround = Animator.StringToHash("Grounded");
    }

    //�÷��̾ �̵����ΰ�?
    //�츮�� �̵��ϴ� �߿� �÷������� ���̱� ����
    public bool getFlagMoving()
    {
        //�۽Ƿδ� ���Ѱ�. 0�� �ٻ�ġ�ν� �Ǽ��� 0���� ū�� Ȯ���� �� ���δ�. 
        //return (h!=0)||(v!=0);
        return Mathf.Abs(h) > Mathf.Epsilon || Mathf.Abs(v) > Mathf.Epsilon;
    }

    public bool getFlagHorizontalMoving()
    {
        return Mathf.Abs(h) > Mathf.Epsilon;
    }

    public bool getFlagRun()
    {
        foreach (BasePlugAble basePlugAble in plugs)
        {
            if (basePlugAble.flagAllowRun)
                return false;
        }
        foreach (BasePlugAble overPlugAble in overridePlugs)
        {
            if (overPlugAble.flagAllowRun)
                return false;
        }
        return true;
    }

    //�޸��°� �����Ѱ�?
    public bool getFlagReadyRunning()
    {
        return flagRun && getFlagMoving() && getFlagRun();
    }
    //Ȥ�� �� ���� �ִ°�? (���׷� ���� �� �ߴµ� �����°� �� ����°� �Ǻ��� ���)
    public bool getFlagGrounded()
    {
        Ray ray = new Ray(playerTransform.position + Vector3.up * 2 * colliderGround.x, Vector3.down);
        return Physics.SphereCast(ray, colliderGround.x, colliderGround.x + 0.2f);
    }
    //Ű ���� Ű ��ȯ ���� üũ
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        playerAnimator.SetFloat(hFloat, h, 0.1f, Time.deltaTime);
        playerAnimator.SetFloat(vFloat, v, 0.1f, Time.deltaTime);
        //���߿�
        flagRun = Input.GetButtonDown("Jump");

        //if (getFlagReadyRunning())
        //{
        //    flagChangerFOV = true;
        //    cameraScript.setFOV(runFOV);
        //}
        //else if (flagChangerFOV)
        //{
        //    cameraScript.restFOV();
        //    flagChangerFOV = false;
        //}

    }
    public void restPosition()
    {
        if (dirLast != Vector3.zero)
        {
            //y���� 0���� ���� �� y����Ͽ� �־��ֱ� ����
            dirLast.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(dirLast);
            Quaternion newRotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, lerpTurn);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
    //�÷��ź� ����
    private void FixedUpdate()
    {
        bool flagAnyPlugActive = false;

        if (plugsLocked > 0 || overridePlugs.Count == 0)
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
        if (!flagAnyPlugActive && overridePlugs.Count == 0)
        {
            playerRigidbody.useGravity = true;
            restPosition();
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
    public Vector3 getDirLast()
    {
        return dirLast;
    }
    public void setDirLast(Vector3 dir)
    {
        dirLast = dir;
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

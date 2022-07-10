using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Sound : MonoBehaviour
{
    //�⺻������ ������ ������ͼ�. �׷����� eff�� bgm�� ������ ���� ��
    //���� ���� �׷��� �ν����Ϳ��� volume ��Ŭ�� -> expose
    //exposed parameters ���� ���� �Ķ���� �̸��� �ٲ��� ��.
    [SerializeField]
    AudioMixer audioMixer = null;
    //���� ������ �����̴�.
    [SerializeField]
    Slider bgmSlider = null;
    [SerializeField]
    Slider effSlider = null;
    [SerializeField]
    Slider masterSlider = null;
    //exposed parameters�� �Ķ���͵��� �̸�.
    string bgm_Group = "BGM";
    string eff_Group = "EFF";
    string master_Group = "MASTER";

    [SerializeField]
    private List<AudioClip> audioClips = new List<AudioClip>();

    private void Awake()
    {
        SetSource();
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        effSlider.onValueChanged.AddListener(SetEffVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    //������ �����ϴ� �Լ�. 
    private void SetBgmVolume(float volume)
    {
        if (volume == 0) //������ 0�̸�  log����� 1�� ó���Ǳ� ������ ũ�� ��� ���� �߻�. ����ó��
        {
            audioMixer.SetFloat(bgm_Group, -80);
            return;
        }
        audioMixer.SetFloat(bgm_Group, Mathf.Log10(volume) * 20);
        //�����̴��� ���� ������������ ����� �ͼ��� �α� �������̱� ������ ��ȯ������ ��ģ��.
    }
    private void SetEffVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat(eff_Group, -80);
            return;
        }
        audioMixer.SetFloat(eff_Group, Mathf.Log10(volume) * 20);
    }
    private void SetMasterVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat(master_Group, -80);
            return;
        }
        audioMixer.SetFloat(master_Group, Mathf.Log10(volume) * 20);
    }
    private void SetSource()
    {
        int i = 0;
        foreach (AudioClip clip in audioClips)
        {
            var obj = new GameObject().AddComponent<AudioSource>();
            obj.name = "Sound "+ i++;
            obj.clip = clip;
            obj.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
            obj.transform.SetParent(this.transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Sound : MonoBehaviour
{
    //기본적으로 조정할 오디오믹서. 그룹으로 eff와 bgm을 나누어 놔야 함
    //또한 각각 그룹의 인스펙터에서 volume 우클릭 -> expose
    //exposed parameters 에서 각각 파라미터 이름을 바꿔줄 것.
    [SerializeField]
    AudioMixer audioMixer = null;
    //볼륨 조절할 슬라이더.
    [SerializeField]
    Slider bgmSlider = null;
    [SerializeField]
    Slider effSlider = null;
    [SerializeField]
    Slider masterSlider = null;
    //exposed parameters의 파라미터들의 이름.
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
    //볼륨을 조절하는 함수. 
    private void SetBgmVolume(float volume)
    {
        if (volume == 0) //볼륨이 0이면  log계산이 1로 처리되기 때문에 크게 들려 오류 발생. 예외처리
        {
            audioMixer.SetFloat(bgm_Group, -80);
            return;
        }
        audioMixer.SetFloat(bgm_Group, Mathf.Log10(volume) * 20);
        //슬라이더는 정수 스케일이지만 오디오 믹서는 로그 스케일이기 때문에 변환과정을 거친다.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SoundType;
public class Sound : MonoBehaviour
{
    //기본적으로 조정할 오디오믹서. 그룹으로 eff와 bgm을 나누어 놔야 함
    //또한 각각 그룹의 인스펙터에서 volume 우클릭 -> expose
    //exposed parameters 에서 각각 파라미터 이름을 바꿔줄 것.
    [SerializeField]
    AudioMixer audioMixer = null;
    //볼륨 조절할 슬라이더.
    [Header("볼륨 조절할 슬라이더")]
    [SerializeField]
    Slider bgmSlider = null;
    [SerializeField]
    Slider effSlider = null;
    [SerializeField]
    Slider masterSlider = null;
    [Header("실행시킬 클립")]
    [SerializeField]
    private List<AudioClip> effAudioClips = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> bgmAudioClips = new List<AudioClip>();
    [Header("오디오 믹서 그룹")]
    [SerializeField]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup effMixerGroup;

    private List<AudioSource> SoundsEff = new List<AudioSource>();
    private List<AudioSource> SoundsBgm = new List<AudioSource>();

    //exposed parameters의 파라미터들의 이름.
    string bgm_Group = "BGM";
    string eff_Group = "EFF";
    string master_Group = "MASTER";

    private AudioSource lastPlayBgm;
    private void Awake()
    {
        SetSource();
        SetAddListener();
    }
    private void Update()
    {
        //사용 예시
        if (Input.GetKeyDown(KeyCode.E))
            PlayEff(SoundType.EffType.Example);
        if (Input.GetKeyDown(KeyCode.R))
            PlayBgm(SoundType.BgmType.Example);
    }
    private void SetAddListener()
    {
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
        int i = 1;
        foreach (AudioClip clip in effAudioClips)
        {
            var obj = new GameObject().AddComponent<AudioSource>();
            obj.name = "Eff " + i;
            obj.playOnAwake = false;
            obj.outputAudioMixerGroup = effMixerGroup;
            obj.clip = clip;
            SoundsEff.Add(obj);
            obj.transform.SetParent(this.transform);
            i++;
        }
        i = 1;
        foreach (AudioClip clip in bgmAudioClips)
        {
            var obj = new GameObject().AddComponent<AudioSource>();
            obj.name = "Bgm " + i;
            obj.playOnAwake = false;
            obj.clip = clip;
            obj.loop = true;
            obj.outputAudioMixerGroup = bgmMixerGroup;

            SoundsBgm.Add(obj);
            obj.transform.SetParent(this.transform);
            i++;
        }

    }
    public void PlayBgm(SoundType.BgmType value)
    {
        Debug.Log("play Bgm");
        lastPlayBgm?.Stop();
        SoundsBgm[(int)value].Play();
        lastPlayBgm = SoundsBgm[(int)value];
    }
    public void PlayEff(SoundType.EffType value)
    {
        Debug.Log("play eff");
        SoundsEff[(int)value].Play();
    }
}

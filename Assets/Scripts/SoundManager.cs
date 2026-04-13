using System;
using UnityEngine;

/// <summary>
/// 声音管理器：控制游戏内的背景音乐(BGM)与各类音效(SFX)
/// </summary>
public class SoundManager : MonoBehaviour
{
    // 单例模式
    public static SoundManager Instance;

    private DataManager _dataManager;

    // 音频源组件
    private AudioSource _bgmSource; // 专门用于播放 BGM
    private AudioSource _sfxSource; // 专门用于播放音效

    [Header("1. 音频剪辑配置")]
    // BGM 配置
    public AudioClip defaultBGM;       // 默认背景音乐
    public AudioClip vipBGM;           // 充值解锁后的背景音乐 (topUpT >= 1 时播放)

    // 音效配置
    public AudioClip winSFX;           // 胜利音效
    public AudioClip loseSFX;          // 失败音效
    public AudioClip paySFX;           // 氪金/充值音效
    public AudioClip popupSFX;         // 弹窗音效
    public AudioClip buttonClickSFX;   // 按钮点击音效

    [Header("2. 参数调节")]
    public float bgmVolume = 0.5f;     // BGM 音量
    public float sfxVolume = 1.0f;     // 音效音量

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 通常声音管理器需要跨场景存在
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _dataManager = DataManager.Instance;
        
        // 初始化音频源
        InitializeAudioSources();
    }

    /// <summary>
    /// 初始化音频组件
    /// </summary>
    private void InitializeAudioSources()
    {
        // 初始化 BGM Source
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true; // BGM 通常循环播放
        _bgmSource.playOnAwake = false;
        _bgmSource.volume = bgmVolume;

        // 初始化 SFX Source
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false; // 音效通常不循环
        _sfxSource.playOnAwake = false;
        _sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        // 游戏开始时播放默认 BGM
        PlayDefaultBGM();
    }

    private void Update()
    {
        if (_dataManager == null) return;

        // 检查充值状态以切换 BGM
        // 逻辑：如果 topUpT >= 1 且当前 BGM 不是 VIP BGM，则切换
        if (_dataManager.topUpT >= 1)
        {
            if (vipBGM != null && _bgmSource.clip != vipBGM)
            {
                PlayVipBGM();
            }
        }
        else
        {
            // 如果 topUpT < 1 且当前不是默认 BGM，则切回默认
            if (defaultBGM != null && _bgmSource.clip != defaultBGM)
            {
                PlayDefaultBGM();
            }
        }
    }

    // =================================================================================
    // 公共播放方法 (供其他脚本调用)
    // =================================================================================

    /// <summary>
    /// 播放默认背景音乐
    /// </summary>
    public void PlayDefaultBGM()
    {
        if (defaultBGM == null) return;
        
        _bgmSource.clip = defaultBGM;
        _bgmSource.Play();
    }

    /// <summary>
    /// 播放充值后的背景音乐
    /// </summary>
    public void PlayVipBGM()
    {
        if (vipBGM == null) return;
        
        _bgmSource.clip = vipBGM;
        _bgmSource.Play();
    }

    /// <summary>
    /// 播放胜利音效
    /// </summary>
    public void PlayWinSound()
    {
        if (winSFX != null) _sfxSource.PlayOneShot(winSFX);
    }

    /// <summary>
    /// 播放失败音效
    /// </summary>
    public void PlayLoseSound()
    {
        if (loseSFX != null) _sfxSource.PlayOneShot(loseSFX);
    }

    /// <summary>
    /// 播放氪金/充值音效
    /// </summary>
    public void PlayPaySound()
    {
        if (paySFX != null) _sfxSource.PlayOneShot(paySFX);
    }

    /// <summary>
    /// 播放弹窗音效
    /// </summary>
    public void PlayPopupSound()
    {
        if (popupSFX != null) _sfxSource.PlayOneShot(popupSFX);
    }

    /// <summary>
    /// 播放按钮点击音效
    /// </summary>
    public void PlayButtonClickSound()
    {
        if (buttonClickSFX != null) _sfxSource.PlayOneShot(buttonClickSFX);
    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用场景管理命名空间

/// <summary>
/// 声音管理器：控制游戏内的背景音乐(BGM)与各类音效(SFX)
/// 特性：场景切换时自动停止失败音效
/// </summary>
public class SoundManager : MonoBehaviour
{
    // 单例模式
    public static SoundManager Instance;

    private DataManager _dataManager;

    // --- 音频源组件 (分离管理) ---
    private AudioSource _bgmSource;      // BGM 专用
    private AudioSource _sfxSource;      // 普通音效专用 (点击、弹窗、氪金)
    private AudioSource _loseSource;     // 失败音效专用 (用于单独控制停止)

    [Header("1. 音频剪辑配置")]
    // BGM 配置
    public AudioClip defaultBGM;       // 默认背景音乐
    public AudioClip vipBGM;           // 充值解锁后的背景音乐

    // 音效配置
    public AudioClip winSFX;           // 胜利音效
    public AudioClip loseSFX;          // 失败音效
    public AudioClip paySFX;           // 氪金/充值音效
    public AudioClip popupSFX;         // 弹窗音效
    public AudioClip buttonClickSFX;   // 按钮点击音效

    [Header("2. 参数调节")]
    public float bgmVolume = 0.5f;
    public float sfxVolume = 1.0f;

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保切换场景时声音管理器不销毁
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _dataManager = DataManager.Instance;
        
        // 初始化音频源
        InitializeAudioSources();

        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 移除事件监听，防止内存泄漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 初始化音频组件
    /// </summary>
    private void InitializeAudioSources()
    {
        // 1. BGM Source
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;
        _bgmSource.volume = bgmVolume;

        // 2. 普通音效 Source
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;
        _sfxSource.volume = sfxVolume;

        // 3. 失败音效 Source (独立出来，方便单独停止)
        _loseSource = gameObject.AddComponent<AudioSource>();
        _loseSource.loop = false;
        _loseSource.playOnAwake = false;
        _loseSource.volume = sfxVolume;
    }

    /// <summary>
    /// 场景加载完成后的回调
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 核心逻辑：每次切换场景，强制停止失败音效
        if (_loseSource != null && _loseSource.isPlaying)
        {
            _loseSource.Stop();
        }
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
        if (_dataManager.topUpT >= 1)
        {
            if (vipBGM != null && _bgmSource.clip != vipBGM)
            {
                PlayVipBGM();
            }
        }
        else
        {
            if (defaultBGM != null && _bgmSource.clip != defaultBGM)
            {
                PlayDefaultBGM();
            }
        }
    }

    // =================================================================================
    // 公共播放方法
    // =================================================================================

    public void PlayDefaultBGM()
    {
        if (defaultBGM == null) return;
        _bgmSource.clip = defaultBGM;
        _bgmSource.Play();
    }

    public void PlayVipBGM()
    {
        if (vipBGM == null) return;
        _bgmSource.clip = vipBGM;
        _bgmSource.Play();
    }

    public void PlayWinSound()
    {
        if (winSFX != null) _sfxSource.PlayOneShot(winSFX);
    }

    /// <summary>
    /// 播放失败音效
    /// 注意：此音效会在场景切换时自动停止
    /// </summary>
    public void PlayLoseSound()
    {
        if (loseSFX != null)
        {
            // 如果已经在播放，先停止再播放（可选，防止重叠）
            if (_loseSource.isPlaying) _loseSource.Stop();
            
            _loseSource.PlayOneShot(loseSFX);
        }
    }

    public void PlayPaySound()
    {
        if (paySFX != null) _sfxSource.PlayOneShot(paySFX);
    }

    public void PlayPopupSound()
    {
        if (popupSFX != null) _sfxSource.PlayOneShot(popupSFX);
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSFX != null) _sfxSource.PlayOneShot(buttonClickSFX);
    }
}
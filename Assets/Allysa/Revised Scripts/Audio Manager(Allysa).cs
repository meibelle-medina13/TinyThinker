using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Audio_Manager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> backgroundMusic;
    public List<AudioClip> InstructionAudio;
    [SerializeField] private List<PlayableDirector> playableDirector;
    [SerializeField] private List<AudioClip> SoundEffects;
    [SerializeField] private List<AudioClip> LetterSounds;
    private AudioSource audioSource;
    [HideInInspector] public AudioSource audioSourceBG1;
    private AudioSource audioSourceBG2;

    private Quarter1_Level3 Q1_3;
    private Quarter1_Level4 Q1_4;
    private Quarter2_Level4 Q2_4;



    void Start()
    {
        Q1_3 = FindObjectOfType<Quarter1_Level3>();
        Q1_4 = FindObjectOfType<Quarter1_Level4>();
        Q2_4 = FindObjectOfType<Quarter2_Level4>();
    }
    public void scene_bgmusic(float bg_volume)
    {
        audioSourceBG1 = gameObject.AddComponent<AudioSource>();
        audioSourceBG1.clip = backgroundMusic[0];

        if (audioSourceBG1.clip != null)
        {
            audioSourceBG1.Play();
            audioSourceBG1.volume = bg_volume;
            audioSourceBG1.loop = true;
            audioSourceBG1.playOnAwake = false;
        }
    }

    public void assessment_bgmusic(float bg_volume)
    {
        if (audioSourceBG1.isPlaying)
        {
            audioSourceBG1.Stop();
        }

        audioSourceBG2 = gameObject.AddComponent<AudioSource>();
        audioSourceBG2.clip = backgroundMusic[1];

        if (audioSourceBG2.clip != null)
        {
            audioSourceBG2.Play();
            audioSourceBG2.volume = bg_volume;
            audioSourceBG2.loop = true;
            audioSourceBG2.playOnAwake = false;
        }
    }

    public void Stop_backgroundMusic2()
    {
        if (audioSourceBG2.isPlaying)
        {
            audioSourceBG2.Stop();
        }
    }

    public void Click()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundEffects[0];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.volume = 0.45f;
        }
    }

    public void Correct()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundEffects[1];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.volume = 0.45f;
        }
    }

    public void Wrong()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundEffects[2];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.volume = 0.45f;
        }
    }

    public void Rotate()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundEffects[3];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.volume = 0.45f;
        }
    }

    public void Repeat_Instruction_NoTimeline(int index)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = InstructionAudio[index];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.loop = false;
        }
    }
    public void Repeat_Instruction(int index)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = InstructionAudio[index];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.loop = false;
        }

        if (index > 0)
        {
            playableDirector[index - 1].time = 0;
            playableDirector[index - 1].Play();
        }

        audioSource.clip = InstructionAudio[index];
        StartCoroutine(DisableButtonsWhileAudioPlays(audioSource));
    }

    private IEnumerator DisableButtonsWhileAudioPlays(AudioSource instruction_audio)
    {
        if (Q1_3 != null)
        {
            foreach (Button button in Q1_3.clickableButtons)
            { button.interactable = false; }
        }

        if (Q2_4 != null)
        {
            foreach (Button button in Q2_4.clickablebuttons)
            { button.interactable = false; }
        }

        while (instruction_audio.isPlaying)
        { yield return null; }

        if (Q1_3 != null)
        {
            foreach (Button button in Q1_3.clickableButtons)
            { button.interactable = true; }
        }

        if (Q2_4 != null)
        {
            foreach (Button button in Q2_4.clickablebuttons)
            { button.interactable = true; }
        }
    }

    public void Repeat_LetterSounds(int index)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = LetterSounds[index];

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void Repeat_Timeline(int index)
    {
        playableDirector[index].time = 0;
        playableDirector[index].Play();
    }

    public void SoundEffect(int index)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundEffects[index];

        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.volume = 1f;
        }
    }
}

using SettingsNS;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundNS
{
    public class SoundManager : MonoBehaviour
    {
        private const string SOUND_NAME_UI_BUTTON_CLICK = "UIButtonClick";
        private const string SOUND_NAME_CUBE_PICK_UP = "CubePickUp";
        private const string SOUND_NAME_COLLISION_WITH_WALL = "CollisionWithWall";

        private static SoundManager instance;
        public static SoundManager Instance { get => instance; }

        [Serializable]
        private class Sound
        {
            public string Name;
            public AudioSource AudioSourceReference;
        }

        [SerializeField]
        private AudioMixerSnapshot regularSnapshot;
        [SerializeField]
        private AudioMixerSnapshot musicMuteSnapshot;
        [SerializeField]
        private Sound[] sounds;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        private void Start() =>
            SetMusicMuteStatus();
        private void OnEnable()
        {
            Settings.IsMusicMuted.OnToggleEvent += SetMusicMuteStatus;
        }
        private void OnDisable()
        {
            Settings.IsMusicMuted.OnToggleEvent -= SetMusicMuteStatus;
        }
        public void PlayUIButtonClick()
        {
            Sound s = FindSound(SOUND_NAME_UI_BUTTON_CLICK);
            PlaySoundOneShot(s);
        }
        public void PlayCubePickUp()
        {
            Sound s = FindSound(SOUND_NAME_CUBE_PICK_UP);
            PlaySoundOneShotMultipleTimes(s);
        }
        public void PlayCollisionWithWall()
        {
            Sound s = FindSound(SOUND_NAME_COLLISION_WITH_WALL);
            PlaySoundOneShot(s);
        }
        private void SetMusicMuteStatus()
        {
            if (Settings.IsMusicMuted.BoolState)
                musicMuteSnapshot.TransitionTo(0);
            else
                regularSnapshot.TransitionTo(0);
        }
        private Sound FindSound(string soundName)
        {
            Sound sound = sounds.FirstOrDefault(x => x.Name == soundName);
            if (sound == null)
            {
#if UNITY_EDITOR
                Debug.Log("There is no such sound!");
#endif
            }
            return sound;
        }
        private void PlaySoundOneShot(Sound sound)
        {
            if (!sound.AudioSourceReference.isPlaying || sound.AudioSourceReference.time >= sound.AudioSourceReference.clip.length / 2f)
                sound.AudioSourceReference.PlayOneShot(sound.AudioSourceReference.clip);
        }
        private void PlaySoundOneShotMultipleTimes(Sound sound) =>
            sound.AudioSourceReference.PlayOneShot(sound.AudioSourceReference.clip);
    }
}
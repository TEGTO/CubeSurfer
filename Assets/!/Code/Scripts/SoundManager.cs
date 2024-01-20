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
        private const string MUTE_MUSIC_KEY = "MuteMusicStatus";

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

        private bool isMusicMuted = false;

        public bool IsMusicMuted { get => isMusicMuted; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
            GetMusicMuteStatus();
        }
        private void Start()
        {
            SetMusicMuteStatus(isMusicMuted);
        }
        private void OnDestroy()
        {
            SaveMusicMuteStatus();
        }
        public void ToggleMusicState() =>
            SetMusicMuteStatus(!isMusicMuted);

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
        private void SetMusicMuteStatus(bool musicMuteStatus)
        {
            isMusicMuted = musicMuteStatus;
            if (musicMuteStatus)
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
            if (!sound.AudioSourceReference.isPlaying)
                sound.AudioSourceReference.PlayOneShot(sound.AudioSourceReference.clip);
        }
        private void PlaySoundOneShotMultipleTimes(Sound sound)
        {
            sound.AudioSourceReference.PlayOneShot(sound.AudioSourceReference.clip);
        }
        private void SaveMusicMuteStatus() =>
              PlayerPrefs.SetInt(MUTE_MUSIC_KEY, isMusicMuted ? 1 : 0);
        private void GetMusicMuteStatus() =>
            isMusicMuted = PlayerPrefs.GetInt(MUTE_MUSIC_KEY, 1) == 1;
    }
}
using System;
using UnityEngine;
namespace SettingsNS
{
    public class ToggleBool
    {
        public Action OnToggleEvent;
        private bool boolState;
        private Action<string, bool> saveMethod;
        private string key;

        public ToggleBool(bool boolState, Action<string, bool> saveMethod, string key)
        {
            this.boolState = boolState;
            this.saveMethod = saveMethod;
            this.key = key;
        }
        public bool BoolState
        {
            get => boolState;
            set
            {
                boolState = value;
                saveMethod(key, boolState);
            }
        }
        public void ToggleValue()
        {
            BoolState = !BoolState;
            OnToggleEvent?.Invoke();
        }
    }
    public class Settings : MonoBehaviour
    {
        private const string MUTE_VIBRATION_KEY = "MuteVibrationStatus";
        private const string MUTE_MUSIC_KEY = "MuteMusicStatus";

        public static ToggleBool IsVibrationMuted = new ToggleBool(LoadBoolValue(MUTE_VIBRATION_KEY), SaveBoolVlue, MUTE_VIBRATION_KEY);
        public static ToggleBool IsMusicMuted = new ToggleBool(LoadBoolValue(MUTE_MUSIC_KEY), SaveBoolVlue, MUTE_MUSIC_KEY);

        private static bool LoadBoolValue(string key) =>
            PlayerPrefs.GetInt(key, 1) == 1;
        private static void SaveBoolVlue(string key, bool value) =>
          PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}

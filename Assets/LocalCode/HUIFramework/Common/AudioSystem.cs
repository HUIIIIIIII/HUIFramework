using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HUIFramework.Common
{
    public class AudioSystem : SingletonMonoBase<AudioSystem>
    {
        private SimpleObjectPool<AudioSource> audio_source_pool;
        private List<AudioSource> music_list = new();
        private List<AudioSource> sound_list = new();
        private Dictionary<AudioSource, long> audio_source_task = new Dictionary<AudioSource, long>();

        private bool enable_sound;
        private bool enable_music;

        public bool EnableSound
        {
            get => enable_sound;
            set
            {
                if (!value)
                {
                    StopAllSound();
                }

                enable_sound = value;
            }
        }

        public bool EnableMusic
        {
            get => enable_music;
            set
            {
                if (!value)
                {
                    StopAllMusic();
                }

                enable_music = value;
            }
        }

        
        public override UniTask InitAsync()
        {
            var audio_source_temp = new GameObject("empty_audio_source").AddComponent<AudioSource>();
            audio_source_temp.transform.SetParent(this.transform);
            audio_source_temp.playOnAwake = false;
            audio_source_pool = new SimpleObjectPool<AudioSource>(() =>
            {
                return Instantiate(audio_source_temp, this.transform);
            });
            enable_sound = true;
            enable_music = true;
            return UniTask.CompletedTask;
        }


        private AudioSource PlayAudio(AudioClip audio_clip, bool isLoop = false)
        {
            if (audio_clip == null) return null;

            var audioSource = audio_source_pool.Get();
            audioSource.clip = audio_clip;
            audioSource.loop = isLoop;
            audioSource.volume = 1;
            audioSource.pitch = 1;
            audioSource.Play();
            if (!isLoop)
            {
                var task_id = TimerSystem.Instance.ScheduleOnce(audio_clip.length, () => { StopAudio(audioSource); });
                audio_source_task.Add(audioSource, task_id);
            }

            return audioSource;
        }

        public AudioSource PlayMusic(AudioClip audio_clip, bool isLoop = false)
        {
            if (!enable_music) return null;
            return PlayAudio(audio_clip, isLoop);
        }

        public AudioSource PlaySound(AudioClip audio_clip, bool isLoop = false)
        {
            if (!enable_sound) return null;
            return PlayAudio(audio_clip, isLoop);
        }

        private void PlayAudioWithTemp(AudioSource audio_source, AudioClip audio_clip = null)
        {
            if (audio_clip != null)
            {
                audio_source.clip = audio_clip;
            }

            audio_source.Play();
        }

        public void PlayMusicWithTemp(AudioSource audioSource, AudioClip audioClip = null)
        {
            if (!enable_music) return;
            PlayAudioWithTemp(audioSource, audioClip);
        }

        public void PlaySoundWithTemp(AudioSource audioSource, AudioClip audioClip = null)
        {
            if (!enable_sound) return;
            PlayAudioWithTemp(audioSource, audioClip);
        }

        public void StopAudio(AudioSource audioSource)
        {
            if (audio_source_task.TryGetValue(audioSource, out var task_id))
            {
                TimerSystem.Instance.Unschedule(task_id);
            }

            audioSource.Stop();
            audioSource.clip = null;
            audio_source_pool.Release(audioSource);
        }

        public void StopAllMusic()
        {
            foreach (var music_item in music_list)
            {
                StopAudio(music_item);
            }

            music_list.Clear();
        }

        public void StopAllSound()
        {
            foreach (var sound_item in sound_list)
            {
                StopAudio(sound_item);
            }

            sound_list.Clear();
        }

        public void StopAllAudio()
        {
            StopAllMusic();
            StopAllSound();
        }
    }
}
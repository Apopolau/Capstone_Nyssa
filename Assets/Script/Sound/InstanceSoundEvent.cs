using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SoundEffect", fileName = "SFX_")]
public class InstanceSoundEvent : SoundEvent
{
    #region config

    public AudioClip[] clips;
    public Vector2 volume = new Vector2(0.5f, 0.5f);
    public Vector2 pitch = new Vector2(1, 1);

    #endregion

    public AudioSource Play(AudioSource audioSourceParam = null)
    {
        if (clips.Length == 0)
        {
            return null;
        }

        AudioSource source = audioSourceParam;
        if (source == null)
        {
            GameObject go = new GameObject(name: "Sound", components: typeof(AudioSource));
            source = go.AddComponent<AudioSource>();
        }

        source.clip = clips[0];
        source.volume = Random.Range(volume.x, volume.y);
        source.pitch = Random.Range(pitch.x, pitch.y);

        source.Play();

        Destroy(source.gameObject, t: source.clip.length / source.pitch);

        return source;
    }
}

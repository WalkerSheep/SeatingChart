using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClip music;
    public void Start()
    {
        if(music)
        {
            AudioManager.instance.PlayMusic(music);
        }
    }
}

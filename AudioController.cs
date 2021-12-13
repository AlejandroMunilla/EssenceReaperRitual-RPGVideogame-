using UnityEngine;

public class AudioController : MonoBehaviour {

	public float volume = 0.25f;
	public string [] tracks;
    public AudioClip battleAudio;
    public AudioClip normalAudio;
    public AudioClip[] serie;
    public bool playSerie = false;
    private float initialVolume = 0.25f;
	private AudioSource audio;
    private AudioSource audioSecond;
    private AudioClip initialClip;
    private int timeBetweenClips;
 
	private float time;

	// Use this for initialization
	void Start () 
	{
		audio = GetComponent<AudioSource>();
		audio.volume = volume;
        if (transform.Find ("Audio") != null)
        {
            audioSecond = transform.Find("Audio").gameObject.GetComponent<AudioSource>();
        }
        battleAudio  = (Instantiate(Resources.Load("Audio/EpicCombat")) as AudioClip);
        if (normalAudio == null)
        {
            normalAudio = (Instantiate(Resources.Load("Audio/SolemnCity")) as AudioClip);
        }
       
        
        if (initialClip != null)
        {
            normalAudio = initialClip;
        }

        audio.clip = normalAudio;
        audio.Play();
        if (playSerie == true)
        {
            PlaySerie();
        }
    }

	public void ChangeToBattle()
	{
        initialClip = audio.clip;
        time = audio.time;
		audio.Pause();
        audio.clip = battleAudio;
        audio.Play();
	}

	public void ChangeToPeace ()
	{
        audio.Pause();
        audio.volume = initialVolume;
        audio.clip = normalAudio;
        audio.time = time;
        audio.Play();
    }

    public void ChangeToOther (string other, float vol)
    {
        initialClip = audio.clip;
        time = audio.time;
        audio.Pause();
     //   Debug.Log(other);
        audio.clip = (Instantiate(Resources.Load("Audio/" + other)) as AudioClip);
        audio.Play();
        audio.volume = vol;
    }

    public void PlaySerie ()
    {
        int lengthSerie = serie.Length;
        int choose = Random.Range(0, lengthSerie);
        for (int cnt = 0; cnt < serie.Length; cnt++)
        {
            if (cnt == choose)
            {
                audio.clip = serie[cnt];
            }
        }
        float lengthClip = audio.clip.length;
        int extraTime = Random.Range(8, 25);
        timeBetweenClips = (int)(lengthClip) + extraTime;
    //    audio.Play();
        Debug.Log(timeBetweenClips);
            
        Invoke("SerieAudio", timeBetweenClips);

    }

    private void SerieAudio ()
    {
        int lengthSerie = serie.Length;
        int choose = Random.Range(0, lengthSerie);
        for (int cnt = 0; cnt < serie.Length; cnt++)
        {
            if (cnt == choose)
            {
                audioSecond.clip = serie[cnt];
            }
        }
        float lengthClip = audioSecond.clip.length;
        int extraTime = Random.Range(3, 10);
        timeBetweenClips = (int)(lengthClip) + extraTime;
        audioSecond.Play();
        Invoke("SerieAudio", timeBetweenClips);
        
    }

    private void ChangeBackToNormal ()
    {
        audio.Pause();
        audio.clip = initialClip;
        audio.Play();
        audio.volume = initialVolume;
    }
}

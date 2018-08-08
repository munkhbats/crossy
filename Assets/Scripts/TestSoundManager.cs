using UnityEngine;

public class TestSoundManager : Singleton<TestSoundManager> {
	public AudioClip CitySound;
	public AudioClip JumpSound;
	public AudioClip DeathSound;
	public AudioClip DeathCarSound;
	public AudioClip DeathRiverSound;
	public AudioClip DeathFallSound;
	public AudioClip RiverSound;
	public AudioClip StarSound;
	public AudioClip CarSound;
	public bool deathStarPlay;
	public bool deathRiverPlay;
	public bool deathCarPlay;
	public bool deathFallPlay;
	
	private AudioSource m_AudioSource;
	
	protected override void Init() {
		m_AudioSource = this.GetComponent<AudioSource>();
	}
	
	public void PlayCitySound() {
		m_AudioSource.PlayOneShot(CitySound);
	}
	
	public void PlayJumpSound() {
		m_AudioSource.PlayOneShot(JumpSound);
	}
	
	public void PlayDeathSound() {
		if(deathStarPlay == false){
			m_AudioSource.PlayOneShot(DeathSound);
			deathStarPlay = true;
		}
	}
	
	public void PlayDeathCarSound() {
		if(deathCarPlay == false){
			m_AudioSource.PlayOneShot(DeathCarSound);
			deathCarPlay = true;
		}
	}
	
	
	public void PlayDeathRiverSound() {
		if(deathStarPlay == false){
			m_AudioSource.PlayOneShot(DeathRiverSound);
			deathStarPlay = true;
		}
	}
	
	public void PlayDeathFallSound() {
		if(deathCarPlay == false){
			m_AudioSource.PlayOneShot(DeathFallSound);
			deathFallPlay = true;
		}
	}
	
	public void PlayCarSound() {
		m_AudioSource.PlayOneShot(CarSound);
	}
	
	public void PlayRiverSound() {
		m_AudioSource.PlayOneShot(RiverSound);
	}
	
	public void PlayStarSound() {
		m_AudioSource.PlayOneShot(StarSound);
	}
	
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    private ParticleSystem shieldParticle;
    private ParticleSystemRenderer systemRenderer;
    private Material shieldMaterial;
    [SerializeField] AnimationCurve powerUpCurve;
  
    [SerializeField] AnimationCurve standartCurve;
    [SerializeField] float lerpValue;
    [SerializeField] float shieldDuration;
    [SerializeField] float maxSpeed;
    [SerializeField] Color damageColor;
 
    void Start()
    {
        shieldParticle = GetComponent<ParticleSystem>();
        systemRenderer = GetComponent<ParticleSystemRenderer>();
        shieldMaterial = systemRenderer.material;

        StartCoroutine(ChangeParticleSpeed());
      StartCoroutine(ColorLerp());
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }
    public IEnumerator ChangeParticleSpeed()
    {
        var solt = shieldParticle.sizeOverLifetime;
        var main = shieldParticle.main;
        solt.size = new ParticleSystem.MinMaxCurve(1.5f, powerUpCurve);
        main.simulationSpeed = 6;
        //WaitForSeconds startDelay = new WaitForSeconds(.05f);
        //yield return startDelay;
        while (true)
        {


            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, maxSpeed, lerpValue);
            if (main.simulationSpeed <= maxSpeed + 0.006f)
                break;
            yield return null;
        }

    }
    public IEnumerator ColorLerp()
    {
        while (true)
        {
            systemRenderer.material.color = Color.Lerp(shieldMaterial.color, damageColor,Time.deltaTime/shieldDuration);
            yield return null;
        }
    }
     


}

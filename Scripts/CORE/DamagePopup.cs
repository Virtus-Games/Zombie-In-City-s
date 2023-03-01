using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, float damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private void Awake() 
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        transform.rotation = Camera.main.transform.rotation;
    }
    public void Setup(float damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 0.2f;
    }

    private void Update() {
        float moveSpeed = 12f;
        transform.position += new Vector3(0,moveSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0){
            // Start Disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor; 
            if (textColor.a < 0){
                Destroy(gameObject);
            }
        }
    }
}

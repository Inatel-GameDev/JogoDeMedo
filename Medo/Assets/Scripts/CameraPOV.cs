using UnityEngine;
using UnityEngine.UI;

public class CameraPOV : MonoBehaviour
{
    public float sesnsibilidadeX;
    public float sesnsibilidadeY;

    public Transform orientation;
    public float rotationX;
    public float rotationY;

    [Header("Configurações do Tremor")]
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;
        
    [Header("Configurações da Vignette")]
    public Image redVignette;
    public float vignetteIntensity = 0.7f;
    public float vignetteFadeSpeed = 2f;

    private Vector3 originalPosition;
    private bool isShaking = false;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalPosition = transform.localPosition;
        redVignette.color = new Color(1, 0, 0, 0); // Inicia transparente
        
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sesnsibilidadeX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sesnsibilidadeY;
        
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f,90f);

        transform.rotation = Quaternion.Euler(rotationX,rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);

    }

    public void TriggerDamageEffect()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCamera());
            StartCoroutine(ShowVignette());
        }
    }

    private System.Collections.IEnumerator ShakeCamera()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeIntensity;
            transform.localPosition = originalPosition + randomOffset;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPosition;
        isShaking = false;
    }

    private System.Collections.IEnumerator ShowVignette()
    {
        // Fade in
        float alpha = 0f;
        while (alpha < vignetteIntensity)
        {
            alpha += Time.deltaTime * vignetteFadeSpeed;
            redVignette.color = new Color(1, 0, 0, alpha);
            yield return null;
        }

        // Fade out
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * vignetteFadeSpeed;
            redVignette.color = new Color(1, 0, 0, alpha);
            yield return null;
        }
    }
}

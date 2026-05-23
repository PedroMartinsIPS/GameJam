using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishDoor : MonoBehaviour
{
    [Header("Configurações")]
    public string nextSceneName = "Nivel2"; 
    public Sprite doorOpenSprite; 

    private SpriteRenderer spriteRenderer;
    private bool isLevelFinished = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLevelFinished)
        {
            StartCoroutine(FinishLevelSequence(other));
        }
    }

    IEnumerator FinishLevelSequence(Collider2D playerCollider)
    {
        isLevelFinished = true;

        if (doorOpenSprite != null)
        {
            spriteRenderer.sprite = doorOpenSprite;
        }

        PlayerController scriptJogador = playerCollider.GetComponent<PlayerController>();
        Rigidbody2D rbJogador = playerCollider.GetComponent<Rigidbody2D>();
        
        if (scriptJogador != null) scriptJogador.enabled = false;
        if (rbJogador != null) rbJogador.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene(nextSceneName);
    }
}
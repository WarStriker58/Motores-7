using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    [SerializeField] float speed = 15;
    public string FirstScene;
    public string SecondScene;
    public GameObject interactMessageNPC1; // Referencia al mensaje de interacción con NPC1 en el Canvas
    public GameObject interactMessageNPC2; // Referencia al mensaje de interacción con NPC2 en el Canvas
    public AudioClip footstepSound; // Sonido de los pasos del jugador
    public AudioClip doorSound; // Sonido de abrir y cerrar la puerta del primer cuarto
    public AudioClip firstRoomMusic; // Música del primer cuarto
    public AudioClip secondRoomMusic; // Música del segundo cuarto
    private AudioSource audioSource; // Componente AudioSource para reproducir sonidos
    private bool interacting = false;
    private bool inRoom = false;
    private bool inSecondRoom = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        MovePlayer();

        // Verificar si el jugador ha interactuado (presionado la tecla "E")
        if (Keyboard.current.eKey.wasPressedThisFrame && !interacting)
        {
            InteractWithNPC();
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;

        // Reproducir sonido de pasos si el jugador se está moviendo
        if (direction.magnitude > 0.1f && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Go"))
        {
            SceneManager.LoadScene(SecondScene);
        }
        else if (collision.gameObject.CompareTag("Return"))
        {
            SceneManager.LoadScene(FirstScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC1"))
        {
            interactMessageNPC1.SetActive(true);
        }
        else if (other.CompareTag("NPC2"))
        {
            interactMessageNPC2.SetActive(true);
        }
        else if (other.CompareTag("FirstRoom"))
        {
            EnterRoom();
        }
        else if (other.CompareTag("SecondRoom"))
        {
            EnterSecondRoom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC1"))
        {
            interactMessageNPC1.SetActive(false);
        }
        else if (other.CompareTag("NPC2"))
        {
            interactMessageNPC2.SetActive(false);
        }
        else if (other.CompareTag("Room"))
        {
            ExitRoom();
        }
        else if (other.CompareTag("SecondRoom"))
        {
            ExitSecondRoom();
        }
    }

    void InteractWithNPC()
    {
        interacting = true;
        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        // Esperar unos segundos antes de ocultar el mensaje
        yield return new WaitForSeconds(3.0f);
        // Ocultar el cuadro de mensaje de interacción
        interactMessageNPC1.SetActive(false);
        interactMessageNPC2.SetActive(false);
        interacting = false;
    }

    void EnterRoom()
    {
        if (!inRoom) // Verificar si el jugador no está ya en la habitación
        {
            inRoom = true;
            audioSource.PlayOneShot(doorSound); // Reproducir sonido de abrir la puerta al entrar
            audioSource.PlayOneShot(firstRoomMusic); // Reproducir música del primer cuarto
            audioSource.loop = true; // Hacer que la música se repita indefinidamente
        }
    }

    void ExitRoom()
    {
        inRoom = false;
        audioSource.PlayOneShot(doorSound); // Reproducir sonido de cerrar la puerta al salir
        audioSource.Stop(); // Detener la música del primer cuarto

        if (inSecondRoom)
        {
            audioSource.Stop(); // Detener la música del segundo cuarto si está reproduciéndose
        }
    }

    void EnterSecondRoom()
    {
        if (!inSecondRoom) // Verificar si el jugador no está ya en el segundo cuarto
        {
            inSecondRoom = true;
            audioSource.PlayOneShot(doorSound); // Reproducir sonido de abrir la puerta al entrar al segundo cuarto
            audioSource.PlayOneShot(secondRoomMusic); // Reproducir música del segundo cuarto
            audioSource.loop = true; // Hacer que la música se repita indefinidamente
        }
    }

    void ExitSecondRoom()
    {
        inSecondRoom = false;
        audioSource.PlayOneShot(doorSound); // Reproducir sonido de cerrar la puerta al salir del segundo cuarto
        audioSource.Stop(); // Detener la música del segundo cuarto

        if (inRoom)
        {
            audioSource.PlayOneShot(firstRoomMusic); // Volver a reproducir la música del primer cuarto si el jugador está en el primer cuarto
        }
    }
}
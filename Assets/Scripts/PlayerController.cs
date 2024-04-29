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
    public GameObject interactMessageNPC1; // Referencia al mensaje de interacci�n con NPC1 en el Canvas
    public GameObject interactMessageNPC2; // Referencia al mensaje de interacci�n con NPC2 en el Canvas
    public AudioClip footstepSound; // Sonido de los pasos del jugador
    public AudioClip doorSound; // Sonido de abrir y cerrar la puerta del primer cuarto
    public AudioClip firstRoomMusic; // M�sica del primer cuarto
    public AudioClip secondRoomMusic; // M�sica del segundo cuarto
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

        // Reproducir sonido de pasos si el jugador se est� moviendo
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
        // Ocultar el cuadro de mensaje de interacci�n
        interactMessageNPC1.SetActive(false);
        interactMessageNPC2.SetActive(false);
        interacting = false;
    }

    void EnterRoom()
    {
        if (!inRoom) // Verificar si el jugador no est� ya en la habitaci�n
        {
            inRoom = true;
            audioSource.PlayOneShot(doorSound); // Reproducir sonido de abrir la puerta al entrar
            audioSource.PlayOneShot(firstRoomMusic); // Reproducir m�sica del primer cuarto
            audioSource.loop = true; // Hacer que la m�sica se repita indefinidamente
        }
    }

    void ExitRoom()
    {
        inRoom = false;
        audioSource.PlayOneShot(doorSound); // Reproducir sonido de cerrar la puerta al salir
        audioSource.Stop(); // Detener la m�sica del primer cuarto

        if (inSecondRoom)
        {
            audioSource.Stop(); // Detener la m�sica del segundo cuarto si est� reproduci�ndose
        }
    }

    void EnterSecondRoom()
    {
        if (!inSecondRoom) // Verificar si el jugador no est� ya en el segundo cuarto
        {
            inSecondRoom = true;
            audioSource.PlayOneShot(doorSound); // Reproducir sonido de abrir la puerta al entrar al segundo cuarto
            audioSource.PlayOneShot(secondRoomMusic); // Reproducir m�sica del segundo cuarto
            audioSource.loop = true; // Hacer que la m�sica se repita indefinidamente
        }
    }

    void ExitSecondRoom()
    {
        inSecondRoom = false;
        audioSource.PlayOneShot(doorSound); // Reproducir sonido de cerrar la puerta al salir del segundo cuarto
        audioSource.Stop(); // Detener la m�sica del segundo cuarto

        if (inRoom)
        {
            audioSource.PlayOneShot(firstRoomMusic); // Volver a reproducir la m�sica del primer cuarto si el jugador est� en el primer cuarto
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenuController : MonoBehaviour
{
    public GameObject menuImage; // Referencia al objeto de la imagen del menú que deseas mostrar
    public Button openButton; // Botón para abrir el menú
    public Button closeButton; // Botón para cerrar el menú

    void Start()
    {
        // Agrega listeners a los botones para que respondan a los eventos de clic
        openButton.onClick.AddListener(OpenMenu);
        closeButton.onClick.AddListener(CloseMenu);

        // Asegúrate de que el menú esté desactivado al inicio
        menuImage.SetActive(false);
    }

    void OpenMenu()
    {
        // Activa el objeto de la imagen del menú
        menuImage.SetActive(true);
    }

    void CloseMenu()
    {
        // Desactiva el objeto de la imagen del menú
        menuImage.SetActive(false);
    }
}
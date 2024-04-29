using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1Controller : MonoBehaviour
{
    public float speed = 3.0f; // Velocidad de movimiento del NPC
    public float pauseDuration = 5.0f; // Duración de la pausa en cada punto de destino

    private Vector3[] destinations = new Vector3[]
    {
        new Vector3(-72, 3.0f, 122),  // Primer punto de destino
        new Vector3(-72, 3.0f, 78),    // Segundo punto de destino
        new Vector3(-28, 3.0f, 78),    // Tercer punto de destino
        new Vector3(-28, 3.0f, 122)    // Cuarto punto de destino
    };

    private int currentDestinationIndex = 0;

    void Start()
    {
        // Mover al NPC hacia el primer punto de destino
        transform.position = destinations[0];

        // Comenzar el movimiento del NPC después de 1 segundo
        StartCoroutine(StartMoving());
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1.0f);

        // Comenzar el movimiento del NPC
        StartCoroutine(MoveNPC());
    }

    IEnumerator MoveNPC()
    {
        while (true)
        {
            // Obtener el punto de destino actual
            Vector3 currentDestination = destinations[currentDestinationIndex];

            // Mover el NPC hacia el punto de destino actual
            while (transform.position != currentDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);
                yield return null;
            }

            // Detener al NPC durante la duración de la pausa
            yield return new WaitForSeconds(pauseDuration);

            // Mover al NPC al siguiente punto de destino
            currentDestinationIndex++;

            // Si hemos alcanzado el último punto de destino, volver al primero
            if (currentDestinationIndex >= destinations.Length)
            {
                currentDestinationIndex = 0;
            }
        }
    }
}
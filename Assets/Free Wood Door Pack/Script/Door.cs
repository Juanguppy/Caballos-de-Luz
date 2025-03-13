using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor, doorGoingToOpen, doorGoingToClose;
        public float openInterval = 10f;
        public float openDuration = 5f;

        private float timer;
        private DoorLight doorLight;

        // Use this for initialization
        void Start()
        {
            asource = GetComponent<AudioSource>();
            doorLight = GetComponentInChildren<DoorLight>(); // Obtener el componente DoorLight del hijo
            timer = openInterval;
            doorLight.SetGreen();
            open = true;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 1)
            {
                doorLight.StartBlinkingAmber();
                asource.clip = doorGoingToClose; // hay que mirar por qué no funciona :(
                Debug.Log("Playing sound");
                asource.Play();
            }

            if (timer <= 0)
            {
                OpenDoor();
                timer = open ? openDuration : openInterval;
            }

            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
            }
        }

        public void OpenDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();

            // Cambiar el color de la luz y el cilindro
            if (doorLight != null)
            {
                if (open)
                {
                    doorLight.SetGreen();
                }
                else
                {
                    doorLight.SetRed();
                }
            }
        }
    }
}
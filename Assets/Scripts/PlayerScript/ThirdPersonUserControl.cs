using UnityEngine;


namespace Character.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform cameraPosition;       // A reference to the main camera in the scenes transform
        private Vector3 cameraForward;          // The current forward direction of the camera
        private Vector3 move;
        private bool jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private MouseLook look;


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                cameraPosition = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            character = GetComponent<ThirdPersonCharacter>();

            look = new MouseLook(transform, Camera.main.transform);
        }

        private void Update()
        {
            if (!jump)
            {
                jump = Input.GetButtonDown("Jump");
            }

            RotateView();
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (cameraPosition != null)
            {
                // calculate camera relative direction to move:
                cameraForward = Vector3.Scale(cameraPosition.forward, new Vector3(1, 0, 1)).normalized;
                move = v * cameraForward + h * cameraPosition.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift))
                move *= 0.5f;
#endif

            // pass all parameters to the character control script
            character.Move(move, crouch, jump);
            jump = false;
            look.UpdateCursorLock();
        }

        private void RotateView()
        {
            look.LookRotation(transform, Camera.main.transform);
        }
    }
}

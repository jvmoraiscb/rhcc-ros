using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class RosOdomSubscriber : UnitySubscriber<MessageTypes.Nav.Odometry>
    {
        [SerializeField]
        private Ackermann ackermann;
        
        private MessageTypes.Nav.Odometry msg;
        private float previousRealTime;
        private bool isMessageReceived;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Nav.Odometry message)
        {
            msg = message;
            isMessageReceived = true;
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
            previousRealTime = Time.realtimeSinceStartup;
        }
        private void ProcessMessage()
        {   
            ackermann.position.x = (float)msg.pose.pose.position.y;
            ackermann.position.y = -0.1f;
            ackermann.position.z = (float)msg.pose.pose.position.x;

            Quaternion quart_aux;

            quart_aux.x = (float)msg.pose.pose.orientation.x;
            quart_aux.y = (float)msg.pose.pose.orientation.y;
            quart_aux.z = (float)msg.pose.pose.orientation.z;
            quart_aux.w = (float)msg.pose.pose.orientation.w;

            Vector3 euler_aux = quart_aux.eulerAngles;

            euler_aux.y = euler_aux.z;
            euler_aux.x = 0;
            euler_aux.z = 0;

            ackermann.rotation = Quaternion.Euler(euler_aux);

            float deltaTime = Time.realtimeSinceStartup - previousRealTime;
            isMessageReceived = false;
        }
    }
}
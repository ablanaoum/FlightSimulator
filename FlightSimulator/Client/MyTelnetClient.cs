using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulatorApp
{
    public class MyTelnetClient : ITelnetClient
    {
        private TcpClient client;
        private NetworkStream stream;

        // Constructor
        public MyTelnetClient() { }

        public void connect(string ip, int port)
        {
            try
            {
                // Create a TcpClient
                this.client = new TcpClient(ip, port);
                // Get a client stream for reading and writing.
                this.stream = client.GetStream();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception: Unable to connect to server");
            }
        }

        public void write(string command)
        {
            try
            {
                // Translate the passed command into ASCII and store it as a Byte array
                byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
                // Send the command to the connected TcpServer
                stream.Write(data, 0, data.Length);
                // Write the command sent to the console
                Console.WriteLine("Sent: {0}", command);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception: Unable to send message to server");
            }
        }

        // Receive the TcpServer response
        public string read()
        {
            // Buffer to store the response bytes
            byte[] data = new byte[256];
            // String to store the response ASCII representation
            string responseData;
            try
            {
                // Read the first batch of the TcpServer response bytes
                int bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                // Write the response received to the console
                Console.WriteLine("Received: {0}", responseData);
                return responseData;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception: Unable to get response from server");
                return null;
            }
        }

        public void disconnect()
        {
            // Close everything
            stream.Close();
            client.Close();
        }
    }
}
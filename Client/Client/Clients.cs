using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Clients
    {
        public string ErrorMsg { get; set; }

        public Clients()
        {
            ErrorMsg = string.Empty;
        }

        public bool IsValidIp(string ipServer)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipServer);
                return true;
            }
            catch (FormatException)
            {
                ErrorMsg = "Adresse IP invalide.";
                return false;
            }
        }

        public bool Connect(string ipServer)
        {
            try
            {
                if (!IsValidIp(ipServer))
                {
                    return false;
                }

                IPAddress ipAddress = IPAddress.Parse(ipServer);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(ipEndPoint);
                Console.WriteLine("Connexion établie");

                //Ajouter la truc qui lance la game
                Game game = new Game();
                game.StartGame(sender);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

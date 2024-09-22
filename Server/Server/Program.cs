using Server;
using Serveur;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

StartComunication();


void StartComunication()
{
    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 11000);
    Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    Socket handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    try
    {
        socketListener.Bind(ipEndPoint);
        socketListener.Listen(10);

        while (true)
        {
            Console.WriteLine("En attente d'une connexion...");

            handler = socketListener.Accept();

            //Ajouter code pour lancer la partie
            Game game = new Game();
            game.StartGame(handler);

            Console.Clear();
            Console.WriteLine("L'adversaire a quitter");
        }
    }
    catch (Exception)
    {
        Console.Clear();
        handler.Close();
        socketListener.Close();
        Console.WriteLine("Client déconnecter retour a la recherche");
        StartComunication();
    }
   
}
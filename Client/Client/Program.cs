using Client;
using System.Net;
using System.Net.Sockets;
using System.Text;

byte[] bytes = new byte[1024];
string message = string.Empty;


StartComunication();

void StartComunication()
{
    var client = new Clients();
    // Connect to a remote device.
    Console.Write("Entrée l'adresse du server : ");
    string? ipServer = Console.ReadLine();
    if(ipServer != null)
    {
        IPAddress ipAdress = IPAddress.Parse(ipServer);

        IPEndPoint ipEndPoint = new IPEndPoint(ipAdress, 11000);
        Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            sender.Connect(ipEndPoint);
            Console.WriteLine("Connexion établie");

            //Ajouter la truc qui lance la game
            Game game = new Game();
            game.StartGame(sender);

            // Release the socket.
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (Exception)
        {
            Console.Clear();
            sender.Close();
            Console.WriteLine("Connection avec le server a été interrompu. Veuillez réessayer.");
            StartComunication();
        }
    }

}


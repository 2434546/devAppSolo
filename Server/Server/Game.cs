using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    public class Game
    {
        Tableau tableau;

        public Game()
        {
            //this.tableau = new Tableau();
        }

        public void StartGame(Socket socket)
        {
            int gridSize = ChoisirTailleGrille();
            EnvoyerTailleGrille(gridSize, socket);

            bool partieAcceptee = RecevoirAccordClient(socket);

            if (partieAcceptee)
            {
                tableau = new Tableau(gridSize);
                ChoisirBateau(socket);

                string win = "notWin";

                while (true)
                {
                    string status;
                    if (win == "notWin")
                        status = AdversaireJouer(socket);
                    else
                        break;

                    if (status == "continu")
                        win = JouerTour(socket);
                    else
                        break;

                }

                RestartGame(socket);
            }
            else
            {
                Console.WriteLine("Le client a refusé la partie.");
            }
        }

        private int ChoisirTailleGrille()
        {
            int taille = 0;
            do
            {
                Console.WriteLine("Choisissez la taille de la grille (entre 4 et 9) : ");
                taille = Convert.ToInt32(Console.ReadLine());
            } while (taille < 4 || taille > 9);

            return taille;
        }

        private void EnvoyerTailleGrille(int gridSize, Socket socket)
        {
            string jsonTaille = Serialiser.SerialiseIntToJson(gridSize);
            byte[] bytes = Encoding.ASCII.GetBytes(jsonTaille);
            socket.Send(bytes);
        }

        private bool RecevoirAccordClient(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            return Serialiser.DeserialiseBoolFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }


        public void RestartGame(Socket socket)
        {
            Console.WriteLine("Votre adversaire décide s'il veut refaire une partie ...");

            Tir? tir = tableau.RecevoirTir(socket);

            if(tir != null)
                if (tir.status == "newGame")
                {
                    tableau.ClearTableau();
                    StartGame(socket);
                }
        }

        public void ChoisirBateau(Socket socket)
        {
            //Attend que le serveur aille choisi
            Console.WriteLine("Votre adversaire choisi l'emplacement du bateau");
            bool bateauChoisiClient = RecevoirChoixBateau(socket);

            bool bateauChoisi = false;

            if (bateauChoisiClient)
            {
                AfficherJeux();

                //Choisi sont bateau
                bateauChoisi = tableau.ChoixBateau();
            }

            AfficherJeux();


            //Fait choisir le serveur
            EnvoyerChoixBateau(bateauChoisi, socket);
        }

        public string JouerTour(Socket socket)
        {
            bool tirEncore = true;
            while (tirEncore)
            {
                Tir? tir = tableau.ChoixTir();
                tir.status = "toCheck";

                tableau.EnvoyerTir(tir, socket);

                tir = tableau.RecevoirTir(socket);

                if (tir != null)
                {
                    if (tir.status == "win")
                    {
                        Console.Clear();
                        Console.WriteLine("Vous avez gagné!!");
                        return "win";
                    }

                    if (tir.hit)
                    {
                        Console.WriteLine("Touché! Vous pouvez rejouer.");
                        tirEncore = true;
                    }
                    else
                    {
                        Console.WriteLine("Dans l'eau! Changement de tour.");
                        tirEncore = false;
                    }

                    tableau.AjoutTir(tir);
                }

                AfficherJeux();
            }

            return "notWin";
        }


        public string AdversaireJouer(Socket socket)
        {
            bool tirEncore = true;
            while (tirEncore)
            {
                Tir? tir = tableau.RecevoirTir(socket);
                if (tir != null)
                {
                    if (tir.status == "toCheck")
                    {
                        tir = tableau.VerificationTir(tir);
                        bool gagnant = tableau.VerifierGagnant();
                        if (gagnant)
                        {
                            tir.status = "win";
                            tableau.EnvoyerTir(tir, socket);
                            Console.Clear();
                            Console.WriteLine("Votre adversaire a gagné");
                            return "";
                        }

                        tableau.EnvoyerTir(tir, socket);
                        
                        if (tir.hit)
                        {
                            Console.WriteLine("Votre adversaire a touché un bateau! Il rejoue.");
                            tirEncore = true;
                        }
                        else
                        {
                            Console.WriteLine("Votre adversaire a raté. À vous de jouer.");
                            tirEncore = false; 
                        }

                        AfficherJeux();
                    }
                }
            }

            return "continu";
        }
        
        public void AfficherJeux()
        {
            Console.Clear();
            AfficherLegende();
            Console.WriteLine("Votre Tableau");
            Console.WriteLine();
            tableau.AffichageTableauJoueur();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Tableau de l'Adversaire");
            Console.WriteLine();
            tableau.AffichageTableauAdversaire();
            Console.WriteLine();
        }

        private void AfficherLegende()
        {
            Console.WriteLine("LÉGENDE :");
            Console.WriteLine("XX = Tir dans l'eau");
            Console.WriteLine("BB = Position du bateau");
            Console.WriteLine("BT = Partie de bateau touché");
        }
        public void EnvoyerChoixBateau(bool bateauChoisi, Socket socket)
        {
            string jsonBool = Serialiser.SerialiseBoolToJson(bateauChoisi);
            byte[] bytes = Encoding.ASCII.GetBytes(jsonBool);
            socket.Send(bytes);
        }

        public bool RecevoirChoixBateau(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            return Serialiser.DeserialiseBoolFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }
    }
}

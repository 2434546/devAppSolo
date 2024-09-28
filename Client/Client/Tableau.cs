using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Tableau
    {
        private string[] tableauJoueur { get; set; }
        private string[] tableauAdversaire { get; set; }
        int size;
        int caseBateauPlace1;
        int caseBateauPlace2;

        public Tableau(int size)
        {
            this.size = size;
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
        }

        public Tir VerificationTir(Tir tir)
        {
            string emplacement = tableauJoueur[tir.coord - 1];
            if (emplacement == "BB")
            {
                tableauJoueur[tir.coord - 1] = "BT";
                tir.hit = true;
            }
            else
            {
                tableauJoueur[tir.coord - 1] = "XX";
            }

            tir.status = "check";
            return tir;
        }

        public void AjoutTir(Tir tir)
        {
            if (tir.hit)
            {
                tableauAdversaire[tir.coord - 1] = "BT";
            }
            else
            {
                tableauAdversaire[tir.coord - 1] = "XX";
            }

        }

        public void AffichageTableauJoueur()
        {
            double gridDimensionJoueur = Math.Sqrt(tableauJoueur.Length);
            int index = 1;        

            for (int i = 0; i < gridDimensionJoueur; i++)
            {
                for (int j = 0; j < gridDimensionJoueur; j++)
                {
                    if (tableauJoueur[(i * size) + j] != null)
                    {
                        if(tableauJoueur[(i * size) + j] == "BT")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (tableauJoueur[(i * size) + j] == "XX")
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else if(tableauJoueur[(i * size) + j] == "BB")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;

                        }

                        Console.Write($" {tableauJoueur[(i * size) + j]} ");
                        Console.ResetColor();
                        Console.Write("|");

                    }

                    else
                    {
                        if (index < 10)
                            Console.Write($" 0{index} |");

                        else
                            Console.Write($" {index} |");
                    }
                    index++;

                   
                }

                if (i < gridDimensionJoueur - 1)
                {
                    Console.WriteLine("");
                    for(int k = 0; k < size; k++)
                    {
                        Console.Write("-----");
                    }
                    Console.WriteLine("");
                }

            }
        }

        public void AffichageTableauAdversaire()
        {
            double gridDimensionAdversaire = Math.Sqrt(tableauAdversaire.Length);
            int index = 1;

            for (int i = 0; i < gridDimensionAdversaire; i++)
            {
                for (int j = 0; j < gridDimensionAdversaire; j++)
                {

                        if(tableauAdversaire[(i * size) + j] != null)
                        {

                            if (tableauAdversaire[(i * size) + j] == "BT")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else if (tableauAdversaire[(i * size) + j] == "XX")
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }

                            Console.Write($" {tableauAdversaire[(i * size) + j]} ");
                            Console.ResetColor();
                            Console.Write("|");

                        }

                    else
                        {
                            if(index < 10)
                            Console.Write($" 0{index} |");

                            else
                            Console.Write($" {index} |");
                        }
                        index++;
                        
                }
                if (i < gridDimensionAdversaire - 1)
                {
                    Console.WriteLine("");
                    for (int k = 0; k < size; k++)
                    {
                        Console.Write("-----");
                    }
                    Console.WriteLine("");
                }

            }
        }

        public void EnvoyerTir(Tir tir, Socket socket)
        {
            string JsonTir = Serialiser.SerialiseTirToJson(tir);
            byte[] bytes = Encoding.ASCII.GetBytes(JsonTir);
            socket.Send(bytes);
        }

        public Tir? RecevoirTir(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRecu = socket.Receive(bytes);
            return Serialiser.DeserialiseTirFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRecu));
        }

        public Tir ChoixTir()
        {

            int coordChoisi = 0;
            bool caseValide = false;
            
            do
            {
                Console.WriteLine("Veuillez choisir une position où tirer dans le tableau");
                coordChoisi = Convert.ToInt32(Console.ReadLine());

                if (coordChoisi >= 1 && coordChoisi <= size * size)
                {
                    if (tableauAdversaire[coordChoisi - 1] != "XX" && tableauAdversaire[coordChoisi - 1] != "BT")
                    {
                        caseValide = true;
                    }
                    else
                    {
                        Console.WriteLine("La case a déjà été choisie.");
                    }
                }
                else
                {
                    Console.WriteLine("Coord hors limites");
                }
            }
            while (!caseValide);

            Tir tir = new Tir(coordChoisi);
            return tir;
        }     

        public bool ChoixBateau()
        {
            bool placementValide = false;
            string[]? coord;

            do
            {

                Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
                string? input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && input.Contains(","))
                {

                    coord = input.Split(",");

                    if (coord == null)
                        coord = input.Split(".");

                    if (coord.Length == 2 && int.TryParse(coord[0], out caseBateauPlace1) && int.TryParse(coord[1], out caseBateauPlace2))
                    {
                        if (caseBateauPlace1 >= 1 && caseBateauPlace1 <= size * size && caseBateauPlace2 >= 1 && caseBateauPlace2 <= size * size)
                        {

                            if (Math.Abs(caseBateauPlace1 - caseBateauPlace2) == 1 || Math.Abs(caseBateauPlace1 - caseBateauPlace2) == size)
                            {
                                if (Math.Abs(caseBateauPlace1 - caseBateauPlace2) == 1 && (Math.Max(caseBateauPlace1, caseBateauPlace2) % size == 1))
                                {
                                    Console.WriteLine("Les cases sélectionné sont placées sur deux lignes différentes opposé");
                                }
                                else if (tableauJoueur[caseBateauPlace1 - 1] == null && tableauJoueur[caseBateauPlace2 - 1] == null)
                                {
                                    tableauJoueur[caseBateauPlace1 - 1] = "BB";
                                    tableauJoueur[caseBateauPlace2 - 1] = "BB";
                                    Console.WriteLine($"Le bateau a été placé aux coordonnées {caseBateauPlace1} et {caseBateauPlace2}");
                                    placementValide = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Les coordonnées ne sont pas alignées. Le bateau doit être placé horizontalement ou verticalement.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Coordonnées hors limites. Veuillez choisir des positions valides dans le tableau.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrée invalide. Veuillez entrer des nombres entiers.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer deux entiers séparés par une virgule.");
                }

            } while (!placementValide);

            return placementValide;
        }

        public bool VerifierGagnant()
        {

            //TODO s'organiser pour que le code soit facilement modifiable si des s'ajoute au jeu ou si les dimensions changent
            if (tableauJoueur[caseBateauPlace1 - 1] == "BT" && tableauJoueur[caseBateauPlace2 - 1] == "BT")
            {
                return true;
            }

            else
                return false;
                
        }

        public void ClearTableau()
        {
            tableauAdversaire = new string[size * size];
            tableauJoueur = new string[size * size];
        }
    }
}

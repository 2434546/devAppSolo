using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Servers
    {
        public string[] tableauJoueur { get; set; }
        public string[] tableauAdversaire { get; set; }
        int size;
        int caseBateauPlace1;
        int caseBateauPlace2;


        public Servers()
        {
            size = 4;
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
        }

        public bool ChoixBateau(string input)
        {
            bool placementValide = false;
            string[]? coord;

            do
            {

                Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
                //string input = Console.ReadLine();

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
    }

}

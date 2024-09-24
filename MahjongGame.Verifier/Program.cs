using System;
using UltimateMahjongConnect.Core.Net.Models;

namespace MahjongGameVerifier

{
    class Program
    {
        static void Main(string[] args)
        {
            // Créez une instance de MahjongTile
            var mahjongTile = new MahjongTile();

            // Créez une instance de MahjongBoard avec des dimensions de plateau de 12x12
            var game = new MahjongBoard(mahjongTile);

            // Initialisez le plateau de manière déterministe
            game.InitializeBoardDeterministically();

            // Affichez quelques informations sur le jeu pour vérification
            Console.WriteLine("Jeu de Mahjong créé avec succès !");
            Console.WriteLine($"Nombre de lignes : 12");
            Console.WriteLine($"Nombre de colonnes : 12");

            // Affichez le plateau initialisé
            DisplayBoard(game);

            game.InitializeBoardRandom();

            // Affichez quelques informations sur le jeu pour vérification
            Console.WriteLine("Jeu de Mahjong créé avec succès !");
            Console.WriteLine($"Nombre de lignes : 12");
            Console.WriteLine($"Nombre de colonnes : 12");

            DisplayBoard(game);

            Console.WriteLine("Vérification terminée. Appuyez sur une touche pour quitter.");
            Console.ReadKey();
        }

        static void DisplayBoard(MahjongBoard board)
        {
            const int columnWidth = 12; // Ajuster la largeur pour chaque colonne

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    var tile = board[i, j];
                    if (tile != null)
                    {
                        Console.Write($"{tile.ToString().PadRight(columnWidth)}");
                    }
                    else
                    {
                        Console.Write("[Empty]".PadRight(columnWidth));
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

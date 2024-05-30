using System;
using MahjongConnect.Logic;
using static UltimateMahjongConnect.Core.Net.MahjongBoard;

namespace MahjongGameVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            // Créez une instance de jeu Mahjong avec des dimensions de plateau de 10x10
            var game = new MahjongGame(10, 10);

            // Affichez quelques informations sur le jeu pour vérification
            Console.WriteLine("Jeu de Mahjong créé avec succès !");
            Console.WriteLine($"Nombre de lignes : {game.Rows}");
            Console.WriteLine($"Nombre de colonnes : {game.Columns}");

            // Ajoutez des joueurs pour tester
            var player1 = new Client("John", "Doe", 30, "john@example.com");
            var player2 = new Client("Jane", "Smith", 25, "jane@example.com");

            // Autres vérifications peuvent être ajoutées selon les besoins

            Console.WriteLine("Vérification terminée. Appuyez sur une touche pour quitter.");
            Console.ReadKey();
        }
    }
}

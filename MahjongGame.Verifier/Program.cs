using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Domain.Models;

namespace MahjongGame.Verifier

{
    class Program
    {
        static void Main(string[] args)
        {
            var mahjongTile = new MahjongTile();

            var game = new MahjongBoard(mahjongTile);

            game.InitializeBoardDeterministically();

            Console.WriteLine("Jeu de Mahjong créé avec succès !");
            Console.WriteLine($"Nombre de lignes : 12");
            Console.WriteLine($"Nombre de colonnes : 12");

            DisplayBoard(game);

            game.InitializeBoardRandom();

            Console.WriteLine("Jeu de Mahjong créé avec succès !");
            Console.WriteLine($"Nombre de lignes : 12");
            Console.WriteLine($"Nombre de colonnes : 12");

            DisplayBoard(game);

            Console.WriteLine("Vérification terminée. Appuyez sur une touche pour quitter.");
            Console.ReadKey();
        }

        static void DisplayBoard(MahjongBoard board)
        {
            const int columnWidth = 12; 

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

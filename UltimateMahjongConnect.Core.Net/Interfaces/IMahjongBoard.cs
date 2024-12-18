﻿using UltimateMahjongConnect.Business.Models;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongBoard
    {
        int GetRows();
        int GetColumns();
        void InitializeBoardDeterministically();
        void TransposeBoard();
        void InitializeBoardRandom();
        void ReverseInitializeBoardDeterministically();
        IMahjongTile this[int row, int col] { get; }
        bool IsPathValid(int row1, int col1, int row2, int col2);
        MahjongPath GetValidatedPath(int row1, int column1, int row2, int column2);
        void MatchTiles(IMahjongTile tile1, IMahjongTile tile2);
    }
}

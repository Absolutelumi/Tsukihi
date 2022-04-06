using System.Collections.Generic;
using System.Drawing;

namespace Tsukihi.Chess
{
    internal static class ChessExtensions
    {
        internal static bool Has(this List<Position> positions, Position position)
        {
            foreach (Position pos in positions)
                if (pos.File == position.File && pos.Rank == position.Rank)
                    return true;

            return false;
        }

        internal static string UpdateBoardImage(this Board board, string channelId)
        {
            Bitmap boardImage = new Bitmap(board.ImagePath);

            using (var graphics = Graphics.FromImage(boardImage))
            {
                for (int file = 0; file < 8; file++)
                for (int rank = 0; rank < 8; rank++)
                    {
                        if (board.Pieces[rank, file] == null) continue;
                        graphics.DrawImage(new Bitmap(board.Pieces[rank, file].ImagePath), new Rectangle(65 + 138 * rank, 65 + 138 * (7 - file), 120, 120));
                    }
            }

            string path = Tsukihi.TempPath + $"chess{channelId}.png";
            boardImage.Save(path);
            return path;
        }

        internal static Board Copy(this Board board)
        {
            Piece[,] pieces = new Piece[8, 8];

            for (int file = 0; file < 8; file++)
            for (int rank = 0; rank < 8; rank++)
                {
                    pieces[file, rank] = board.Pieces[file, rank];
                }

            return new Board(pieces, board.Turn);
        }

        private static bool IsCheck(this Board board)
        {
            // Board.Next() only checks *possible* moves, so it only checks current turn's player's possible moves
            // To test whether enemy pieces can take the king, create new board with opposite turn
            Board test = new Board(board.Pieces, board.Turn.Opposite());

            // If any outcomes do not have ally king, ally is in check
            foreach (Board outcome in test.Next())
                if (!outcome.Has<King>(board.Turn))
                    return true;

            return false;
        }

        // Right now, counts draws as checkmate - needs to be reworked
        private static bool IsCheckmate(this Board board)
        {
            // Loop through all possible moves by check'd player
            foreach (Board outcome in board.Next())
            {
                // As Board.IsCheck() checks whether the current turn's player is in check,
                // outcome's turn needs to be inverted to see whether board's player is still in check
                // (As calling .Next() simulates all moves, the turn is flipped)
                Board test = new Board(outcome.Pieces, board.Turn);
                if (!test.IsCheck()) return false;
            }

            return true;
        }

        internal static (bool Check, bool Checkmate) InCheck(this Board board)
        {
            // Only check for checkmate if board is in check
            bool isCheck = board.IsCheck();
            return (isCheck, isCheck ? board.IsCheckmate() : false);
        }

        internal static PlayerType Opposite(this PlayerType player) => player == PlayerType.White ? PlayerType.Black : PlayerType.White;

        internal static Piece[,] GetDefaultBoard()
        {
            Piece[,] board = new Piece[8, 8];

            for (int i = 0; i < 8; i++)
            {
                board[i, 1] = new Pawn(PlayerType.White);
                board[i, 6] = new Pawn(PlayerType.Black);

                switch (i)
                {
                    case 0:
                    case 7:
                        board[i, 0] = new Rook(PlayerType.White);
                        board[i, 7] = new Rook(PlayerType.Black);
                        break;

                    case 1:
                    case 6:
                        board[i, 0] = new Knight(PlayerType.White);
                        board[i, 7] = new Knight(PlayerType.Black);
                        break;

                    case 2:
                    case 5:
                        board[i, 0] = new Bishop(PlayerType.White);
                        board[i, 7] = new Bishop(PlayerType.Black);
                        break;

                    case 3:
                        board[i, 0] = new Queen(PlayerType.White);
                        board[i, 7] = new Queen(PlayerType.Black);
                        break;

                    case 4:
                        board[i, 0] = new King(PlayerType.White);
                        board[i, 7] = new King(PlayerType.Black);
                        break;
                }
            }

            return board;
        }
    }
}

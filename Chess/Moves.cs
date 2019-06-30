using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves (Board board)
        {
            this.board = board;
        }

        public bool CanMove (FigureMoving fm)
        {
            this.fm = fm;
            return
                CanMoveFrom() &&
                CanMoveTo() &&
                CanFigureMove();
        }

        bool CanMoveFrom ()
        {
            return fm.from.OnBorad() &&
                fm.figure.GetColor() == board.moveColor;
        }

        bool CanMoveTo()
        {
            return fm.from != fm.to && fm.to.OnBorad() &&
                board.GetFigureAt(fm.to).GetColor() != board.moveColor;
        }

        bool CanFigureMove()
        {
            switch (fm.figure)
            {
                case Figure.blackKing:
                case Figure.whiteKing:
                    return CanKingMove();

                case Figure.blackQueen:
                case Figure.whiteQueen:
                    return CanStraightMove();

                case Figure.blackRook:
                case Figure.whiteRook:
                    return (fm.SignX == 0 || fm.SignY == 0) &&
                        CanStraightMove();

                case Figure.blackBishop:
                case Figure.whiteBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) &&
                        CanStraightMove();


                case Figure.blackKnight:
                case Figure.whiteKnight:
                    return CanKnightMove();
                    

                case Figure.blackPawn:
                case Figure.whitePawn:
                    return CanPawnMove();


                default:
                    return false;
            }
        }

        private bool CanPawnMove()
        {
            if (fm.from.y < 1 || fm.from.y > 6)
                return false;
            int stepY = fm.figure.GetColor() == Color.white ? 1 : -1;
            return
                CanPawnGo(stepY) ||
                CanPawnJump(stepY) ||
                CanPawnEat(stepY);
        }

        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAt(fm.to) != Figure.none)
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }

        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigureAt(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == 2 * stepY)
                        if (fm.from.y == 1 || fm.from.y == 6)
                            if (board.GetFigureAt(new Square(fm.from.x, fm.from.y + stepY)) == Figure.none)
                                return true;
            return false;
        }

        private bool CanPawnGo(int stepY)
        {
            if (board.GetFigureAt(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }

        private bool CanKingMove() => fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1;


        private bool CanKnightMove() => fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2
            || fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1;

        private bool CanStraightMove()
        {
            Square at = fm.from;
            do
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY);
                if (at == fm.to)
                    return true;
            } while (at.OnBorad() && board.GetFigureAt(at) == Figure.none);
            return false;
        }

        
    }
}

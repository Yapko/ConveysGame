using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConveysGameOfLife
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game()
			{
				FieldSize = 10,
				Cycles = 50,
				InitialLiveCells = new List<Point> {
					new Point { X = 1, Y = 1 },
					new Point { X = 2, Y = 2 },
					new Point { X = 3, Y = 3 },
					new Point { X = 4, Y = 4 },
				}
			};

			game.OnIterationComplete += (a, b) =>
			{
				bool[,] field = game.CurrentGameState;
				for (int i = 0; i < game.FieldSize; ++i)
				{
					for (int j = 0; j < game.FieldSize; ++j)
					{
						Console.Write(field[i, j] ? 1 : 0);
					}

					Console.WriteLine();
				}

				Console.WriteLine("===============================");
			};

			game.Run();
		}
	}
}

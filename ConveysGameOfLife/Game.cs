using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConveysGameOfLife
{
	public class Game
	{
		public int FieldSize { get; set; }
		public int Cycles { get; set; }
		public List<Point> InitialLiveCells { get; set; }
		public event EventHandler OnIterationComplete;

		private bool[,] _mem1;
		private bool[,] _mem2;

		private int _currentCycle;
		private bool[,] ReadMem
		{
			get
			{
				return _currentCycle % 2 == 0 ? _mem1 : _mem2;
			}
		}

		private bool[,] WriteMem
		{
			get
			{
				return _currentCycle % 2 == 0 ? _mem2 : _mem1;
			}
		}

		public bool[,] CurrentGameState
		{
			get { return ReadMem; }
		}

		private void Reinit()
		{
			_mem1 = new bool[FieldSize, FieldSize];
			_mem2 = new bool[FieldSize, FieldSize];

			foreach (Point p in InitialLiveCells)
			{
				_mem1[p.X, p.Y] = true;
			}

			_currentCycle = 0;
		}

		private int ReadValueAt(int x, int y)
		{
			// change stategy here, right now toroidal array
			//return ReadMem[x % FieldSize, y % FieldSize] ? 1 : 0;

			// outside filed cells are dead
			if (x >= 0 && x < FieldSize && y >= 0 && y < FieldSize)
				return ReadMem[x % FieldSize, y % FieldSize] ? 1 : 0;
			else return 0;
		}

		public void Run()
		{
			Reinit();
			for (_currentCycle = 0; _currentCycle < Cycles; ++_currentCycle)
			{
				for (int x = 0; x < FieldSize; ++x)
				{
					for (int y = 0; y < FieldSize; ++y)
					{
						int liveNeighbours = 0;
						liveNeighbours += ReadValueAt(x - 1, y - 1);
						liveNeighbours += ReadValueAt(x - 1, y);
						liveNeighbours += ReadValueAt(x - 1, y + 1);
						liveNeighbours += ReadValueAt(x, y - 1);
						liveNeighbours += ReadValueAt(x, y + 1);
						liveNeighbours += ReadValueAt(x + 1, y - 1);
						liveNeighbours += ReadValueAt(x + 1, y);
						liveNeighbours += ReadValueAt(x + 1, y + 1);

						if (ReadMem[x, y] == true)
						{
							if (liveNeighbours < 2) WriteMem[x, y] = false;
							if (liveNeighbours == 2 || liveNeighbours == 3) WriteMem[x, y] = true;
							if (liveNeighbours > 3) WriteMem[x, y] = false;
						}
						else if (liveNeighbours == 3)
						{
							WriteMem[x, y] = true;
						}
					}
				}

				// todo: emit event that cycle updated
				if (OnIterationComplete != null)
				{
					OnIterationComplete(this, new EventArgs());
				}
			}
		}
	}
}

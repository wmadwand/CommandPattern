using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMS03
{
	public interface ICommand
	{
		void Execute();
		void Undo();
	}

	public class MacroCommand : ICommand
	{
		private List<ICommand> _commands;

		public MacroCommand()
		{
			_commands = new List<ICommand>();
		}

		public void Add(ICommand command)
		{
			_commands.Add(command);
		}

		public void Remove(ICommand command)
		{
			_commands.Remove(command);
		}

		public void Execute()
		{
			foreach (ICommand item in _commands)
			{
				item.Execute();
			}
		}

		public void Undo()
		{
			int count = _commands.Count - 1;

			for (int i = count; i >= 0; i--)
			{
				_commands[i].Undo();
			}
		}
	}

	public class Coder
	{
		public void StartCoding() { Console.WriteLine("StartCoding"); }

		public void StopCoding() { Console.WriteLine("StopCoding"); }
	}

	public class CoderCommand : ICommand
	{
		private Coder _coder;

		public CoderCommand(Coder coder)
		{
			_coder = coder;
		}

		public void Execute()
		{
			_coder.StartCoding();
		}

		public void Undo()
		{
			_coder.StopCoding();
		}
	}

	public class Tester
	{
		public void StartTesting() { Console.WriteLine("StartTesting"); }

		public void StopTesting() { Console.WriteLine("StopTesting"); }
	}

	public class TesterCommand : ICommand
	{
		private Tester _tester;

		public TesterCommand(Tester tester)
		{
			_tester = tester;
		}

		public void Execute()
		{
			_tester.StartTesting();
		}

		public void Undo()
		{
			_tester.StopTesting();
		}
	}

	public class Manager
	{
		private ICommand _command;

		public void SetCommand(ICommand command)
		{
			_command = command;
		}

		public void StartProject()
		{
			_command.Execute();
		}

		public void StopProject()
		{
			_command.Undo();
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			Coder coder = new Coder();
			Tester tester = new Tester();

			MacroCommand macroCommand = new MacroCommand();
			macroCommand.Add(new CoderCommand(coder));
			macroCommand.Add(new TesterCommand(tester));

			Manager manager = new Manager();
			manager.SetCommand(macroCommand);

			manager.StartProject();
			manager.StopProject();

			Console.ReadKey();
		}
	}
}

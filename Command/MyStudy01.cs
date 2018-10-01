using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMS01
{
	// command interface
	public interface ICommand
	{
		void Execute();
		void Undo();
	}

	public class NoCommand : ICommand
	{
		public void Execute() { }

		public void Undo() { }
	}

	// receiver
	public class TV
	{
		public void On()
		{
			Console.WriteLine("TV is on");
		}

		public void Off()
		{
			Console.WriteLine("TV is off");
		}
	}

	// concrete command
	public class TVOnCommand : ICommand
	{
		private TV _tv;

		public TVOnCommand(TV tv)
		{
			this._tv = tv;
		}

		public void Execute()
		{
			_tv.On();
		}

		public void Undo()
		{
			_tv.Off();
		}
	}

	// invoker
	public class RemoteControl
	{
		private ICommand _command;

		public RemoteControl()
		{
			_command = new NoCommand();
		}

		public void SetCommand(ICommand command)
		{
			_command = command;
		}

		public void PressButton()
		{
			_command?.Execute();
		}

		public void PressButtonUndo()
		{
			_command?.Undo();
		}
	}

	// ---------------------------------------

	// another one receiver
	public class Microwave
	{
		public void StartCooking()
		{
			Console.WriteLine("StartCooking");
		}

		public void StopCooking()
		{
			Console.WriteLine("StopCooking");
		}
	}

	// another one command
	public class MicrowaveCommand : ICommand
	{
		private Microwave _microwave;

		public MicrowaveCommand(Microwave microwave)
		{
			_microwave = microwave;
		}

		public void Execute()
		{
			_microwave.StartCooking();
		}

		public void Undo()
		{
			_microwave.StopCooking();
		}
	}

	// ---------------------------------------

	public class SimpleCommand : ICommand
	{
		private Action _action;

		public SimpleCommand(Action action)
		{
			_action = action;
		}

		public void Execute()
		{
			_action?.Invoke();
		}

		public void Undo()
		{

		}
	}

	// client
	public class Program
	{
		static void Main22(string[] args)
		{
			TV tv = new TV(); // receiver
			ICommand tvOnCommand = new TVOnCommand(tv); // concrete command

			RemoteControl remoteControl = new RemoteControl(); // invoker
			remoteControl.SetCommand(tvOnCommand);

			remoteControl.PressButton();
			remoteControl.PressButtonUndo();

			Console.ReadKey();

			Microwave microwave = new Microwave();
			ICommand microwaveCommand = new MicrowaveCommand(microwave);

			remoteControl.SetCommand(microwaveCommand);
			remoteControl.PressButton();
			remoteControl.PressButtonUndo();

			ICommand simpleCommand = new SimpleCommand(microwave.StartCooking);
			simpleCommand.Execute();
		}
	}
}
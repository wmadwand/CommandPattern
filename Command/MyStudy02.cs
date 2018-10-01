using System;
using System.Collections.Generic;

namespace CommandMS02
{
	public interface ICommand
	{
		void Execute();
		void Undo();
	}

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

	public class TVOnCommand : ICommand
	{
		private TV _tv;

		public TVOnCommand(TV tv)
		{
			_tv = tv;
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

	public class Volume
	{
		private const int OFF = 0;
		private const int HIGH = 20;

		private int _level;

		public Volume()
		{
			_level = OFF;
		}

		public void RaiseSound()
		{
			if (_level < HIGH)
			{
				_level++;
				Console.WriteLine(_level);
			}
		}

		public void DropSound()
		{
			if (_level > OFF)
			{
				_level--;
				Console.WriteLine(_level);
			}
		}
	}

	public class VolumeCommand : ICommand
	{
		private Volume _volume;

		public VolumeCommand(Volume volume)
		{
			_volume = volume;
		}

		public void Execute()
		{
			_volume.RaiseSound();
		}

		public void Undo()
		{
			_volume.DropSound();
		}
	}

	public enum ButtonType
	{
		TVOn,
		ChangeVolume
	}

	public class MultiRemoteControl
	{
		private Dictionary<ButtonType, ICommand> _commands;
		private Stack<ICommand> _commandsHistory;

		public MultiRemoteControl()
		{
			_commands = new Dictionary<ButtonType, ICommand>();
			_commandsHistory = new Stack<ICommand>();
		}

		public void SetCommand(ButtonType button, ICommand command)
		{
			_commands[button] = command;
		}

		public void PressButton(ButtonType button)
		{
			if (_commands.TryGetValue(button, out ICommand command))
			{
				command?.Execute();
				_commandsHistory.Push(command);
			}
		}

		public void PressUndoButton()
		{
			if (_commandsHistory.Count > 0)
			{
				ICommand command = _commandsHistory.Pop();
				command.Undo();
			}
		}
	}

	// client
	public class Program
	{
		public static void Main(string[] args)
		{
			// receivers
			TV tv = new TV();
			Volume volume = new Volume();

			// commands
			ICommand tvOnCommand = new TVOnCommand(tv);
			ICommand volumeChange = new VolumeCommand(volume);

			// invoker
			MultiRemoteControl control = new MultiRemoteControl();
			control.SetCommand(ButtonType.TVOn, tvOnCommand);
			control.SetCommand(ButtonType.ChangeVolume, volumeChange);

			control.PressButton(ButtonType.TVOn);

			control.PressButton(ButtonType.ChangeVolume);
			control.PressButton(ButtonType.ChangeVolume);
			control.PressButton(ButtonType.ChangeVolume);
			control.PressButton(ButtonType.ChangeVolume);

			control.PressUndoButton();
			control.PressUndoButton();
			control.PressUndoButton();
			control.PressUndoButton();
			control.PressUndoButton();

			Console.ReadKey();
		}
	}
}

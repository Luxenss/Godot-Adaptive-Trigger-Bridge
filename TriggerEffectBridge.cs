/*
 * MIT License
 * 
 * Copyright (c) 2025 Ramazan "Luxends" Sen
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Godot;
using System;
using HidSharp;
using System.Linq;
using System.Collections.Generic;
using ExtendInput.DataTools.DualSense;

public partial class TriggerEffectBridge : Node
{
	private List<HidDevice> allDualSenses = new List<HidDevice>();
	private byte[] lastEffectData = new byte[64];
	private bool reset_triggers_on_quit = true;
	private bool error_on_invalid_index = false;
	private bool print_on_effect_apply = false;

	public override void _Ready()
	{
		DeviceList.Local.Changed += DeviceList_Changed;
		FindDualSense();
	}

	private void DeviceList_Changed(object sender, DeviceListChangedEventArgs e)
	{
		FindDualSense();
	}

	private void FindDualSense()
	{
		allDualSenses = DeviceList.Local
			.GetHidDevices(0x054C)
			.Where(d => d.ProductID == 0x0CE6 || d.ProductID == 0x0DF2)
			.ToList();

		if (allDualSenses.Count > 0)
		{
			GD.Print($"{allDualSenses.Count} DualSense{(allDualSenses.Count > 1 ? "s" : "")} found.");
			GD.Print(GetConnectedDualSenseInfos());
		}
		else
			GD.Print("No DualSense found.");
	}
	
	public bool HasDualSense()
	{
		return allDualSenses.Count > 0;
		}
		
	public Godot.Collections.Dictionary GetConnectedDualSenseInfos()
	{
		var result = new Godot.Collections.Dictionary();
	
		for (int i = 0; i < allDualSenses.Count; i++)
		{
			var device = allDualSenses[i];
			var dict = new Godot.Collections.Dictionary
			{
				{ "ProductName", device.ProductName },
				{ "Manufacturer", device.Manufacturer },
				{ "VendorID", device.VendorID },
				{ "ProductID", device.ProductID }
			};
	
			result[i] = dict;
		}
	
		return result;
	}
	
	public bool Off(string side)
	{
		return Off(side, 0);
	}
	
	public bool Off(string side, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Off(data, index),
			controllerIndex);
		return result;
	
	}
		// For callers from GDScript who don't provide controllerIndex
	public bool Feedback(string side, int position, int strength)
	{
		return Feedback(side, position, strength, 0);
	}
	
	public bool Feedback(string side, int position, int strength, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Feedback(
				data,
				index,
				(byte)position,
				(byte)strength),
			controllerIndex);
		return result;
	}
	
	public bool Weapon(string side, int startPosition, int endPosition, int strength)
	{
		return Weapon(side, startPosition, endPosition, strength, 0);
	}
	
	public bool Weapon(string side, int startPosition, int endPosition, int strength, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Weapon(
				data,
				index,
				(byte)startPosition,
				(byte)endPosition,
				(byte)strength),
			controllerIndex);
		return result;
	}
	
	public bool Vibration(string side, int position, int amplitude, int frequency)
	{
		return Vibration(side, position, amplitude, frequency, 0);
	}
	
	public bool Vibration(string side, int position, int amplitude, int frequency, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Vibration(
				data,
				index,
				(byte)position,
				(byte)amplitude,
				(byte)frequency),
			controllerIndex);
		return result;
	}
	
	public bool MultiplePositionFeedback(string side, byte[] strength)
	{
		return MultiplePositionFeedback(side, strength, 0);
	}
	
	public bool MultiplePositionFeedback(string side, byte[] strength, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.MultiplePositionFeedback(data, index, strength),
			controllerIndex);
		return result;
	}
	
	public bool SlopeFeedback(string side, int startPos, int endPos, int startStrength, int endStrength)
	{
		return SlopeFeedback(side, startPos, endPos, startStrength, endStrength, 0);
	}
	
	public bool SlopeFeedback(string side, int startPos, int endPos, int startStrength, int endStrength, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.SlopeFeedback(
				data,
				index,
				(byte)startPos,
				(byte)endPos,
				(byte)startStrength,
				(byte)endStrength),
			controllerIndex);
		return result;
	}
	
	public bool MultiplePositionVibration(string side, byte frequency, byte[] amplitude)
	{
		return MultiplePositionVibration(side, frequency, amplitude, 0);
	}
	
	public bool MultiplePositionVibration(string side, byte frequency, byte[] amplitude, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.MultiplePositionVibration(data, index, frequency, amplitude),
			controllerIndex);
		return result;
	}
	
	
	
	public bool Bow(string side, int startPos, int endPos, int strength, int snapForce)
	{
		return Bow(side, startPos, endPos, strength, snapForce, 0);
	}
	
	public bool Bow(string side, int startPos, int endPos, int strength, int snapForce, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Bow(
				data,
				index,
				(byte)startPos,
				(byte)endPos,
				(byte)strength,
				(byte)snapForce),
			controllerIndex);
		return result;
	}
	
	public bool Galloping(string side, int startPos, int endPos, int firstFoot, int secondFoot, int frequency)
	{
		return Galloping(side, startPos, endPos, firstFoot, secondFoot, frequency, 0);
	}
	
	public bool Galloping(string side, int startPos, int endPos, int firstFoot, int secondFoot, int frequency, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Galloping(
				data,
				index,
				(byte)startPos,
				(byte)endPos,
				(byte)firstFoot,
				(byte)secondFoot,
				(byte)frequency),
			controllerIndex);
		return result;
	}
	
	public bool Machine(string side, int startPos, int endPos, int amplitudeA, int amplitudeB, int frequency, int period)
	{
		return Machine(side, startPos, endPos, amplitudeA, amplitudeB, frequency, period, 0);
	}
	
	public bool Machine(string side, int startPos, int endPos, int amplitudeA, int amplitudeB, int frequency, int period, int controllerIndex)
	{
		bool result = false;
		ApplyTriggerEffect(side, (data, index) =>
			result = TriggerEffectGenerator.Machine(
				data,
				index,
				(byte)startPos,
				(byte)endPos,
				(byte)amplitudeA,
				(byte)amplitudeB,
				(byte)frequency,
				(byte)period),
			controllerIndex);
		return result;
	}
	
	private void ApplyTriggerEffect(string side, Func<byte[], int, bool> effectFunc, int controllerIndex = 0)
	{
		if (controllerIndex < 0 || controllerIndex >= allDualSenses.Count)
		{
			if (error_on_invalid_index)
			{
				GD.PrintErr($"Invalid DualSense index: {controllerIndex}");
			}	
			return;
		}

		if (!allDualSenses[controllerIndex].TryOpen(out var localStream))
		{
			GD.PrintErr($"Failed to open DualSense #{controllerIndex}.");
			return;
		}

		using (localStream)
		{
			if (!localStream.CanWrite)
			{
				GD.PrintErr($"DualSense #{controllerIndex} is not writable.");
				return;
			}

			byte[] data = new byte[64];
			Array.Copy(lastEffectData, data, 64);

			data[0] = 0x02;
			data[1] = 0xFF;
			data[10] = 0x08;

			bool result = false;
			if (side == "right" || side == "r")
			{
				result = effectFunc(data, 11);
			}
			else if (side == "left" || side == "l")
			{
				result = effectFunc(data, 22);
			}
			else if (side == "both" || side == "b")
			{
				bool resultLeft = effectFunc(data, 22);
				bool resultRight = effectFunc(data, 11);
				result = resultLeft && resultRight;
			}
			else
			{
				GD.PrintErr("Invalid trigger side: " + side);
				return;
			}

			try
			{
				localStream.Write(data, 0, 48);
				localStream.Flush();
				Array.Copy(data, lastEffectData, 64);
				if (print_on_effect_apply)
				{
					GD.Print($"DualSense #{controllerIndex} - {side} trigger effect applied. Result: {result}");
				}
			}
			catch (Exception ex)
			{
				GD.PrintErr("Error while sending data: " + ex.Message);
			}
		}
	}

	public override void _ExitTree()
	{
		if (!reset_triggers_on_quit)
			return;

		for (int i = 0; i < allDualSenses.Count; i++)
		{
			if (!allDualSenses[i].TryOpen(out var localStream))
			{
				GD.PrintErr($"Failed to open DualSense #{i} on exit.");
				continue;
			}

			using (localStream)
			{
				if (!localStream.CanWrite)
				{
					GD.PrintErr($"DualSense #{i} is not writable on exit.");
					continue;
				}

				byte[] data = new byte[64];
				data[0] = 0x02;
				data[1] = 0xFF;
				data[10] = 0x08;

				TriggerEffectGenerator.Off(data, 11); // Right trigger
				TriggerEffectGenerator.Off(data, 22); // Left trigger

				try
				{
					localStream.Write(data, 0, 48);
					localStream.Flush();
					GD.Print($"DualSense #{i} triggers reset on exit.");
				}
				catch (Exception ex)
				{
					GD.PrintErr($"Error resetting DualSense #{i} on exit: {ex.Message}");
				}
			}
		}
	}
}

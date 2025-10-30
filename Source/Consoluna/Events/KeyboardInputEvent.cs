/*
 * Copyright (c). 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib.Events
{
	//*-------------------------------------------------------------------------*
	//*	KeyboardInputEventArgs																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Event arguments for handling a keyboard events on Consoluna applications.
	/// </summary>
	public class KeyboardInputEventArgs : InputEventArgs
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the KeyboardInputEventArgs item.
		/// </summary>
		public KeyboardInputEventArgs()
		{
			EventType = InputEventType.Keyboard;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	KeyCharacter																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="KeyCharacter">KeyCharacter</see>.
		/// </summary>
		private char mKeyCharacter = char.MinValue;
		/// <summary>
		/// Get/Set the printable keyboard character.
		/// </summary>
		public char KeyCharacter
		{
			get { return mKeyCharacter; }
			set { mKeyCharacter = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	KeyCode																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="KeyCode">KeyCode</see>.
		/// </summary>
		private int mKeyCode = 0;
		/// <summary>
		/// Get/Set the key code received.
		/// </summary>
		public int KeyCode
		{
			get { return mKeyCode; }
			set { mKeyCode = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	KeyModifier																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="KeyModifier">KeyModifier</see>.
		/// </summary>
		private KeyModifierType mKeyModifier =
			KeyModifierType.None;
		/// <summary>
		/// Get/Set the modifiers for this key.
		/// </summary>
		public KeyModifierType KeyModifier
		{
			get { return mKeyModifier; }
			set { mKeyModifier = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}

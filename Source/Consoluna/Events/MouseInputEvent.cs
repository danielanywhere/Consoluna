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
	//*	MouseInputEventArgs																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Event arguments for handling a mouse event on Consoluna applications.
	/// </summary>
	public class MouseInputEventArgs : InputEventArgs
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
		/// Create a new instance of the MouseInputEventArgs item.
		/// </summary>
		public MouseInputEventArgs()
		{
			EventType = InputEventType.Mouse;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MouseX																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MouseX">MouseX</see>.
		/// </summary>
		private int mMouseX = 0;
		/// <summary>
		/// Get/Set the mouse X position.
		/// </summary>
		public int MouseX
		{
			get { return mMouseX; }
			set { mMouseX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MouseY																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MouseY">MouseY</see>.
		/// </summary>
		private int mMouseY = 0;
		/// <summary>
		/// Get/Set the mouse Y position.
		/// </summary>
		public int MouseY
		{
			get { return mMouseY; }
			set { mMouseY = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}

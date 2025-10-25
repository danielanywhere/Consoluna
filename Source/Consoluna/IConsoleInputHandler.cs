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

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	IConsoleInputHandler																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Input handler interface for ConsolePlus.
	/// </summary>
	public interface IConsoleInputHandler
	{
		//*-----------------------------------------------------------------------*
		//* Close																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Close any open connections and reset system settings.
		/// </summary>
		void Close();
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Initialize																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize the input handler.
		/// </summary>
		void Initialize();
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	InputInfo																															*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get/Set a reference to the input info collection assigned to this
		///// instance.
		///// </summary>
		//List<ConsoleInputEventArgs> InputInfo { get; set; }
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReadInput																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read any available input.
		/// </summary>
		/// <returns>
		/// Reference to an input event, if found. Otherwise, null.
		/// </returns>
		ConsoleInputEventArgs ReadInput();
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}

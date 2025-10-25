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
	//*	ConsoleInputMode																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of available input modes for ConsolePlus.
	/// </summary>
	public enum ConsoleInputMode
	{
		/// <summary>
		/// No input mode specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Non-blocking immediate character. The caller will poll for input
		/// directly on the host's schedule.
		/// </summary>
		DirectPoll,
		/// <summary>
		/// Blocking. The caller will wait for a specific value to be received,
		/// discarding other non-matching values.
		/// </summary>
		FilterWait,
		/// <summary>
		/// Non-blocking. The caller will register to receive input events
		/// as they occur at the console.
		/// </summary>
		EventDriven
	}
	//*-------------------------------------------------------------------------*

}

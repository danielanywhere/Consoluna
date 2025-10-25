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
	//*	ConsolunaInputEventType																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of event types recognized in Consoluna applications.
	/// </summary>
	public enum ConsolunaInputEventType
	{
		/// <summary>
		/// No event type specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// User keyboard event.
		/// </summary>
		Keyboard,
		/// <summary>
		/// Mouse event.
		/// </summary>
		Mouse,
		/// <summary>
		/// Window resize event.
		/// </summary>
		Resize
	}
	//*-------------------------------------------------------------------------*

}

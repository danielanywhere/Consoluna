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
using System.Timers;

using ConsolunaLib.Internal;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaShapeCollectionEventArgs																				*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Event arguments for console character collections.
	/// </summary>
	public class ConsolunaShapeCollectionEventArgs
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
		/// Create a new instance of the ConsolunaShapeCollectionEventArgs item.
		/// </summary>
		public ConsolunaShapeCollectionEventArgs()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaShapeCollectionEventArgs item.
		/// </summary>
		/// <param name="e">
		/// Reference to a generic event that has been captured.
		/// </param>
		public ConsolunaShapeCollectionEventArgs(
			CollectionChangeEventArgs<ConsolunaShapeItem> e)
		{
			if(e != null)
			{
				mActionName = e.ActionName;
				mAffectedItems.AddRange(e.AffectedItems);
				mHandled = e.Handled;
				mPropertyName = e.PropertyName;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ActionName																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ActionName">ActionName</see>.
		/// </summary>
		private string mActionName = "";
		/// <summary>
		/// Get/Set the name of the action on the collection.
		/// </summary>
		public string ActionName
		{
			get { return mActionName; }
			set { mActionName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AffectedItems																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AffectedItems">AffectedItems</see>.
		/// </summary>
		private List<ConsolunaShapeItem> mAffectedItems =
			new List<ConsolunaShapeItem>();
		/// <summary>
		/// Get a reference to the collection of items on the event.
		/// </summary>
		public List<ConsolunaShapeItem> AffectedItems
		{
			get { return mAffectedItems; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Handled																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Handled">Handled</see>.
		/// </summary>
		private bool mHandled = false;
		/// <summary>
		/// Get/Set a value indicating whether this event has been handled.
		/// </summary>
		public bool Handled
		{
			get { return mHandled; }
			set { mHandled = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PropertyName																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PropertyName">PropertyName</see>.
		/// </summary>
		private string mPropertyName = "";
		/// <summary>
		/// Get/Set the name of the affected property.
		/// </summary>
		public string PropertyName
		{
			get { return mPropertyName; }
			set { mPropertyName = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}

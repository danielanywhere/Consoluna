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

using ConsolunaLib.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsolunaLib.Internal
{
	//*-------------------------------------------------------------------------*
	//*	ChangeObjectCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ChangeObjectItem Items, which itself raises events when
	/// the contents of this collection or when the property of one of its items
	/// have changed.
	/// </summary>
	/// <typeparam name="T">
	/// Any type for which change handling will be configured.
	/// </typeparam>
	public class ChangeObjectCollection<T> : List<T>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnCollectionChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the CollectionChanged event when the contents of the collection
		/// have changed.
		/// </summary>
		/// <param name="e">
		/// Collection change event arguments.
		/// </param>
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs<T> e)
		{
			if(e != null && !e.Handled)
			{
				CollectionChanged?.Invoke(this, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnItemPropertyChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ItemPropertyChanged event when the value of an item property
		/// has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Dock panel property change event arguments.
		/// </param>
		protected virtual void OnItemPropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			if(e != null && !e.Handled)
			{
				ItemPropertyChanged?.Invoke(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an item to the collection.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be added.
		/// </param>
		public new void Add(T item)
		{
			if(item != null)
			{
				if(item is ChangeObjectItem @objectItem)
				{
					objectItem.PropertyChanged += OnItemPropertyChanged;
				}
				base.Add(item);
				OnCollectionChanged(
					new CollectionChangeEventArgs<T>("Add", item));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a range of items to the collection.
		/// </summary>
		/// <param name="collection">
		/// Reference to the collection of items to be added.
		/// </param>
		public new void AddRange(IEnumerable<T> collection)
		{
			List<T> affectedItems = null;

			if(collection != null)
			{
				affectedItems = new List<T>();
				foreach(T tItem in collection)
				{
					if(tItem is ChangeObjectItem @objectItem)
					{
						objectItem.PropertyChanged += OnItemPropertyChanged;
					}
					base.Add(tItem);
					affectedItems.Add(tItem);
				}
				if(affectedItems.Count > 0)
				{
					OnCollectionChanged(
						new CollectionChangeEventArgs<T>("Add", affectedItems));
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all of the elements from the property collection.
		/// </summary>
		public new void Clear()
		{
			List<T> affectedItems = new List<T>();

			affectedItems.AddRange(this);
			base.Clear();

			OnCollectionChanged(
				new CollectionChangeEventArgs<T>("Collection", affectedItems));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CollectionChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raised when the contents of the collection have changed.
		/// </summary>
		public event EventHandler<CollectionChangeEventArgs<T>> CollectionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Insert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert an item into the collection at the specified ordinal index.
		/// </summary>
		/// <param name="index">
		/// The 0-based index at which to insert the new item.
		/// </param>
		/// <param name="item">
		/// Reference to the item to be inserted.
		/// </param>
		public new void Insert(int index, T item)
		{
			if(index > -1 && item != null)
			{
				if(item is ChangeObjectItem @objectItem)
				{
					objectItem.PropertyChanged += OnItemPropertyChanged;
				}
				base.Insert(index, item);
				OnCollectionChanged(new CollectionChangeEventArgs<T>("Add", item));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertRange																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a range of items at the specified ordinal index.
		/// </summary>
		/// <param name="index">
		/// The 0-based index at which to insert the new item.
		/// </param>
		/// <param name="collection">
		/// Reference to the collection of items to insert.
		/// </param>
		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			int activeIndex = index;
			List<T> affectedItems = null;

			if(index > -1 && collection != null)
			{
				affectedItems = new List<T>();
				foreach(T tItem in collection)
				{
					if(tItem is ChangeObjectItem @objectItem)
					{
						objectItem.PropertyChanged += OnItemPropertyChanged;
					}
					base.Insert(activeIndex++, tItem);
					affectedItems.Add(tItem);
				}
				if(affectedItems.Count > 0)
				{
					OnCollectionChanged(
						new CollectionChangeEventArgs<T>("Add", affectedItems));
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ItemPropertyChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raised when the value of a property on an individual item has changed.
		/// </summary>
		public event EventHandler<PropertyChangeEventArgs>
			ItemPropertyChanged;
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PropertyName																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member value for <see cref="PropertyName">PropertyName</see>.
		///// </summary>
		//private string mPropertyName = "";
		///// <summary>
		///// Get/Set a property name to be associated with this collection for
		///// bubble-up events.
		///// </summary>
		//public string PropertyName
		//{
		//	get { return mPropertyName; }
		//	set { mPropertyName = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the first matching instance of the specified item from the
		/// collection.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be removed.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified item was removed from the
		/// collection.
		/// </returns>
		public new bool Remove(T item)
		{
			bool result = base.Remove(item);

			if(item != null && item is ChangeObjectItem @objectItem)
			{
				objectItem.PropertyChanged -= OnItemPropertyChanged;
			}
			if(result)
			{
				OnCollectionChanged(new CollectionChangeEventArgs<T>("Remove", item));
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAll																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all items matching the condition from the collection.
		/// </summary>
		/// <param name="match">
		/// Reference to the predicate condition to match.
		/// </param>
		/// <returns>
		/// Count of items removed.
		/// </returns>
		public new int RemoveAll(Predicate<T> match)
		{
			List<T> affectedItems = null;
			int count = 0;
			int index = 0;
			T item = default(T);
			int result = 0;

			if(match != null)
			{
				count = this.Count;
				if(count > 0)
				{
					affectedItems = new List<T>();
					for(index = 0; index < count; index++)
					{
						item = this[index];
						if(match(item))
						{
							//	Unregister the item.
							if(item is ChangeObjectItem @objectItem)
							{
								objectItem.PropertyChanged -= OnItemPropertyChanged;
							}
							//	Remove the item.
							base.RemoveAt(index);
							//	Discount.
							count--;
							//	Deindex.
							index--;
							result++;
							affectedItems.Add(item);
						}
					}
					if(affectedItems.Count > 0)
					{
						OnCollectionChanged(
							new CollectionChangeEventArgs<T>("Remove", affectedItems));
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAt																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the item at the specified ordinal index of the collection.
		/// </summary>
		/// <param name="index">
		/// The 0-based index at which the item will be removed.
		/// </param>
		public new void RemoveAt(int index)
		{
			T item = default(T);

			if(index > -1 && index < this.Count)
			{
				item = this[index];
				//	Unregister the item.
				if(item is ChangeObjectItem @objectItem)
				{
					objectItem.PropertyChanged -= OnItemPropertyChanged;
				}
				base.RemoveAt(index);
				OnCollectionChanged(new CollectionChangeEventArgs<T>("Remove", item));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveRange																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove a range of items from the collection.
		/// </summary>
		/// <param name="index">
		/// The index at which to begin removing.
		/// </param>
		/// <param name="count">
		/// The count of items to remove.
		/// </param>
		public new void RemoveRange(int index, int count)
		{
			List<T> affectedItems = null;
			T item = default(T);
			int remaining = 0;

			if(index > -1 && index < this.Count && count > 0)
			{
				remaining = Math.Min(count, this.Count - index);
				if(remaining > 0)
				{
					affectedItems = new List<T>();
					while(remaining > 0)
					{
						item = this[index];
						//	Unregister the item.
						if(item is ChangeObjectItem @objectItem)
						{
							objectItem.PropertyChanged -= OnItemPropertyChanged;
						}
						base.RemoveAt(index);
						affectedItems.Add(item);
						remaining--;
					}
					if(affectedItems.Count > 0)
					{
						OnCollectionChanged(
							new CollectionChangeEventArgs<T>("Remove", affectedItems));
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	ChangeObjectItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// The object whose changes will raise an event.
	/// </summary>
	public class ChangeObjectItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		////*-----------------------------------------------------------------------*
		////* OnCollectionChanged																										*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Raises the PropertyChanged event when the contents of a member
		///// collection have changed.
		///// </summary>
		///// <param name="propertyName">
		///// The name of the property whose value has changed.
		///// </param>
		//protected virtual void OnCollectionChanged(
		//	[CallerMemberName] string propertyName = "")
		//{
		//	OnPropertyChanged(propertyName);
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPropertyChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property change event arguments.
		/// </param>
		protected virtual void OnPropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			if(sender != null && e != null)
			{
				PropertyChanged?.Invoke(sender, e);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property has
		/// changed.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property whose value has changed.
		/// </param>
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = "")
		{
			if(propertyName?.Length > 0)
			{
				OnPropertyChanged(this, new PropertyChangeEventArgs()
				{
					PropertyName = propertyName
				});
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property value
		/// has changed.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property whose value has changed.
		/// </param>
		/// <param name="oldValue">
		/// The original value.
		/// </param>
		/// <param name="newValue">
		/// The new value.
		/// </param>
		protected virtual void OnPropertyChanged(
			string propertyName, object oldValue, object newValue)
		{
			OnPropertyChanged(this, new PropertyChangeEventArgs()
			{
				PropertyName = propertyName,
				OldValue = oldValue,
				NewValue = newValue
			});
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* PropertyChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raised when the value of a property has changed.
		/// </summary>
		public event EventHandler<PropertyChangeEventArgs> PropertyChanged;
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	DrawingSymbolCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of DrawingSymbolItem Items.
	/// </summary>
	public class DrawingSymbolCollection : List<DrawingSymbolItem>
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
		//*	Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a new item to the collection by member values.
		/// </summary>
		/// <param name="name">
		/// Name of the symbol.
		/// </param>
		/// <param name="symbol">
		/// The drawing symbol.
		/// </param>
		public DrawingSymbolItem Add(string name, char symbol)
		{
			DrawingSymbolItem result = new DrawingSymbolItem()
			{
				Name = (name?.Length > 0 ? name : ""),
				Symbol = symbol
			};

			this.Add(result);
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSymbolValue																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified symbol.
		/// </summary>
		/// <param name="name">
		/// Name of the symbol to find.
		/// </param>
		/// <returns>
		/// The value of the specified symbol, if found. Otherwise, '\0'.
		/// </returns>
		public char GetSymbolValue(string name)
		{
			string lowerName = "";
			char result = char.MinValue;
			DrawingSymbolItem symbolItem = null;

			if(name?.Length > 0)
			{
				lowerName = name.ToLower();
				symbolItem = this.FirstOrDefault(x => x.Name.ToLower() == lowerName);
				if(symbolItem != null)
				{
					result = symbolItem.Symbol;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SetSymbol																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified symbol, adding it to the collection if
		/// an item of the same name doesn't already exist.
		/// </summary>
		public void SetSymbol(string name, char symbol)
		{
			string lowerName = "";
			DrawingSymbolItem symbolItem = null;

			if(name?.Length > 0)
			{
				lowerName = name.ToLower();
				symbolItem = this.FirstOrDefault(x => x.Name.ToLower() == lowerName);
				if(symbolItem == null)
				{
					symbolItem = new DrawingSymbolItem()
					{
						Name = name
					};
					this.Add(symbolItem);
				}
				symbolItem.Symbol = symbol;
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	DrawingSymbolItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A named symbol character for use anywhere in the application context.
	/// </summary>
	public class DrawingSymbolItem
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
		/// Create a new instance of the DrawingSymbolItem item.
		/// </summary>
		public DrawingSymbolItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the DrawingSymbolItem item.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol.
		/// </param>
		/// <param name="symbol">
		/// The drawing symbol character.
		/// </param>
		public DrawingSymbolItem(string name, char symbol)
		{
			if(name?.Length > 0)
			{
				mName = name;
			}
			mSymbol = symbol;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Name">Name</see>.
		/// </summary>
		private string mName = "";
		/// <summary>
		/// Get/Set the name of this item.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Symbol																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Symbol">Symbol</see>.
		/// </summary>
		private char mSymbol = '\0';
		/// <summary>
		/// Get/Set the printable character symbol associated with this item.
		/// </summary>
		public char Symbol
		{
			get { return mSymbol; }
			set { mSymbol = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}

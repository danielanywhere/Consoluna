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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsolunaLib.Internal;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ColorInfo																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Color description for Consoluna applications.
	/// </summary>
	public class ColorInfo : ChangeObjectItem
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
		/// Create a new instance of the ColorInfo item.
		/// </summary>
		public ColorInfo()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ColorInfo item.
		/// </summary>
		/// <param name="red">
		/// Red channel value.
		/// </param>
		/// <param name="green">
		/// Green channel value.
		/// </param>
		/// <param name="blue">
		/// Blue channel value.
		/// </param>
		public ColorInfo(byte red, byte green, byte blue)
		{
			mRed = red;
			mGreen = green;
			mBlue = blue;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ColorInfo item.
		/// </summary>
		/// <param name="rgb">
		/// Red channel value.
		/// </param>
		public ColorInfo((byte red, byte green, byte blue) rgb)
		{
			mRed = rgb.red;
			mGreen = rgb.green;
			mBlue = rgb.blue;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ColorInfo item.
		/// </summary>
		/// <param name="hexColor">
		/// Hex color to initialize.
		/// </param>
		public ColorInfo(string hexColor)
		{
			SetColor(this, hexColor);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Blue																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Blue">Blue</see>.
		/// </summary>
		private byte mBlue = 0x00;
		/// <summary>
		/// Get/Set the blue channel color.
		/// </summary>
		public byte Blue
		{
			get { return mBlue; }
			set
			{
				bool bChanged = (mBlue != value);
				mBlue = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Equals																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this item's members are equal to the
		/// members of the caller's item.
		/// </summary>
		public override bool Equals(object o)
		{
			bool result = false;

			if(o is ColorInfo color)
			{
				if(color.mBlue == mBlue &&
					color.mGreen == mGreen &&
					color.mRed == mRed)
				{
					result = true;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FromHsl																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new RGB Color from HSL values.
		/// </summary>
		/// <param name="hsl">
		/// The hue, saturation, and brightness.
		/// </param>
		/// <returns>
		/// An RGB tuple.
		/// </returns>
		public static (byte red, byte green, byte blue) FromHsl((float hue,
			float saturation, float lightness) hsl)
		{
			return FromHsl(hsl.hue, hsl.saturation, hsl.lightness);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new RGB Color from HSL values.
		/// </summary>
		/// <param name="hue">
		/// The hue to convert.
		/// </param>
		/// <param name="saturation">
		/// The saturation to convert.
		/// </param>
		/// <param name="lightness">
		/// The lightness to convert.
		/// </param>
		/// <returns>
		/// An RGB tuple.
		/// </returns>
		public static (byte red, byte green, byte blue) FromHsl(float hue,
			float saturation, float lightness)
		{
			int b = 0;
			float C = (1f - Math.Abs(2f * lightness - 1f)) * saturation;
			int g = 0;
			float m = lightness - C / 2f;
			int r = 0;
			float X = C * (1f - Math.Abs((hue / 60f) % 2f - 1f));

			float r1 = 0f, g1 = 0f, b1 = 0f;

			if(hue >= 0f && hue < 60f)
			{
				r1 = C; g1 = X; b1 = 0f;
			}
			else if(hue >= 60f && hue < 120f)
			{
				r1 = X; g1 = C; b1 = 0f;
			}
			else if(hue >= 120f && hue < 180f)
			{
				r1 = 0f; g1 = C; b1 = X;
			}
			else if(hue >= 180f && hue < 240f)
			{
				r1 = 0f; g1 = X; b1 = C;
			}
			else if(hue >= 240f && hue < 300f)
			{
				r1 = X; g1 = 0f; b1 = C;
			}
			else if(hue >= 300f && hue < 360f)
			{
				r1 = C; g1 = 0f; b1 = X;
			}

			r = (int)Math.Round((r1 + m) * 255f);
			g = (int)Math.Round((g1 + m) * 255f);
			b = (int)Math.Round((b1 + m) * 255f);

			return ((byte)ClampByte(r), (byte)ClampByte(g), (byte)ClampByte(b));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetHashCode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the unique hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			int factor = 0;
			int result = 2025102301;

			factor = 0 - (int)((double)result * 0.25);

			result *= (factor + mRed.GetHashCode());
			result *= (factor + mGreen.GetHashCode());
			result *= (factor + mBlue.GetHashCode());
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Green																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Green">Green</see>.
		/// </summary>
		private byte mGreen = 0x00;
		/// <summary>
		/// Get/Set the green channel color.
		/// </summary>
		public byte Green
		{
			get { return mGreen; }
			set
			{
				bool bChanged = (mGreen != value);
				mGreen = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Red																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Red">Red</see>.
		/// </summary>
		private byte mRed = 0x00;
		/// <summary>
		/// Get/Set the red channel color.
		/// </summary>
		public byte Red
		{
			get { return mRed; }
			set
			{
				bool bChanged = (mRed != value);
				mRed = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SetColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the color's channel values from an RGB tuple.
		/// </summary>
		/// <param name="colsoleColor">
		/// Reference to the object to update.
		/// </param>
		/// <param name="color">
		/// The tuple to apply.
		/// </param>
		public static void SetColor(ColorInfo consoleColor,
			(byte red, byte green, byte blue) color)
		{
			if(consoleColor != null)
			{
				consoleColor.mRed = color.red;
				consoleColor.mGreen = color.green;
				consoleColor.mBlue = color.blue;
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Set the color's channel values from the caller's HTML hex code.
		/// </summary>
		/// <param name="consoleColor">
		/// Reference to the object to update.
		/// </param>
		/// <param name="hexColor">
		/// Hex color string to parse.
		/// </param>
		public static void SetColor(ColorInfo consoleColor, string hexColor)
		{
			string text = "";

			if(consoleColor != null && hexColor?.Length > 6 &&
				hexColor.StartsWith("#"))
			{
				consoleColor.mRed = Convert.ToByte(hexColor.Substring(1, 2), 16);
				consoleColor.mGreen = Convert.ToByte(hexColor.Substring(3, 2), 16);
				consoleColor.mBlue = Convert.ToByte(hexColor.Substring(5, 2), 16);
				consoleColor.OnPropertyChanged("Color");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToHsl																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the HSL representation of the caller's RGB value,
		/// </summary>
		/// <param name="color">
		/// Reference to the color to be converted.
		/// </param>
		/// <returns>
		/// A tuple containing the hue, saturation, and lightness components of an
		/// HSL color.
		/// </returns>
		public static (float hue, float saturation, float lightness) ToHsl(
			ColorInfo color)
		{
			float blue = 0f;
			float delta = 0f;
			float green = 0f;
			float hue = 0f;
			float lightness = 0f;
			float max = 0f;
			float min = 0f;
			float red = 0f;
			float saturation = 0f;

			if(color != null)
			{
				// Normalize RGB values to [0,1]
				red = (float)color.mRed / 255f;
				green = (float)color.mGreen / 255f;
				blue = (float)color.mBlue / 255f;

				max = Math.Max(red, Math.Max(green, blue));
				min = Math.Min(red, Math.Min(green, blue));
				delta = max - min;

				// Calculate Lightness
				lightness = (max + min) / 2f;

				hue = 0f;
				saturation = 0f;

				if(delta != 0f)
				{
					// Calculate Saturation
					if(max + min != 0f && max - min != 2f)
					{
						saturation = lightness < 0.5f ?
							delta / (max + min) :
							delta / (2f - max - min);
					}

					// Calculate Hue
					if(delta != 0f)
					{
						if(max == red)
						{
							hue = ((green - blue) / delta) % 6f;
						}
						else if(max == green)
						{
							hue = ((blue - red) / delta) + 2f;
						}
						else // max == bf
						{
							hue = ((red - green) / delta) + 4f;
						}
					}

					hue *= 60f;
					if(hue < 0f)
					{
						hue += 360f;
					}
				}
			}
			return (hue, saturation, lightness);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a string representation of this object.
		/// </summary>
		/// <returns>
		/// The string representation of this object.
		/// </returns>
		public override string ToString()
		{
			return $"#{mRed:x2}{mGreen:x2}{mBlue:x2}";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransferValues																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transfer the primitive member values of the source object to the
		/// target.
		/// </summary>
		/// <param name="source">
		/// Reference to the source object containing values to transfer.
		/// </param>
		/// <param name="target">
		/// Reference to the target object that will receive the update.
		/// </param>
		public static void TransferValues(ColorInfo source, ColorInfo target)
		{
			if(source != null && target != null)
			{
				target.mBlue = source.mBlue;
				target.mGreen = source.mGreen;
				target.mRed = source.mRed;
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}

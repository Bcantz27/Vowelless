//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

#ifndef _H_GUIDEFAULTCONTROLRENDER_
#define _H_GUIDEFAULTCONTROLRENDER_

class GuiControlProfile;

void renderRaisedBox(RectI &bounds, GuiControlProfile *profile);
void renderSlightlyRaisedBox(RectI &bounds, GuiControlProfile *profile);
void renderLoweredBox(RectI &bounds, GuiControlProfile *profile);
void renderSlightlyLoweredBox(RectI &bounds, GuiControlProfile *profile, bool active = true);
void renderBorder(RectI &bounds, GuiControlProfile *profile);
void renderFilledBorder( RectI &bounds, GuiControlProfile *profile );
void renderFilledBorder( RectI &bounds, ColorI &borderColor, ColorI &fillColor );
void renderSizableBitmapBordersFilled(RectI &bounds, S32 baseMultiplier, GuiControlProfile *profile); // DAW: Added
void renderSizableBitmapBordersFilledIndex(RectI &bounds, S32 startIndex, GuiControlProfile *profile);
void renderFixedBitmapBordersFilled(RectI &bounds, S32 baseMultiplier, GuiControlProfile *profile); // DAW: Added
void renderFixedBitmapBordersFilled(RectI &bounds, S32 startIndex, GuiControlProfile *profile);
void renderFixedBitmapBordersStretchYFilled(RectI &bounds, S32 baseMultiplier, GuiControlProfile *profile); // DAW: Added

#endif

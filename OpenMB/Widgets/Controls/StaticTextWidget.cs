﻿/*
-----------------------------------------------------------------------------
This source file is part of AdvancedMogreFramework
For the latest info, see https://github.com/cookgreen/AdvancedMogreFramework
Copyright (c) 2016-2020 Cook Green

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/
using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenMB.Widgets
{
    public class StaticText : Widget
    {
        protected TextAreaOverlayElement mTextArea;
        protected bool mFitToTray;
        public StaticText(string name, string caption, float width, bool specificColor, ColourValue color)
        {
            OverlayManager overlayMgr = OverlayManager.Singleton;
            element = overlayMgr.CreateOverlayElement("BorderPanel", name);
            element.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            element.HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;
            element.Height = 32;
            mTextArea = overlayMgr.CreateOverlayElement("TextArea", name + "/StaticTextCaption") as TextAreaOverlayElement;
            mTextArea.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;
            mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Center);
            mTextArea.Top = 10;
            mTextArea.FontName = "EngineFont";
            mTextArea.CharHeight = 18;
            mTextArea.SpaceWidth = 9;
            if(!specificColor)
            {
                mTextArea.Colour = new ColourValue(0.9f, 1f, 0.7f);
            }
            else
            {
                mTextArea.Colour = color;
            }
            ((OverlayContainer)element).AddChild(mTextArea);
            SetText(caption);
        }

        public string GetText()
        {
            return mTextArea.Caption;
        }

        public void SetText(string text)
        {
            mTextArea.Caption = text;
        }

        public override void CursorPressed(Mogre.Vector2 cursorPos)
        {
        }

        public bool _isFitToTray()
        {
            return mFitToTray;
        }
    }
}

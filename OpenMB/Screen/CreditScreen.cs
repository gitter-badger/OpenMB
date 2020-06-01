﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
    public class CreditScreen : Screen
    {
        private int time;
        private List<Widget> elements;
        private List<string> elementNames;
        private StringVector strCreditLst;
        private float alpha;

        public override event Action OnScreenExit;
        public override string Name
        {
            get
            {
                return "Credit";
            }
        }

        public CreditScreen()
        {
            time = 0;
            strCreditLst = new StringVector();
            StringBuilder creditBuilder = new StringBuilder();
            creditBuilder.AppendLine("OpenMB");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("CopyRight 2016-2020 Cook Green");
            creditBuilder.AppendLine("");
            strCreditLst.Add(creditBuilder.ToString());

            creditBuilder.Clear();
            creditBuilder.AppendLine("Design: Cook Green");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Programming: Cook Green");
            creditBuilder.AppendLine("");
            strCreditLst.Add(creditBuilder.ToString());

            creditBuilder.Clear();
            creditBuilder.AppendLine("All Credit List:");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Ogre - Open Source 3D Render Engine");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Steve 'sinbad' Streeting");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Matias 'dark_sylinc' Goldberg");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Eugene Golushkov");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Pavel 'paroj' Rojtberg");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Dave 'masterfalcon' Rogers");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Murat 'Wolfmanfx' Sari");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Philip 'spacegaier' Allgaier");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Assaf Raman");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("Mogre - C++/CLI Wrapper for Ogre");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("GantZ");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("MogreBites - GUI Library for Mogre");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("rains soft(andyhebear)");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("NAudio - DotNet Platform Sound Library");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("markheath");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("NVorbis - Ogg Format Support For NAudio");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("ioctlLR");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("OpenBrf - Powerful tool for importing/exporting M&B models and animations");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("mtarini");
            creditBuilder.AppendLine("");
            creditBuilder.AppendLine("");
            strCreditLst.Add(creditBuilder.ToString());
        }
        public override void Init(params object[] param)
        {
            elements = new List<Widget>();
            elementNames = new List<string>();
            UIManager.Instance.DestroyAllWidgets();
        }

        public override void Run()
        {
            UIManager.Instance.HideCursor();
        }

        public override void Update(float timeSinceLastFrame)
        {
            if (time >= 0 && time <= 1200)
            {
                if (!elementNames.Contains("lbCredit0"))
                {
                    elements.Add(UIManager.Instance.CreateStaticText(UIWidgetLocation.TL_NONE, "lbCredit0", strCreditLst[0]));
                    elements[0].OverlayElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
                    elements[0].OverlayElement.Left = 0.5f;
                    elements[0].OverlayElement.Top = 0.5f;
                    elementNames.Add("lbCredit0");
                }
                ColourValue elementColor = elements[0].OverlayElement.Colour;
                alpha = elementColor.a;
                if (alpha > 0.0f)
                {
                    alpha -= 0.0005f;
                    elements[0].OverlayElement.Colour = new ColourValue(
                        elementColor.r,
                        elementColor.g,
                        elementColor.b,
                        float.Parse(alpha.ToString("0.00")));
                }
            }
            else if (time > 1200 && time <= 2400)
            {
                if (elementNames.Contains("lbCredit0"))
                {
                    elements.Remove(elements.Find(o => o.Name == "lbCredit0"));
                    elementNames.Remove("lbCredit0");
                    UIManager.Instance.DestroyWidget("lbCredit0");
                }
                if (!elementNames.Contains("lbCredit1"))
                {
                    elements.Add(UIManager.Instance.CreateStaticText(UIWidgetLocation.TL_NONE, "lbCredit1", strCreditLst[1]));
                    elements[0].OverlayElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
                    elements[0].OverlayElement.Left = 0.5f;
                    elements[0].OverlayElement.Top = 0.5f;
                    elementNames.Add("lbCredit1");
                }
                ColourValue elementColor = elements[0].OverlayElement.Colour;
                alpha = elementColor.a;
                if (alpha > 0.0f)
                {
                    alpha -= 0.0005f;
                    elements[0].OverlayElement.Colour = new ColourValue(
                        elementColor.r,
                        elementColor.g,
                        elementColor.b,
                        float.Parse(alpha.ToString("0.00")));
                }
            }
            else if (time > 2400 && time <= 12000)
            {
                if (elementNames.Contains("lbCredit1"))
                {
                    elements.Remove(elements.Find(o => o.Name == "lbCredit1"));
                    elementNames.Remove("lbCredit1");
                    UIManager.Instance.DestroyWidget("lbCredit1");
                }
                if (!elementNames.Contains("lbCredit2"))
                {
                    elements.Add(UIManager.Instance.CreateStaticText(UIWidgetLocation.TL_NONE, "lbCredit2", strCreditLst[2]));
                    elements[0].OverlayElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
                    elements[0].OverlayElement.Left = 0.5f;
                    elements[0].OverlayElement.Top = 1.0f;
                    elementNames.Add("lbCredit2");
                }
                if (elements[0].OverlayElement.Top > -1.0f)
                {
                    elements[0].OverlayElement.Top -= 0.00025f;
                }
            }
            else
            {
                if (OnScreenExit != null)
                {
                    OnScreenExit();
                }
            }
            time++;
        }

        public override void Exit()
        {
            base.Exit();

            UIManager.Instance.DestroyAllWidgets();
            time = 0;
            elements.Clear();
            if (OnScreenExit != null)
            {
                OnScreenExit();
            }
        }

        public override void InjectMouseMove(MouseEvent arg)
        {
            base.InjectMouseMove(arg);
            Exit();
        }

        public override void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            base.InjectMousePressed(arg, id);
            Exit();
        }

        public override void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            base.InjectMouseReleased(arg, id);
            Exit();
        }

        public override void InjectKeyPressed(KeyEvent arg)
        {
            base.InjectKeyPressed(arg);
            Exit();
        }

        public override void InjectKeyReleased(KeyEvent arg)
        {
            base.InjectKeyReleased(arg);
            Exit();
        }
    }
}

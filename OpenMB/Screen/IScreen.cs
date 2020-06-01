﻿using Mogre;
using MOIS;
using OpenMB.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Screen
{
    public interface IScreen
    {
        bool IsVisible { get; }
        event Action OnScreenExit;
		event Action<string, string> OnScreenEventChanged;
        string Name { get; }
        List<Widget> UIWidgets { get; }

        void Init(params object[] param);
        void Run();
        void Update(float timeSinceLastFrame);
        void Exit();

        void InjectMouseMove(MouseEvent arg);
        void InjectMousePressed(MouseEvent arg, MouseButtonID id);
        void InjectMouseReleased(MouseEvent arg, MouseButtonID id);
        void InjectKeyPressed(KeyEvent arg);
        void InjectKeyReleased(KeyEvent arg);
        void Show();
        void Hide();
        bool CheckEnterScreen(Vector2 mousePos);
    }
}

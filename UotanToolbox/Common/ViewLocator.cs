﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SukiUI.Controls;
using SukiUI.Toasts;

namespace UotanToolbox.Common;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<object, Control> _controlCache = [];

    public Control Build(object data)
    {
        string fullName = data?.GetType().FullName;
        if (fullName is null)
        {
            return new TextBlock { Text = "Data is null or has no name." };
        }

        if (fullName.Contains("ApplicationInfo"))
        {
            fullName = "UotanToolbox.Features.Appmgr.AppmgrViewModel";
        }

        string name = fullName.Replace("ViewModel", "View");
        Type type = Type.GetType(name);
        if (type is null)
        {
            return new TextBlock { Text = $"No View For {name}." };
        }

        if (fullName.Contains("UotanToolbox.Features.Home.HomeView"))
        {
            if (!_controlCache.TryGetValue(data!, out Control ress))
            {
                ress ??= Global.homeView!;
                _controlCache[data!] = ress;
            }
        }

        if (!_controlCache.TryGetValue(data!, out Control res))
        {
            res ??= (Control)Activator.CreateInstance(type)!;
            _controlCache[data!] = res;
        }

        res.DataContext = data;
        return res;
    }

    public bool Match(object data)
    {
        return data is INotifyPropertyChanged;
    }
}
﻿using ReactiveUI;
using System;
using System.Reactive;
using ShoppingList.Model;

namespace ShoppingList.ViewModels
{
    public class ShoppingItemDisplay : ReactiveObject
    {
        private ShoppingItem _item;
        public ShoppingItem Item
        {
            get { return _item; }
            set { this.RaiseAndSetIfChanged(ref _item, value); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }

        public bool IsOwner => App.CurrentUser!.Id.Equals(Item.UserId);
        public string QuantityDisplay => Item.Quantity + " " + Item.Unit.ToString();
        public ReactiveCommand<Unit, bool> ToggleExpandedCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }
        public Action Editing { get; }
        public ShoppingItemDisplay(ShoppingItem item, Action editing)
        {
            _item = item;
            ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
            Editing = editing;
            EditCommand = ReactiveCommand.Create(() => editing());

        }
    }
}

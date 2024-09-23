﻿using ReactiveUI;
using ShoppingList.Loaders;
using ShoppingList.Models;
using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingList.ViewModels
{
    internal class GroceryListViewModel : ViewModelBase
    {
        public ObservableCollection<ShoppingItemDisplay> ShoppingList { get; }

        private bool _inputMode;
        public bool InputMode
        {
            get { return _inputMode; }
            set { this.RaiseAndSetIfChanged(ref _inputMode, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ObservableCollection<UnitType> Units { get; }

        private ShoppingItem _currentlyEditedItem;
        public ShoppingItem CurrentlyEditedItem
        {
            get { return _currentlyEditedItem; }
            set { this.RaiseAndSetIfChanged(ref _currentlyEditedItem, value); }
        }
        public int CurrentlyEditedItemIndex { get; set; }

        public ReactiveCommand<Unit, Unit> InputModeOnCommand { get; }
        public ReactiveCommand<Unit, Unit> InputModeOffCommand { get; }
        public GroceryListViewModel()
        {
            InputMode = false;
            _currentlyEditedItem = ShoppingItem.Empty;
            CurrentlyEditedItemIndex = -1;

            _errorMessage = "";
            Units = [.. (UnitType[])Enum.GetValues(typeof(UnitType))];

            ShoppingList = [.. ShoppingListLoader.LoadShoppingList().Select(item => new ShoppingItemDisplay(item))];
            ShoppingList.CollectionChanged += (_, _) => ShoppingListLoader.SaveShoppingList([.. ShoppingList.Select(display => display.Item)]);
            ShoppingList.ToList().ForEach(display => display.Editing += OnEditing);

            InputModeOnCommand = ReactiveCommand.Create(() => OnInputModeOn(ShoppingItem.Empty, -1));
            InputModeOffCommand = ReactiveCommand.Create(OnInputModeOff);
        }
        private void OnInputModeOn(ShoppingItem item, int index)
        {
            CurrentlyEditedItem = item;
            CurrentlyEditedItemIndex = index;
            InputMode = true;
        }
        private void OnInputModeOff()
        {
            InputMode = false;
            ErrorMessage = string.Empty;
        }

        internal void Save()
        {
            ShoppingItemDisplay newDisplay = new(CurrentlyEditedItem);
            newDisplay.Editing += OnEditing;

            if (CurrentlyEditedItemIndex < 0)
                ShoppingList.Add(newDisplay);
            else
                ShoppingList[CurrentlyEditedItemIndex] = newDisplay;

            InputMode = false;

            ShoppingListLoader.SaveShoppingList([.. ShoppingList.Select(display => display.Item)]);
        }
        internal void OnEditing(ShoppingItemDisplay display)
        {
            OnInputModeOn((display.Item.Clone() as ShoppingItem)!, ShoppingList.IndexOf(display));
        }
    }
}

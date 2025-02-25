﻿using ReactiveUI;
using ShoppingList.Model.Models;
using System.Collections.Generic;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<Page, ViewModelBase> _pages;

		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public MainViewModel(UserAccountModel accountModel)
        {
            _pages = new Dictionary<Page, ViewModelBase>()
            {
                { Page.Login, new LoginViewModel(accountModel, ChangePage, ShowLoading) },
                { Page.Register, new RegisterViewModel(accountModel, ChangePage, ShowLoading) },
                { Page.Main, new LoggedInViewModel(accountModel) },
            };

            _currentPage = _pages[Page.Login];
        }

        private void ChangePage(Page page)
        {
            CurrentPage = _pages[page];
        }
        private void ShowLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }

    public enum Page
    {
        Login,
        Register,
        Main
    }
}

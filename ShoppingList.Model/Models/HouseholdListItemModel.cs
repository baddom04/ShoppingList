﻿using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Models
{
    public class HouseholdListItemModel(Household household)
    {
        private readonly IApplicationService _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();
        private readonly IHouseholdService _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        public Household Household { get; } = household;
        public event Action? HouseholdChanged;

        public async Task ApplyAsync()
        {
            await _applicationService.ApplyAsync(Household.Id);
        }
        public async Task UpdateRelationshipAsync()
        {
            Household.Relationship = await _householdService.GetUserRelationshipAsync(Household.Id);
            HouseholdChanged?.Invoke();
        }
    }
}

using DepositUI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using DepositUI.Core.Data;

namespace DepositUI.Pages
{
    public partial class History
    {
        private const int columnNumber = 4;
        private int nextdeposit = 0;
        private List<DepositModel> deposits;

        [Inject]
        private IDepositService DepositService { get; set; }
        [Inject]
        private AuthenticationStateProvider Context { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await Context.GetAuthenticationStateAsync()).User;

            if (user != null && !user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo(
                    $"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
            }
            else
            {
                deposits = await this.DepositService.GetDepositsAsync(0, 16);
                nextdeposit = deposits.Count;
            }
        }

        private async Task LoadMore()
        {
            var loaded = await this.DepositService.GetDepositsAsync(nextdeposit, 16);
            deposits.AddRange(loaded);
            nextdeposit += loaded.Count;
        }
    }
}
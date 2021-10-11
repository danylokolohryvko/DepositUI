using DepositUI.BLL.Interfaces;
using DepositUI.Core.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Details
    {
        private List<DepositCalc> depositDetails;

        [Parameter]
        public int DepositId { get; set; }
        [Inject]
        private IDepositService DepositService { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }
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
                depositDetails = await this.DepositService.GetDepositDetailsAsync(DepositId);
            }
        }

        private async Task LoadDepositCSV(int depositId)
        {
            var stringCSV = await this.DepositService.GetDepositCSV(depositId);
            await JS.InvokeAsync<object>("SaveFile", $"deposit{depositId}.csv", stringCSV);
        }
    }
}
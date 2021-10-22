using DepositUI.Core.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Components
{
    public partial class DepositDetailsTable
    {
        [Parameter]
        public List<DepositCalculation> DepositDetails { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JS.InvokeVoidAsync("SetTableResize");
        }
    }
}

using DepositUI.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace DepositUI.Components
{
    public partial class NavTabs
    {
        [Parameter]
        public ModeType Mode { get; set; }
    }
}

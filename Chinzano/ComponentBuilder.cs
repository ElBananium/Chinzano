using Discord;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public class AdditionalComponentBuilder
    {
        public static ButtonService ButtonsService;

        public static MenuService MenusService;

        public static ModalService ModalsService;


        private ComponentBuilder _compBuilder;

        public AdditionalComponentBuilder()
        {
            _compBuilder = new ComponentBuilder();
            
        }

        public AdditionalComponentBuilder WithButton<T>(Dictionary<string,string> AdInfo = null) where T : ButtonBase
        {
            var button = ButtonsService.GetComponentByName<T>(AdInfo);
            _compBuilder.WithButton(button);
            return this;
        }

        public AdditionalComponentBuilder WithSelectMenu<T>(Dictionary<string, string> AdInfo = null) where T : MenuBase
        {
            var menu = MenusService.GetComponentByName<T>(AdInfo);
            _compBuilder.WithSelectMenu(menu);

            return this;
        }

        public MessageComponent Build()
        {
            return _compBuilder.Build();

        }

        public static ModalBuilder GetModal<T>(Dictionary<string, string> AdInfo = null) where T : ModalBase
        {
            return ModalsService.GetComponentByName<T>(AdInfo);

            
        }




    }
}

using ArchnemesisRecipies.Components;
using ArchnemesisRecipies.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ArchnemesisRecipies.Utility
{
    public interface IMouseHandler
    {
        public List<ArchnemesisModViewModel> Mods { get; }
        public IEnumerable<ArchnemesisModViewModel> SelectedMods { get; }
        Action<ArchnemesisModViewModel, MouseEventArgs> OnClick { get; }
        void SetTooltipData(ArchnemesisModViewModel mod, TooltipContainer _tooltip);
    }

    public abstract class MouseHandlerBase : IMouseHandler
    {
        public List<ArchnemesisModViewModel> Mods { get; }
        public IEnumerable<ArchnemesisModViewModel> SelectedMods => Mods.Where(x => x.Selected);
        public abstract Action<ArchnemesisModViewModel, MouseEventArgs> OnClick { get; }
        public void SetTooltipData(ArchnemesisModViewModel mod, TooltipContainer _tooltip)
        {
            ArchnemesisModViewModel tooltipData = null;
            if (mod is { MouseOver: true } && !SelectedMods.Contains(mod))
            {
                tooltipData = mod;
            }
            _tooltip.SetModel(tooltipData);
        }
    }

    public class HighlightHandler : MouseHandlerBase
    {
        public override Action<ArchnemesisModViewModel, MouseEventArgs> OnClick => (m, e) =>
        {
            if (e is { Button: 1} click)
            {

            }

            // remove all styles
            Mods.ForEach(x => x.HighlightStyle = string.Empty);            

            // set selected styles
            HighlightComponents(Mods.Where(x => x.Selected), 0);

            // parent styles
            HighlightParents(Mods.Where(x => x.Selected).SelectMany(x => x.ComponentOf), 1);
        };

        private void HighlightComponents(IEnumerable<ArchnemesisModViewModel> mods, int depth)
        {
            foreach (var mod in mods)
            {
                if (string.IsNullOrEmpty(mod.HighlightStyle))
                {
                    mod.HighlightStyle = depth switch
                    {
                        0 => "selected",
                        _ => $"component-{depth}"
                    };
                }

                HighlightComponents(mod.Components, depth + 1);
            }
        }

        private void HighlightParents(IEnumerable<ArchnemesisModViewModel> mods, int depth)
        {
            foreach (var mod in mods)
            {
                if (string.IsNullOrEmpty(mod.HighlightStyle))
                {
                    mod.HighlightStyle = $"parent-{depth}";
                }

                HighlightParents(mod.ComponentOf, depth + 1);
            }
        }
    }

    public class RecipeHandler : MouseHandlerBase
    {
        public override Action<ArchnemesisModViewModel, MouseEventArgs> OnClick => throw new NotImplementedException();
    }
}

window.onload = function () {
    document.addEventListener("contextmenu", (e) => {
        e.preventDefault();
    }, false);
}

function copyClipboard(value) {
    navigator.clipboard.writeText(value);
}

function positionTooltip(tooltipFor, visibility) {
    let spacing = 10;
    let $tooltip = $(`#component-tooltip`);
    if (visibility) {
        $tooltip.attr('hidden', false);
        let $anchor = $(`#${tooltipFor} img.bordered`);

        let parentHeight = $('#mod-list').height();
        let windowWidth = $(window).width();

        let anchor = $anchor.get(0).getBoundingClientRect();

        let relativeCenters = alignCenterRelativeTo($tooltip, $anchor);
        let top = 0;
        let left = 0;

        // bottom if available space, else top
        if ($tooltip.outerHeight() + anchor.bottom + spacing < parentHeight) {
            top = anchor.bottom + spacing;
            left = relativeCenters.x;
        } else {
            top = anchor.top - spacing - $tooltip.outerHeight();
            left = relativeCenters.x;
        }

        // slide into complete view
        if (left <= spacing) {
            left = spacing;
        } else {
            let overflow = (left + $tooltip.width() + spacing) - windowWidth;
            if (overflow > 0) {
                left -= (overflow + spacing * 2);
            }            
        }

        $tooltip.css({ top: `${top}px`, left: `${left}px` });
        $tooltip.css({ opacity: '1' });
    } else {
        $tooltip.css({ opacity: '0' });
        $tooltip.css({ top: `0px`, left: `0px` });
        $tooltip.attr('hidden', true);
    }
}

function alignCenterRelativeTo($element, $relativeTo) {
    return {
        x: $relativeTo.offset().left + ($relativeTo.width() / 2) - ($element.width() / 2),
        y: $relativeTo.offset().top + ($relativeTo.height() / 2) - ($element.height() / 2)
    };
}

function toggleRowCollapsed(row) {
    let $tier = $(`#mod-tier-${row}`).find('h3, .mods');
    if ($tier.hasClass('expanded')) {
        $tier.removeClass('expanded');
        $tier.addClass('collapsed');
    } else {
        $tier.removeClass('collapsed');
        $tier.addClass('expanded');
    }
}
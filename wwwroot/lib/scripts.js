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
        let $window = $('#mod-list');
        let anchor = $anchor.get(0).getBoundingClientRect();

        let relativeCenters = alignCenterRelativeTo($tooltip, $anchor);
        let top = 0;
        let left = 0;

        // bottom if available space, else top
        if ($tooltip.outerHeight() + anchor.bottom + spacing < $window.height()) {
            top = anchor.bottom + spacing;
            left = relativeCenters.x;
        } else {
            top = anchor.top - spacing - $tooltip.outerHeight();
            left = relativeCenters.x;
        }

        // slide into complete view
        if (left <= spacing) {
            left = spacing;
        } else if (left + $tooltip.width() + spacing >= $window.width()) {
            let extra = $window.width() - (left + $tooltip.width() + spacing);
            left -= extra;
        }

        $tooltip.css({ top: `${top}px`, left: `${left}px` });
        $tooltip.css({ opacity: '1' });
    } else {
        $tooltip.attr('hidden', true);
        $tooltip.css({ opacity: '0' });
        $tooltip.css({ top: `0px`, left: `0px` });
    }
}

function alignCenterRelativeTo($element, $relativeTo) {
    let l1 = $relativeTo.offset().left,
        x1 = $relativeTo.width() / 2,
        t1 = $relativeTo.offset().top,
        y1 = $relativeTo.height() / 2;
    let l2 = $element.offset().left,
        x2 = $element.width() / 2,
        t2 = $element.offset().top,
        y2 = $element.height() / 2;

    return {
        x: (l1 + x1) - (l2 + x2),
        y: (t1 + y1) - (t2 + y2)
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
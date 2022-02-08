function getName($row) {
    return $row.find('td:nth-of-type(1)').text();
}

function getLink($row) {
    var imgSrc = $row.find('td:nth-of-type(1) img')[0].src;
    return imgSrc.substr(imgSrc.lastIndexOf('/') + 1).split('?')[0];
}

function getMod($row) {
    return $row.find('td:nth-of-type(2) span.explicitMod').text();
}

function getRewardType($row) {
    return $.map($row.find('td:nth-of-type(2) span.currency'), v => $(v).text()).join(' ');
}

function getComponents($row) {
    return $.map($row.find('td:last-child div'), function ($div) {
        return $($div).find('a:nth-of-type(2)').text();
    });
}

function getEffect($row) {
    return $row.find('td:nth-of-type(2)').clone().children().remove().end().text();
}

function getJson() {
    var map = $.map($($0).find('tbody tr'), function (value) {
        var $row = $(value);

        var data = {
            "Name": getName($row),
            "Image": getLink($row),
            "Mod": getMod($row),
            "Type": getRewardType($row),
            "Components": getComponents($row),
            "Effect": getEffect($row)
        }
        return data;
    });

    return JSON.stringify(map);
}
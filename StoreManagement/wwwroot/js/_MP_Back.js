////const { debug } = require("console");

var MediaWidth = {
    MoreThan: function (nWidth, orEqual) {
        orEqual = Boolean(orEqual);
        nWidth = nWidth + (orEqual ? 0 : 1);
        return window.matchMedia('(min-width: ' + nWidth + 'px)').matches;
    },
    LessThan: function (nWidth, orEqual) {
        orEqual = Boolean(orEqual);
        nWidth = nWidth - (orEqual ? 0 : 1);
        return window.matchMedia('(max-width: ' + nWidth + 'px)').matches;
    }
};

$(function () {
    var $body = $('body');

    var $SITE_NAVSIDE = $('#SITE_NAVSIDE');
    var $SITE_CONTENT = $('#SITE_CONTENT');
    var $SITE_OVERLAY = $('#SITE_OVERLAY');
    var $MENU_TRIGGER = $('#MENU_TRIGGER');

    var class_CollapseMenu = 'side-collapsed';
    var class_SmallHeader = 'head-min';

    $MENU_TRIGGER.on('click', function () { $body.toggleClass(class_CollapseMenu); $(this).blur(); });
    $SITE_OVERLAY.on('click', function () { $body.addClass(class_CollapseMenu); });

    var Menu_Preparing = function () {
        $SITE_NAVSIDE.css('transition', 'unset');
        $SITE_OVERLAY.css('transition', 'unset');
        if (MediaWidth.LessThan(1200)) $body.addClass(class_CollapseMenu);
        else $body.removeClass(class_CollapseMenu);
        setTimeout(function () {
            $SITE_NAVSIDE.css('transition', '');
            $SITE_OVERLAY.css('transition', '');
        }, 300);
    }
    var Header_Preparing = function () {
        var nPositionY_Scroll = $(window).scrollTop();
        if (nPositionY_Scroll > 100) $SITE_CONTENT.addClass(class_SmallHeader);
        else $SITE_CONTENT.removeClass(class_SmallHeader);
    }

    window.addEventListener('resize', Menu_Preparing);
    window.addEventListener('scroll', Header_Preparing);

    Menu_Preparing();
    //Header_Preparing();

    $('.treeview').click(function () {
        console.log($(this).children('ul.treeview-menu'));
        $(this).children('ul.treeview-menu').toggle(200);
    });

    $('.treeview').parent().children('ul.treeview-menu').toggle(200);

});
